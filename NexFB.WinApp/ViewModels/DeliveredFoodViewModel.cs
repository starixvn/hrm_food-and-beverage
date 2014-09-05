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
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
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
            UndoOrderCommand = new RelayCommand((x) => CanExecuteUndoOrderCommand(), (x) => ExecuteUndoOrderCommand(x));
            EditOrderCommand = new RelayCommand((x) => CanExecuteEditOrderCommand(), (x) => ExecuteEditOrderCommand(x));
            DeliveredMeals = this.GetDeliveredMealOrder;
        }

        #endregion

        #region Properties

        public ICommand UndoOrderCommand { get; set; }

        public ICommand EditOrderCommand { get; set; }

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
        public bool CanExecuteUndoOrderCommand()
        {
            return true;
        }

        public bool CanExecuteEditOrderCommand()
        {
            return true;
        }

        // Execute Command.

        /// <summary>
        /// Execute Undo Order Command.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public void ExecuteUndoOrderCommand(object value)
        {
            if (value != null)
            {
                int orderId = (int)value;
                var criteria = new UndoOrderCriteria() { OrderId = orderId };
                CommandResult result = KitchenService.UndoOrder(criteria);
                if (result.Code != 0)
                {
                    MessageBox.Show(result.Message);
                }
                else
                {
                    DeliveredMeals = this.GetDeliveredMealOrder;
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage);
            }
        }

        public void ExecuteEditOrderCommand(object value)
        {
            if (value == null)
            {
                MessageBox.Show(Lang.ErrorMessage);
            }
            else
            {
                MessageBox.Show(value.ToString());
            }
        }

        #endregion
    }
}
