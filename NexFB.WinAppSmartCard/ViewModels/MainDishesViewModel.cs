// --------------------------------------------------------------------------
// <copyright file="MainDishesViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.16">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Pos.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using WPFMessageBoxCustomControl.Enums;

    /// <summary>
    /// Main Dishes ViewModel.
    /// </summary>
    public class MainDishesViewModel : MainDishModelProperty
    {
        private static DispatcherTimer digitalClockDispatcher = new DispatcherTimer();
        private MainViewModel _mainViewModel;

        public MainDishesViewModel()
        {
            EditMenu = new RelayCommand((y) => ExecuteEditMenu(y));
            ConfirmCommand = new RelayCommand((y) => ExecuteConfirmCommand());
            RemoveEmployeeOrder = new RelayCommand((y) => ExecuteRemoveEmployeeOrder(y));
            ShowOrderScreen();
        }

        #region Properties

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                _mainViewModel = value;
                this.OnPropertyChanged("MainViewModel");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Employee To Order List.
        /// </summary>
        /// <param name="employeeInfo">Employee Info.</param>
        /// <param name="mainViewModel">Main ViewModel.</param>
        public void AddEmployeeOrderToList(Employee employeeInfo, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            if (OrderOwner == null)
            {
                OrderOwner = employeeInfo;
            }

            if (ScreenOrderVisible == ConstantValue.Visible)
            {
                if (ObservableListEmployee != null)
                {
                    if (!ObservableListEmployee.Any(x => x.SysId == employeeInfo.SysId))
                    {
                        employeeInfo.OrderNo = ObservableListEmployee.Count + 1;
                        employeeInfo.CurrentCulture = mainViewModel.CurrentCulture;
                        HideMenuList(employeeInfo);
                        GetAlternativeFood(employeeInfo);
                        ProcessAddDataInCurrentThread(employeeInfo);
                    }
                    else
                    {
                        this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_AlreadyThisOrder);
                    }
                }
            }
            else
            {
                DisplayEmployeeHasAlreadyOrdered(employeeInfo, mainViewModel);
            }
        }

        public void CheckDataForLanguageChange(Employee employeeInfo, MainViewModel mainViewModel)
        {
            if (ObservableListEmployee.Count == 1)
            {
                ObservableListEmployee = new ObservableCollection<Employee>();
                AddEmployeeOrderToList(employeeInfo, mainViewModel);
            }
        }

        /// <summary>
        /// Display info for employee who has already ordered.
        /// </summary>
        /// <param name="employeeInfo">Employee Info.</param>
        /// <param name="mainViewModel">Main ViewModel.</param>
        public void DisplayEmployeeHasAlreadyOrdered(Employee employeeInfo, MainModelProperty mainViewModel)
        {
            if (ObservableListEmployee.Count == 0)
            {
                var getEmployeeOrderBySysId = new GetMyOrderBySysIdCriteria() { SysId = OrderOwner.SysId };
                List<SCEmployeeOrder> employeeOrders = this.SmartCardOrderService.GetMyOrderBySysId(getEmployeeOrderBySysId);
                if (employeeOrders != null)
                {
                    SCEmployeeOrder employeeOrder = employeeOrders.FirstOrDefault();
                    OrderNo = employeeOrder.OrderNo;
                    employeeInfo.OrderNo = 1;
                    employeeInfo.MenuVnmName = employeeOrder.MealVnmName;
                    employeeInfo.MenuEngName = employeeOrder.MealEngName;
                    employeeInfo.CurrentCulture = mainViewModel.CurrentCulture;
                    employeeInfo.DisplayNotifyMenuName = employeeInfo.DisplayMenuName;
                    employeeInfo.GroupOrderNickName = employeeOrder.OrderNickName;
                    CountDownTimer = 30;
                    TimerDigitalClock();
                }

                GetAlternativeFood(employeeInfo);
                ProcessAddDataInCurrentThread(employeeInfo);
            }
            else
            {
                mainViewModel.MainTabReadOnly = true;
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ReceivedOrder);
            }
        }

        /// <summary>
        /// Get alternative Food.
        /// </summary>
        /// <param name="employeeInfo">Employee Info.</param>
        public void GetAlternativeFood(Employee employeeInfo)
        {
            var getAlternativeFoodCriteria = new GetAlternativeFoodBySysIdCriteria() { SysId = employeeInfo.SysId };
            List<FoodOrder> alternativeFood = this.SmartCardOrderService.GetAlternativeFoodBySysId(getAlternativeFoodCriteria);
            if (alternativeFood != null)
            {
                List<Food> foodMapping = Mapper.Map<List<FoodOrder>, List<Food>>(alternativeFood);
                Food currentMainFood = new Food()
                {
                    FoodId = int.MaxValue,
                    FoodVnmName = employeeInfo.MenuVnmName,
                    FoodEngName = employeeInfo.MenuEngName,
                    CurrentCulture = MainViewModel.CurrentCulture
                };

                SelectedFood = currentMainFood;
                foodMapping.Insert(0, currentMainFood);
                foodMapping.ForEach(x => x.CurrentCulture = MainViewModel.CurrentCulture);
                employeeInfo.ListAlternativeFood = foodMapping;
            }
        }

        /// <summary>
        /// Process add data.
        /// </summary>
        /// <param name="employeeInfo">Employee Info.</param>
        public void ProcessAddDataInCurrentThread(Employee employeeInfo)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                ObservableListEmployee.Add(employeeInfo);
                ToggleSideMenu();
                CheckAllowChangeLanguage();
            }));
        }

        public void CheckAllowChangeLanguage()
        {
            MainViewModel.IsAllowChangeLanguage = this.ObservableListEmployee.Count < 2;
        }

        public void CancelTimerDispatcher()
        {
            MainDishTimerLive = false;
            digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        public void ShowOrderScreen()
        {
            ScreenOrderVisible = ConstantValue.Visible;
            ScreenNotificationVisible = ConstantValue.Hidden;
        }

        public void ShowNotificationScreen()
        {
            ScreenOrderVisible = ConstantValue.Hidden;
            ScreenNotificationVisible = ConstantValue.Visible;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Remove employee from list.
        /// </summary>
        /// <param name="value">Employee sysId.</param>
        private void ExecuteRemoveEmployeeOrder(object value)
        {
            if (value != null)
            {
                int employeeSysId = (int)value;
                Employee employeeRemoved = ObservableListEmployee.Where(x => x.SysId == employeeSysId).SingleOrDefault();
                ObservableListEmployee.Remove(employeeRemoved);
                if (employeeRemoved.SysId == OrderOwner.SysId)
                {
                    OrderOwner = ObservableListEmployee.FirstOrDefault();
                    this.MainViewModel.OrderOwner = this.OrderOwner;
                }

                UpdateOrderNo();
                ToggleSideMenu();
                CheckAllowChangeLanguage();
            }
        }

        private void UpdateOrderNo()
        {
            for (int i = 0; i < ObservableListEmployee.Count; i++)
            {
                ObservableListEmployee[i].OrderNo = i + 1;
            }
        }

        /// <summary>
        /// Execute Edit Menu.
        /// </summary>
        /// <param name="value">The menu id.</param>
        private void ExecuteEditMenu(object value)
        {
            if (value != null)
            {
                WPFMessageBoxResult editOrderConfirm = this.ShowWarningEditOrder(MainViewModel.CurrentCulture);
                if (editOrderConfirm == WPFMessageBoxResult.Yes)
                {
                    int employeeSysId = (int)value;
                    Employee selectedEmployee = ObservableListEmployee.Where(x => x.SysId == employeeSysId).SingleOrDefault();
                    ShowMenuList(selectedEmployee);
                }
            }
        }

        /// <summary>
        /// Confirm Command.
        /// </summary>
        private void ExecuteConfirmCommand()
        {
            if (ObservableListEmployee.Count == 0)
            {
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ContainEmployee);
            }
            else
            {
                var orderDetails = new StringBuilder(ConstantValue.BeginRootTag);

                if (ObservableListEmployee.Any() && ObservableListEmployee.Count > 0)
                {
                    foreach (var employee in ObservableListEmployee)
                    {
                        DisplayOrderedMeal(employee);
                        if (employee.SelectedValueDropDown == 0 || employee.SelectedValueDropDown == int.MaxValue)
                        {
                            orderDetails.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, 1, 1);
                        }
                        else
                        {
                            orderDetails.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, employee.SelectedValueDropDown, 0);
                        }
                    }
                }

                var insertOrderCriteria = new InsertOrderMealByUserCriteria()
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy = OrderOwner.SysId,
                    TakeAway = false,
                    ListDetail = orderDetails.Append(ConstantValue.EndRootTag).ToString()
                };

                OrderResult result = this.SmartCardOrderService.InsertOrderFood(insertOrderCriteria);
                if (result.CommandResult.Code == 0)
                {
                    OrderSuccessAction(result);
                }
                else
                {
                    this.ShowErrorMessage(MainViewModel.CurrentCulture, result.CommandResult.Message);
                }
            }
        }

        private void OrderSuccessAction(OrderResult result)
        {
            OrderNo = result.OrderNo;
            MainViewModel.SendMessage(result.OrderId);
            MainViewModel.MainTabReadOnly = true;

            CountDownTimer = 30;
            ShowNotificationScreen();
            TimerDigitalClock();
        }

        /// <summary>
        /// Display order info after ordered.
        /// </summary>
        /// <param name="employee">Selected Employee.</param>
        private void DisplayOrderedMeal(Employee employee)
        {
            if (employee.SelectedValueDropDown > 0 && employee.SelectedValueDropDown != int.MaxValue)
            {
                if (MainViewModel.CurrentCulture == ConstantValue.VietNamCulture)
                {
                    employee.DisplayNotifyMenuName = employee.ListAlternativeFood.Where(x => x.FoodId == employee.SelectedValueDropDown).SingleOrDefault().FoodVnmName;
                }
                else
                {
                    employee.DisplayNotifyMenuName = employee.ListAlternativeFood.Where(x => x.FoodId == employee.SelectedValueDropDown).SingleOrDefault().FoodEngName;
                }
            }
            else
            {
                employee.DisplayNotifyMenuName = employee.DisplayMenuName;
            }
        }

        private void HideMenuList(Employee employeeInfo)
        {
            employeeInfo.FoodInfoVisibility = ConstantValue.Visible;
            employeeInfo.FoodListVisibility = ConstantValue.Hidden;
        }

        private void ShowMenuList(Employee employeeInfo)
        {
            employeeInfo.FoodInfoVisibility = ConstantValue.Hidden;
            employeeInfo.FoodListVisibility = ConstantValue.Visible;
        }

        private void TimerDigitalClock()
        {
            digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            digitalClockDispatcher.Tick += new EventHandler(TimerTick);
            digitalClockDispatcher.Start();
        }

        /// <summary>
        /// Timer click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            CountDownTimer -= 1;
            MainDishTimerLive = true;
            if (CountDownTimer == 0)
            {
                if (MainViewModel != null)
                {
                    MainDishTimerLive = false;
                    MainViewModel.ExecuteReturnToStandByScreen();
                }

                digitalClockDispatcher.Stop();
            }
        }

        private void ToggleSideMenu()
        {
            if (this.ObservableListEmployee.Count > 1)
            {
                MainViewModel.SideButtonEnable = false;
            }
            else
            {
                MainViewModel.SideButtonEnable = true;
            }
        }

        #endregion
    }
}
