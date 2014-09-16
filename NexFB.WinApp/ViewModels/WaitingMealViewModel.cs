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
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client.Hubs;
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
                Proxy = Connection.CreateHubProxy("chat");
                Proxy.On<string>("newMessage", (name) => OnSendData(name));
                Connection.Start();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        private void OnSendData(string message)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => RemoveItemFromList(message)));
        }

        private void RemoveItemFromList(string message)
        {
            Order item = WaitingMeals.Where(x => x.OrderId == int.Parse(message)).SingleOrDefault();
            WaitingMeals.Remove(item);
            SystemSounds.Asterisk.Play();
            ////System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"c:\mywavfile.wav");
            ////player.Play();
            ////MessageBox.Show(message);
        }

        ////private void ActionSendButtonClick(object sender, RoutedEventArgs e)
        ////{
        ////    SendMessage();
        ////}

        ////private Task SendMessage()
        ////{
        ////    return Proxy.Invoke("SendMessage", ClientNameTextBox.Text);
        ////}

        #endregion
    }
}
