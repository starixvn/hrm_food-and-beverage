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
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.WinAppSmartCard.Core.Commands;
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
        private static ObservableCollection<Employee> _observableListEmployee = new ObservableCollection<Employee>();
        private List<Employee> _listEmployeeOrder;
        private Employee _orderOwner;
        private int _currentCulture;
        private Food _selectedFood;
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private string _orderNo;

        public MainDishesViewModel()
        {
            ListEmployeeOrder = new List<Employee>();
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

        public string OrderNo
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

        public List<Employee> ListEmployeeOrder
        {
            get
            {
                return _listEmployeeOrder;
            }

            set
            {
                _listEmployeeOrder = value;
                this.OnPropertyChanged("ListEmployeeOrder");
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Employee To Order List.
        /// </summary>
        /// <param name="employeeInfo">Employee Info.</param>
        /// <param name="currentCulture">Current culture.</param>
        public void AddEmployeeOrderToList(Employee employeeInfo, int currentCulture)
        {
            if (ListEmployeeOrder != null)
            {
                if (!ListEmployeeOrder.Any(x => x.SysId == employeeInfo.SysId))
                {
                    employeeInfo.OrderNo = ListEmployeeOrder.Count + 1;
                    employeeInfo.CurrentCulture = currentCulture;
                    HideMenuList(employeeInfo);
                    if (OrderOwner == null)
                    {
                        OrderOwner = employeeInfo;
                    }

                    ListEmployeeOrder.Add(employeeInfo);
                    ObservableListEmployee = new ObservableCollection<Employee>(this.ListEmployeeOrder);
                    CurrentCulture = currentCulture;
                }
                else
                {
                    MessageBox.Show("You are already included in this order");
                }
            }
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

        ////private bool CanExecuteCancelCommand()
        ////{
        ////    return true;
        ////}

        /// <summary>
        /// Remove employee from list.
        /// </summary>
        /// <param name="value">Employee sysId.</param>
        private void ExecuteRemoveEmployeeOrder(object value)
        {
            if (value != null)
            {
                int employeeSysId = (int)value;
                Employee employeeRemoved = ListEmployeeOrder.Where(x => x.SysId == employeeSysId).SingleOrDefault();
                if (employeeRemoved != null)
                {
                    ListEmployeeOrder.Remove(employeeRemoved);
                    ObservableListEmployee = new ObservableCollection<Employee>(ListEmployeeOrder);
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
                    Employee selectedEmployee = ListEmployeeOrder.Where(x => x.SysId == employeeSysId).SingleOrDefault();
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
                    ObservableListEmployee = new ObservableCollection<Employee>(this.ListEmployeeOrder);
                }
            }
        }

        /// <summary>
        /// Confirm Command.
        /// </summary>
        private void ExecuteConfirmCommand()
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
                OrderNo = result.OrderNo;
                ShowNotificationScreen();
            }
            else
            {
                MessageBox.Show(result.CommandResult.Message);
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

        #endregion
    }
}
