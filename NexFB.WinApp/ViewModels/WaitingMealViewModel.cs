// --------------------------------------------------------------------------
// <copyright file="WaitingMealViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Authority;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinApp.Core.Commands;

    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class WaitingMealViewModel : ViewModelBase
    {
        private List<Order> _waitingMeals;

        private IKitchenService _kitchenService;

        public WaitingMealViewModel(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService ?? new KitchenService();
        }

        public WaitingMealViewModel()
            : this(null)
        {
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ShowMessage(x));
            DeliverOrderFoodCommand = new RelayCommand((x) => CanExecuteDeliverOrderFoodCommand(), (x) => ShowMessage(x));

            WaitingMeals = this.GetWaitingMealOrder();
        }

        public ICommand DeliverOrderFoodCommand { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public List<Order> WaitingMeals
        {
            get
            {
                return _waitingMeals;
            }

            set
            {
                if (_waitingMeals == value)
                {
                    return;
                }

                _waitingMeals = value;
                OnPropertyChanged("WaitingMeals");
            }
        }

        public bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        public bool CanExecuteDeliverOrderFoodCommand()
        {
            return true;
        }

        /// <summary>
        /// Get Data from DB.
        /// </summary>
        /// <param name="obj">Parameter value.</param>
        public void ShowMessage(object obj)
        {
            var criteria = new GetEmployeeByCodeCriteria()
            {
                Code = "02355",
                DepartmentId = "1"
            };
            var _employeeService = new EmployeeService();

            UserSession user = _employeeService.GetUserByCode(criteria);
            MessageBox.Show(user.ToString());
        }

        public List<Order> GetWaitingMealOrder()
        {
            var criteria = new GetWaitingMealOrderCriteria();
            List<Order> waitingMeals = _kitchenService.GetWaitngMealOrder(criteria);
            return waitingMeals;
        }
    }
}
