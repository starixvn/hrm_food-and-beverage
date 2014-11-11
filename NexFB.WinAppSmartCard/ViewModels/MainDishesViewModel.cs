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
    using System.Windows.Input;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Pos.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using WPFMessageBoxCustomControl.Enums;

    /// <summary>
    /// Main Dishes ViewModel.
    /// </summary>
    public class MainDishesViewModel : ViewModelBase
    {
        private const string _OrderDetails = "<User SysId='{0}' MenuId='{1}' IsMainMenu='{2}'></User>";
        private const string _BeginRootTag = "<ROOT>";
        private const string _EndRootTag = "</ROOT>";
        private static DispatcherTimer digitalClockDispatcher = new DispatcherTimer();

        private ObservableCollection<Employee> _observableListEmployee = new ObservableCollection<Employee>();
        private Employee _orderOwner;
        private Food _selectedFood;
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private int _orderNo;
        private int _countDownTimer;
        private bool _mainDishTimerLive;
        private MainViewModel _mainViewModel;
        private bool _takeAwayChecked;

        public MainDishesViewModel()
        {
            EditMenu = new RelayCommand((x) => CanExecuteEditMenu(), (y) => ExecuteEditMenu(y));
            ConfirmCommand = new RelayCommand((x) => CanExecuteConfirmCommand(), (y) => ExecuteConfirmCommand());
            RemoveEmployeeOrder = new RelayCommand((x) => CanExecuteRemoveEmployeeOrder(), (y) => ExecuteRemoveEmployeeOrder(y));
            ShowOrderScreen();
            TakeAwayChecked = false;
        }

        #region Properties

        public ICommand RemoveEmployeeOrder { get; set; }

        public ICommand EditMenu { get; set; }

        public ICommand ConfirmCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public Food SelectedFood
        {
            get
            {
                return _selectedFood;
            }

            set
            {
                _selectedFood = value;
                this.OnPropertyChanged("SelectedFood");
            }
        }

        public bool TakeAwayChecked
        {
            get
            {
                return _takeAwayChecked;
            }

            set
            {
                _takeAwayChecked = value;
                this.OnPropertyChanged("TakeAwayChecked");
            }
        }

        public bool MainDishTimerLive
        {
            get
            {
                return _mainDishTimerLive;
            }

            set
            {
                _mainDishTimerLive = value;
                this.OnPropertyChanged("MainDishTimerLive");
            }
        }

        public int OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
                this.OnPropertyChanged("OrderNo");
            }
        }

        public int CountDownTimer
        {
            get
            {
                return _countDownTimer;
            }

            set
            {
                _countDownTimer = value;
                this.OnPropertyChanged("CountDownTimer");
            }
        }

        public string ScreenOrderVisible
        {
            get
            {
                return _screenOrderVisible;
            }

            set
            {
                _screenOrderVisible = value;
                OnPropertyChanged("ScreenOrderVisible");
            }
        }

        public string ScreenNotificationVisible
        {
            get
            {
                return _screenNotificationVisible;
            }

            set
            {
                _screenNotificationVisible = value;
                OnPropertyChanged("ScreenNotificationVisible");
            }
        }

        public Employee OrderOwner
        {
            get
            {
                return _orderOwner;
            }

            set
            {
                _orderOwner = value;
                OnPropertyChanged("OrderOwner");
            }
        }

        public ObservableCollection<Employee> ObservableListEmployee
        {
            get
            {
                return _observableListEmployee;
            }

            set
            {
                _observableListEmployee = value;
                this.OnPropertyChanged("ObservableListEmployee");
            }
        }

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
        public void DisplayEmployeeHasAlreadyOrdered(Employee employeeInfo, MainViewModel mainViewModel)
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
            }));
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

        private bool CanExecuteRemoveEmployeeOrder()
        {
            return true;
        }

        private bool CanExecuteEditMenu()
        {
            return true;
        }

        private bool CanExecuteConfirmCommand()
        {
            return true;
        }

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
            }
        }

        private void UpdateOrderNo()
        {
            for (int i = 0; i < ObservableListEmployee.Count; i++)
            {
                ObservableListEmployee[i].OrderNo = i + 1;
            }

            ObservableListEmployee = new ObservableCollection<Employee>(this.ObservableListEmployee);
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
            var abc = TakeAwayChecked;
            if (ObservableListEmployee.Count == 0)
            {
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ContainEmployee);
            }
            else
            {
                var orderDetails = new StringBuilder(_BeginRootTag);

                if (ObservableListEmployee.Any() && ObservableListEmployee.Count > 0)
                {
                    foreach (var employee in ObservableListEmployee)
                    {
                        DisplayOrderedMeal(employee);
                        if (employee.SelectedValueDropDown == 0 || employee.SelectedValueDropDown == int.MaxValue)
                        {
                            orderDetails.AppendFormat(_OrderDetails, employee.SysId, 1, 1);
                        }
                        else
                        {
                            orderDetails.AppendFormat(_OrderDetails, employee.SysId, employee.SelectedValueDropDown, 0);
                        }
                    }
                }

                var insertOrderCriteria = new InsertOrderMealByUserCriteria()
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy = OrderOwner.SysId,
                    TakeAway = TakeAwayChecked,
                    ListDetail = orderDetails.Append(_EndRootTag).ToString()
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
