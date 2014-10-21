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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.SmartCardOrder;
    using NexFB.WinAppSmartCard.Core.Models;

    /// <summary>
    /// Main Dishes ViewModel.
    /// </summary>
    public class MainDishesViewModel : ViewModelBase
    {
        private static ObservableCollection<Employee> _observableListEmployee = new ObservableCollection<Employee>();
        private List<Employee> _listEmployeeOrder;

        public MainDishesViewModel()
        {
            ListEmployeeOrder = new List<Employee>();
        }

        #region Properties

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
        /// <param name="employeeCardNumber">Employee Card Number.</param>
        public void AddEmployeeOrderToList(string employeeCardNumber)
        {
            string cardValue = employeeCardNumber.Substring(employeeCardNumber.Length - 5, 5);
            var getEmployeeByCardNumber = new GetEmployeeByCardNumberCriteria() { CardNumber = cardValue };
            SCEmployeeOrder employeeData = this.SmartCardOrderService.GetEmployeeByCardId(getEmployeeByCardNumber);
            Employee employeeInfo = Mapper.Map<SCEmployeeOrder, Employee>(employeeData);

            if (ListEmployeeOrder != null)
            {
                if (!ListEmployeeOrder.Any(x => x.SysId == employeeInfo.SysId))
                {
                    employeeInfo.OrderNo = ListEmployeeOrder.Count + 1;
                    ListEmployeeOrder.Add(employeeInfo);
                    ObservableListEmployee = new ObservableCollection<Employee>(this.ListEmployeeOrder);
                }
            }
        }

        #endregion

        #region Private Methods
        
        #endregion
    }
}
