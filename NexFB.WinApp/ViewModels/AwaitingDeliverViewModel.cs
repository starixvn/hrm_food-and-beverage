// --------------------------------------------------------------------------
// <copyright file="AwaitingDeliverViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.8">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// Awaiting Deliver ViewModel.
    /// </summary>
    public class AwaitingDeliverViewModel : ViewModelBase
    {
        #region Fields

        private List<Order> _awaitingDeliverMeals;
        private string _searchValue;

        #endregion

        #region Constructors
        public AwaitingDeliverViewModel()
            : this(null)
        {
        }

        public AwaitingDeliverViewModel(string searchValue)
        {
            SearchValue = searchValue;
            DeliverOrderFoodCommand = new RelayCommand((x) => CanExecuteDeliverOrderFoodCommand(), (x) => ExecuteDeliverOrderFoodCommand(x));
            AwaitingDeliverMeals = GetAwaitingDeliverMealOrder;
        }

        #endregion

        #region Properties

        public ICommand DeliverOrderFoodCommand { get; set; }

        public List<Order> AwaitingDeliverMeals
        {
            get
            {
                return _awaitingDeliverMeals;
            }

            set
            {
                if (_awaitingDeliverMeals == value)
                {
                    return;
                }

                _awaitingDeliverMeals = value;
                OnPropertyChanged("AwaitingDeliverMeals");
            }
        }

        public string SearchValue
        {
            get
            {
                return _searchValue;
            }

            set
            {
                if (_searchValue == value)
                {
                    return;
                }

                _searchValue = value;
                OnPropertyChanged("SearchValue");
            }
        }

        public List<Order> GetAwaitingDeliverMealOrder
        {
            get
            {
                var criteria = new GetAwaitingDeliverMealOrderCriteria();
                List<Order> awaitingDeliverMeals = KitchenService.GetAwaitingDeliverMealOrder(criteria);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    awaitingDeliverMeals = awaitingDeliverMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue)).ToList();
                }

                return awaitingDeliverMeals;
            }
        }

        #endregion

        #region Relay Commands

        public bool CanExecuteDeliverOrderFoodCommand()
        {
            return true;
        }

        /// <summary>
        /// Execute Deliver Order Food Command.
        /// </summary>
        /// <param name="value">Parameter Value.</param>
        public void ExecuteDeliverOrderFoodCommand(object value)
        {
            if (value != null)
            {
                int orderId = (int)value;
                bool printed = ExecutePrintInvoice(orderId, Constant.DeliverOrderFoodInvoice);
                if (printed)
                {
                    var criteria = new ProcessDeliverOrderCriteria() { OrderId = orderId };
                    CommandResult received = KitchenService.DeliverOrder(criteria);
                    if (received.Code != 0)
                    {
                        MessageBox.Show(received.Message.ToString());
                    }
                    else
                    {
                        AwaitingDeliverMeals = this.GetAwaitingDeliverMealOrder;
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
