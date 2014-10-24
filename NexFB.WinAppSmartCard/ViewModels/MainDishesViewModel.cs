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
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;

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
        private int _currentCulture;
        private Food _selectedFood;
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private int _orderNo;
        private int _countDownTimer;
        private MainViewModel _mainViewModel;

        public MainDishesViewModel()
        {
            RemoveEmployeeOrder = new RelayCommand((x) => CanExecuteRemoveEmployeeOrder(), (y) => ExecuteRemoveEmployeeOrder(y));
            EditMenu = new RelayCommand((x) => CanExecuteEditMenu(), (y) => ExecuteEditMenu(y));
            ConfirmCommand = new RelayCommand((x) => CanExecuteConfirmCommand(), (y) => ExecuteConfirmCommand());
            ShowOrderScreen();
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

        public int CurrentCulture
        {
            get
            {
                return _currentCulture;
            }

            set
            {
                _currentCulture = value;
                OnPropertyChanged("CurrentCulture");
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
        /// <param name="currentCulture">Current culture.</param>
        /// <param name="mainViewModel">Main ViewModel.</param>
        public void AddEmployeeOrderToList(Employee employeeInfo, int currentCulture, MainViewModel mainViewModel)
        {
            if (ObservableListEmployee != null)
            {
                if (!ObservableListEmployee.Any(x => x.SysId == employeeInfo.SysId))
                {
                    employeeInfo.OrderNo = ObservableListEmployee.Count + 1;
                    employeeInfo.CurrentCulture = currentCulture;
                    HideMenuList(employeeInfo);
                    if (OrderOwner == null)
                    {
                        OrderOwner = employeeInfo;
                    }

                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                    {
                        ObservableListEmployee.Add(employeeInfo);
                        CurrentCulture = currentCulture;
                        MainViewModel = mainViewModel;
                        ToggleSideMenu();
                    }));
                }
                else
                {
                    this.ShowErrorMessage("You are already included in this order");
                }
            }
        }

        public void CancelTimerDispatcher()
        {
            digitalClockDispatcher.Stop();
            CountDownTimer = 0;
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
                if (employeeRemoved != null)
                {
                    ObservableListEmployee.Remove(employeeRemoved);
                    ToggleSideMenu();
                }
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
                int employeeSysId = (int)value;
                var getAlternativeFoodCriteria = new GetAlternativeFoodBySysIdCriteria() { SysId = employeeSysId };
                List<FoodOrder> alternativeFood = this.SmartCardOrderService.GetAlternativeFoodBySysId(getAlternativeFoodCriteria);
                if (alternativeFood != null)
                {
                    Employee selectedEmployee = ObservableListEmployee.Where(x => x.SysId == employeeSysId).SingleOrDefault();
                    List<Food> foodMapping = Mapper.Map<List<FoodOrder>, List<Food>>(alternativeFood);
                    Food currentMainFood = new Food()
                    {
                        FoodId = int.MaxValue,
                        FoodVnmName = selectedEmployee.MenuVnmName,
                        FoodEngName = selectedEmployee.MenuEngName,
                    };

                    SelectedFood = currentMainFood;
                    foodMapping.Insert(0, currentMainFood);
                    foodMapping.ForEach(x => x.CurrentCulture = CurrentCulture);
                    selectedEmployee.ListAlternativeFood = foodMapping;
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
                this.ShowErrorMessage("The order must contain at least 1 employee");
            }
            else
            {
                var orderDetails = new StringBuilder(_BeginRootTag);

                if (ObservableListEmployee.Any() && ObservableListEmployee.Count > 0)
                {
                    foreach (var employee in ObservableListEmployee)
                    {
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
                    ListDetail = orderDetails.Append(_EndRootTag).ToString()
                };

                OrderResult result = this.SmartCardOrderService.InsertOrderFood(insertOrderCriteria);
                if (result.CommandResult.Code == 0)
                {
                    MainViewModel.MainButtonEnable = false;
                    OrderNo = result.OrderNo;
                    CountDownTimer = 30;
                    ShowNotificationScreen();
                    TimerDigitalClock();
                }
                else
                {
                    this.ShowErrorMessage(result.CommandResult.Message);
                }
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

        private void ShowOrderScreen()
        {
            ScreenOrderVisible = ConstantValue.Visible;
            ScreenNotificationVisible = ConstantValue.Hidden;
        }

        private void ShowNotificationScreen()
        {
            ScreenOrderVisible = ConstantValue.Hidden;
            ScreenNotificationVisible = ConstantValue.Visible;
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
            if (CountDownTimer == 0)
            {
                if (MainViewModel != null)
                {
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
