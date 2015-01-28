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
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Enums;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using WPFMessageBoxCustomControl.Enums;

    /// <summary>
    /// Main Dishes ViewModel.
    /// </summary>
    public class MainDishesViewModel : MainDishModelProperty
    {
        private static DispatcherTimer _digitalClockDispatcher = new DispatcherTimer();
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

            if (ScreenOrderVisible == CommonResources.Visible)
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
                DisplayEmployeeHasAlreadyOrdered(employeeInfo.IsLocal);
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
        ///  Display info for employee who has already ordered.
        /// </summary>
        /// <param name="employeeInLocal">Check employee is expat or local.</param>
        public void DisplayEmployeeHasAlreadyOrdered(bool employeeInLocal)
        {
            MainViewModel.ChangeLanguageAccordingToEmployeeType(employeeInLocal, true);
            if (ObservableListEmployee.Count == 0)
            {
                var getEmployeeOrderBySysId = new GetMyOrderBySysIdCriteria() { SysId = OrderOwner.SysId };
                List<SCEmployeeOrder> employeeOrders = this.SmartCardOrderService.GetMyOrderBySysId(getEmployeeOrderBySysId);
                if (employeeOrders != null && employeeOrders.Count > 0)
                {
                    //// Get latest order.
                    int maxValue = employeeOrders.Where(x => !x.IsSideDish).Max(x => x.OrderId);
                    SCEmployeeOrder employeeOrder = employeeOrders.Where(x => x.OrderId == maxValue).FirstOrDefault();

                    var getOrderDetailsCriteria = new GetOrderDetailsCriteria() { OrderId = employeeOrder.OrderId };
                    Order order = KitchenService.GetOrderDetails(getOrderDetailsCriteria);
                    if (order != null)
                    {
                        OrderNo = order.OrderNo;
                        List<OrderMenu> listOrder = order.ListMenu;
                        if (listOrder != null && listOrder.Count > 0)
                        {
                            foreach (var item in listOrder)
                            {
                                Employee employee = new Employee()
                                {
                                    OrderNo = ObservableListEmployee.Count + 1,
                                    MenuVnmName = item.MenuVnmName,
                                    MenuEngName = item.MenuEngName,
                                    CurrentCulture = MainViewModel.CurrentCulture,
                                    Quantity = item.QuantityOrder,
                                    NickName = item.NickName,
                                    GroupOrderNickName = employeeOrder.OrderNickName
                                };
                                employee.DisplayNotifyMenuName = string.Format("{0} x {1}", employee.DisplayMenuName, employee.Quantity);
                                ObservableListEmployee.Add(employee);
                            }

                            OrderOwner.GroupOrderNickName = order.NickName;
                            OrderOwner = this.OrderOwner;
                            CountDownTimer = ConstantValue.CountDownTimer;
                            TimerDigitalClock();
                        }
                    }
                }
            }
            else
            {
                MainViewModel.MainTabReadOnly = true;
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
                    FoodChineseName = employeeInfo.MenuEngName,
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
                DeleteButtonVisible = AllowDeleteMemberInOrder();
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
            _digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        public void ShowOrderScreen()
        {
            ScreenOrderVisible = CommonResources.Visible;
            ScreenNotificationVisible = CommonResources.Hidden;
        }

        public void ShowNotificationScreen()
        {
            ScreenOrderVisible = CommonResources.Hidden;
            ScreenNotificationVisible = CommonResources.Visible;
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
                if (ObservableListEmployee.Count > 1)
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
                    DeleteButtonVisible = AllowDeleteMemberInOrder();
                    CheckAllowChangeLanguage();
                }
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
            MainViewModel.UpdateOrderOwnerInfoAfterOrderSuccessful();
            MainViewModel.SendMessage(result.OrderId);
            MainViewModel.MainTabReadOnly = true;
            MainViewModel.SideButtonEnable = true;
            CountDownTimer = ConstantValue.CountDownTimer;
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
                if (MainViewModel.CurrentCulture == (int)LanguageLocalization.VietNamCulture)
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
            employeeInfo.FoodInfoVisibility = CommonResources.Visible;
            employeeInfo.FoodListVisibility = CommonResources.Hidden;
        }

        private void ShowMenuList(Employee employeeInfo)
        {
            employeeInfo.FoodInfoVisibility = CommonResources.Hidden;
            employeeInfo.FoodListVisibility = CommonResources.Visible;
        }

        private void TimerDigitalClock()
        {
            _digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            _digitalClockDispatcher.Tick += new EventHandler(TimerTick);
            _digitalClockDispatcher.Start();
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

                _digitalClockDispatcher.Stop();
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

        private string AllowDeleteMemberInOrder()
        {
            return ObservableListEmployee.Count > 1 ? CommonResources.Visible : CommonResources.Hidden;
        }

        #endregion
    }
}
