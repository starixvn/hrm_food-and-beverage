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
    using System.Windows.Documents;
    using System.Windows.Input;
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class WaitingMealViewModel : ViewModelBase
    {
        #region Fields

        private List<Order> _waitingMeals;

        #endregion

        #region Constructors

        public WaitingMealViewModel()
        {
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ExecuteReceiveOrderFoodCommand(x));
            DeliverOrderFoodCommand = new RelayCommand((x) => CanExecuteDeliverOrderFoodCommand(), (x) => ExecuteDeliverOrderFoodCommand(x));

            WaitingMeals = this.GetWaitingMealOrder;
        }

        #endregion

        #region Properties

        public ICommand DeliverOrderFoodCommand { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public ICommand PrintInvoiceCommand { get; set; }

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

        public List<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                return waitingMeals;
            }
        }

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        public bool CanExecuteDeliverOrderFoodCommand()
        {
            return true;
        }

        // Execute Command.

        /// <summary>
        /// Execute Receive Order Food Command.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public void ExecuteReceiveOrderFoodCommand(object value)
        {
            CommandResult received = null;
            bool printed = false;
            if (value != null)
            {
                int orderId = (int)value;
                printed = ExecutePrintInvoice(orderId, Constant.ReceiveOrderFoodInvoice);
                if (printed)
                {
                    var criteria = new ProcessReceiveOrderCriteria() { OrderId = orderId };
                    received = KitchenService.ReceiveOrder(criteria);
                    if (received.Code != 0)
                    {
                        MessageBox.Show(received.Message.ToString());
                    }
                    else
                    {
                        WaitingMeals = this.GetWaitingMealOrder;
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Execute Deliver Order Food Command.
        /// </summary>
        /// <param name="value">Parameter Value.</param>
        public void ExecuteDeliverOrderFoodCommand(object value)
        {
            CommandResult received = null;
            bool printed = false;
            if (value != null)
            {
                int orderId = (int)value;
                printed = ExecutePrintInvoice(orderId, Constant.DeliverOrderFoodInvoice);
                if (printed)
                {
                    var criteria = new ProcessDeliverOrderCriteria() { OrderId = orderId };
                    received = KitchenService.DeliverOrder(criteria);
                    if (received.Code != 0)
                    {
                        MessageBox.Show(received.Message.ToString());
                    }
                    else
                    {
                        WaitingMeals = this.GetWaitingMealOrder;
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage.ToString());
            }
        }

        #endregion
    }
}
