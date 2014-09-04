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
    using NexFB.WinApp.Core.Commands;

    /// <summary>
    /// Delivered Food ViewModel.
    /// </summary>
    public class DeliveredFoodViewModel : ViewModelBase
    {
        #region Fields

        private List<Order> _deliveredMeals;

        #endregion

        #region Constructors

        public DeliveredFoodViewModel()
        {
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ShowMessage(x));
            ExportOrderFoodCommand = new RelayCommand((x) => CanExecuteExportOrderFoodCommand(), (x) => ShowMessage(x));
            DeliveredMeals = this.GetDeliveredMealOrder;
        }

        #endregion

        #region Properties

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

        public List<Order> GetDeliveredMealOrder
        {
            get
            {
                var criteria = new GetDeliveredMealOrderCriteria();
                List<Order> deliveredMeals = KitchenService.GetDeliveredMealOrder(criteria);
                return deliveredMeals;
            }
        }

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        public bool CanExecuteExportOrderFoodCommand()
        {
            return true;
        }

        // Execute Command.
        public void ShowMessage(object value)
        {
            if (value == null)
            {
                MessageBox.Show("null");
            }
            else
            {
                MessageBox.Show(value.ToString());
            }
        }

        #endregion
    }
}
