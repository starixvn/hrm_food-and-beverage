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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Core.Enums;
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

        private static ObservableCollection<Order> _awaitingDeliverMeals = new ObservableCollection<Order>();
        private string _host = ConfigurationManager.AppSettings[Constant.HubHost];
        private string _searchValue;

        #endregion

        #region Constructors
        public AwaitingDeliverViewModel()
            : this(null)
        {
        }

        public AwaitingDeliverViewModel(string searchValue)
        {
            ListenToHub();
            SearchValue = searchValue;
            DeliverOrderFoodCommand = new RelayCommand((x) => CanExecuteDeliverOrderFoodCommand(), (x) => ExecuteDeliverOrderFoodCommand(x));
            AwaitingDeliverMeals = GetAwaitingDeliverMealOrder;
        }

        #endregion

        #region Properties

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public IHubProxy Proxy { get; set; }

        public Thread Thread { get; set; }

        public ICommand DeliverOrderFoodCommand { get; set; }

        public ObservableCollection<Order> AwaitingDeliverMeals
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

        public ObservableCollection<Order> GetAwaitingDeliverMealOrder
        {
            get
            {
                var criteria = new GetAwaitingDeliverMealOrderCriteria();
                List<Order> awaitingDeliverMeals = KitchenService.GetAwaitingDeliverMealOrder(criteria);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    awaitingDeliverMeals = awaitingDeliverMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
                }

                ObservableCollection<Order> observableWaitingDeliverMeals = new ObservableCollection<Order>(awaitingDeliverMeals);

                return observableWaitingDeliverMeals;
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
                        Order item = AwaitingDeliverMeals.Where(x => x.OrderId == orderId).SingleOrDefault();
                        AwaitingDeliverMeals.Remove(item);

                        var taskSendNotify = new Task(() => KitchenNotify(item.OrderId));
                        var taskAppNotifyReceived = new Task(() => RefeshAwaitingDeliverOrderList(AwaitingDeliverMeals, item.OrderId));
                        taskSendNotify.Start();
                        taskAppNotifyReceived.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Connect to Hub Server.
        /// </summary>
        private void ListenToHub()
        {
            Active = true;
            Thread = new System.Threading.Thread(() =>
            {
                Connection = new HubConnection(_host);
                Proxy = Connection.CreateHubProxy("order");
                Proxy.On<Order>("editReceivedOrder", (order) => OnEditOrder(order));
                Proxy.On<Order>("addOrderToAwaitingDeliverMeals", (order) => AddOrderToAwaitingDeliverMeals(order));
                Proxy.On<ObservableCollection<Order>>("refeshAwaitingDeliverOrderList", (orders) => SyncDataForApp(orders));
                Connection.Start().Wait();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        private void OnEditOrder(Order order)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => EditItemInList(order)));
        }

        private void SyncDataForApp(ObservableCollection<Order> awaitingDeliveredOrders)
        {
            if (awaitingDeliveredOrders != null)
            {
                this.AwaitingDeliverMeals = awaitingDeliveredOrders;
            }
        }

        /// <summary>
        /// Method Edit Item In Current AwaitingDeliverMeals List.
        /// </summary>
        /// <param name="order">Edited item.</param>
        private void EditItemInList(Order order)
        {
            if (order != null)
            {
                int currentStatusId = order.CurrentStatusId;
                if (currentStatusId == (int)OrderStatus.ReceivedOrder || currentStatusId == (int)OrderStatus.ReducedOrder)
                {
                    int index = AwaitingDeliverMeals.IndexOf(AwaitingDeliverMeals.Where(x => x.OrderId == order.OrderId).SingleOrDefault());
                    AwaitingDeliverMeals[index] = order;
                }
            }
        }

        private void AddOrderToAwaitingDeliverMeals(Order order)
        {
            if (order != null)
            {
                int currentStatusId = order.CurrentStatusId;
                if (currentStatusId == (int)OrderStatus.ReceivedOrder || currentStatusId == (int)OrderStatus.ReducedOrder)
                {
                    AwaitingDeliverMeals = GetAwaitingDeliverMealOrder;
                }
            }
        }

        private void KitchenNotify(int orderId)
        {
            Proxy.Invoke("KitchenNotify", orderId);
        }

        private void RefeshAwaitingDeliverOrderList(ObservableCollection<Order> awaitingDeliveredOrders, int currentOrderDelivered)
        {
            Proxy.Invoke("RefeshAwaitingDeliverOrderList", awaitingDeliveredOrders, currentOrderDelivered);
        }

        #endregion
    }
}
