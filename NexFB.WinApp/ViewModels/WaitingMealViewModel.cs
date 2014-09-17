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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using System.Media;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Documents;
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
    /// Pending Food ViewModel.
    /// </summary>
    public class WaitingMealViewModel : ViewModelBase
    {
        #region Fields
        private static ObservableCollection<Order> _waitingMeals = new ObservableCollection<Order>();
        private string _host = ConfigurationManager.AppSettings[Constant.HubHost];
        private string _searchValue;

        #endregion

        #region Constructors
        public WaitingMealViewModel()
            : this(null)
        {
        }

        public WaitingMealViewModel(string searchValue)
        {
            ListenToHub();
            SearchValue = searchValue;
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ExecuteReceiveOrderFoodCommand(x));

            WaitingMeals = this.GetWaitingMealOrder;
        }

        #endregion

        #region Properties

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public IHubProxy Proxy { get; set; }

        public Thread Thread { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public ObservableCollection<Order> WaitingMeals
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

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    waitingMeals = waitingMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
                }

                ObservableCollection<Order> observableWaitingMeals = new ObservableCollection<Order>(waitingMeals);

                return observableWaitingMeals;
            }
        }

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecuteReceiveOrderFoodCommand()
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
            if (value != null)
            {
                int orderId = (int)value;
                bool printed = ExecutePrintInvoice(orderId, Constant.ReceiveOrderFoodInvoice);

                if (printed)
                {
                    var criteria = new ProcessReceiveOrderCriteria() { OrderId = orderId };
                    CommandResult received = KitchenService.ReceiveOrder(criteria);
                    if (received.Code != 0)
                    {
                        MessageBox.Show(received.Message.ToString());
                    }
                    else
                    {
                        Order item = WaitingMeals.Where(x => x.OrderId == orderId).SingleOrDefault();
                        WaitingMeals.Remove(item);
                        var taskSendNotify = new Task(() => KitchenNotify(item.OrderId));
                        var taskAppNotifyReceived = new Task(() => RefeshWaitingOrderList(WaitingMeals, item.OrderId));
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
                Proxy.On<Order>("newOrderForApp", (order) => OnNewOrder(order));
                Proxy.On<Order>("editNewOrder", (order) => OnEditOrder(order));
                Proxy.On<ObservableCollection<Order>>("refeshWaitingOrderList", (orders) => SyncDataForApp(orders));
                Connection.Start().Wait();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        private void OnNewOrder(Order order)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => AddItemToList(order)));
        }

        private void OnEditOrder(Order order)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => EditItemInList(order)));
        }

        private void AddItemToList(Order order)
        {
            if (order != null)
            {
                if (order.CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    WaitingMeals.Add(order);
                    SystemSounds.Asterisk.Play();
                }
            }
        }

        private void EditItemInList(Order order)
        {
            if (order != null)
            {
                if (order.CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    int index = WaitingMeals.IndexOf(WaitingMeals.Where(x => x.OrderId == order.OrderId).SingleOrDefault());
                    WaitingMeals[index] = order;
                }
            }
        }

        private void SyncDataForApp(ObservableCollection<Order> waitingMeals)
        {
            if (waitingMeals != null)
            {
                this.WaitingMeals = waitingMeals;
            }
        }

        private void KitchenNotify(int orderId)
        {
            Proxy.Invoke("KitchenNotify", orderId);
        }

        private void RefeshWaitingOrderList(ObservableCollection<Order> waitingMeals, int currentOrderReceived)
        {
            Proxy.Invoke("RefeshWaitingOrderList", waitingMeals, currentOrderReceived);
        }

        #endregion
    }
}
