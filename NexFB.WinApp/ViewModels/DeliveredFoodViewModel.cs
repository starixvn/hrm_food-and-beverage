// --------------------------------------------------------------------------
// <copyright file="DeliveredFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.WinApp.Core.Commands;

    /// <summary>
    /// Delivered Food ViewModel.
    /// </summary>
    public class DeliveredFoodViewModel : ViewModelBase
    {
        private List<Order> _deliveredMeals;

        private IKitchenService _kitchenService;

        public DeliveredFoodViewModel(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService ?? new KitchenService();
        }

        public DeliveredFoodViewModel()
            : this(null)
        {
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ShowMessage(x));
            ExportOrderFoodCommand = new RelayCommand((x) => CanExecuteExportOrderFoodCommand(), (x) => ShowMessage(x));
            DeliveredMeals = this.GetDeliveredMealOrder();
        }

        public ICommand ExportOrderFoodCommand { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public List<Order> DeliveredMeals
        {
            get
            {
                return _deliveredMeals;
            }

            set
            {
                if (_deliveredMeals == value)
                {
                    return;
                }

                _deliveredMeals = value;
                OnPropertyChanged("DeliveredMeals");
            }
        }

        public bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        public bool CanExecuteExportOrderFoodCommand()
        {
            return true;
        }

        public void ShowMessage(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("null");
            }
            else
            {
                MessageBox.Show(obj.ToString());
            }
        }

        public List<Order> GetDeliveredMealOrder()
        {
            var criteria = new GetDeliveredMealOrderCriteria();
            List<Order> deliveredMeals = _kitchenService.GetDeliveredMealOrder(criteria);
            return deliveredMeals;
        }
    }
}
