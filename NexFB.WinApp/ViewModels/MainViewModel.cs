// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Media;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.InternetBased;
    using NexFB.WinApp.ViewModels.Partials.Main;
    using NexFB.WinApp.Views;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : MainModelProperty
    {
        private static string _host = ConfigurationManager.AppSettings[Constant.HubHost];
        private static string _hostLocal = ConfigurationManager.AppSettings[Constant.HubHostLocal];
        private static DispatcherTimer digitalClockDispatcher = new DispatcherTimer();
        private static DispatcherTimer refeshAppDataDispatcher = new DispatcherTimer();

        public MainViewModel()
        {
            ////var allScreens = System.Windows.Forms.Screen.AllScreens;
            CurrentViewModel = new WaitingMealViewModel();
            CheckCurrentViewModel();
            InitializeRelayCommand();
            InitializeHubConnection();
            InitializeHubConnectionLocal();
            TimerDigitalClock();
            StartTimerGetData(Constant.GetAllData, null);
        }

        public Task SendNewOrder(int orderId)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                return Proxy.Invoke("SendNewOrder", orderId);
            }
            else
            {
                return ProxyLocal.Invoke("SendNewOrder", orderId);
            }
        }

        public Task SendEditOrder(int orderId)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                return Proxy.Invoke("SendEditOrder", orderId);
            }
            else
            {
                return ProxyLocal.Invoke("SendEditOrder", orderId);
            }
        }

        #region RelayCommands

        // Can Execute Command
        private bool CanExecuteViewPendingFoood()
        {
            return true;
        }

        private bool CanExecuteViewAwaitingDeliverFood()
        {
            return true;
        }

        private bool CanExecuteViewDeliveredFood()
        {
            return true;
        }

        private bool CanExecuteViewOrderFood()
        {
            return true;
        }

        private bool CanDisplayKeyPad()
        {
            return true;
        }

        private bool CanExecuteNumPadPressed()
        {
            if (SearchResult != null && SearchResult.Length == 3)
            {
                return false;
            }

            return true;
        }

        private bool CanExecuteNumPadReleased()
        {
            return true;
        }

        // Can Execute Command.
        private bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        private bool CanExecuteDeliverOrderFoodCommand()
        {
            return true;
        }

        private bool CanExecuteUndoOrderCommand()
        {
            return true;
        }

        private bool CanExecuteEditOrderCommand()
        {
            return true;
        }

        private bool CanExecuteSettingCommand()
        {
            return true;
        }

        // Execute Command
        private void ExecuteSettingCommand()
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Show();
        }

        private void ExecuteViewPendingFood()
        {
            CurrentViewModel = new WaitingMealViewModel(WaitingSearchResult);
            CheckCurrentViewModel();
        }

        private void ExecuteViewAwaitingDeliverFood()
        {
            CurrentViewModel = new AwaitingDeliverViewModel(AwaitingDeliverSearchResult);
            CheckCurrentViewModel();
        }

        private void ExecuteViewDeliveredFood()
        {
            CurrentViewModel = new DeliveredFoodViewModel(DeliveredSearchResult, this);
            CheckCurrentViewModel();
        }

        private void ExecuteViewOrderFood()
        {
            CurrentViewModel = new OrderFoodViewModel(this);
            CheckCurrentViewModel();
        }

        private void ExecuteDisplayKeyPad()
        {
            if (KeyPadVisibility == Constant.Visible)
            {
                KeyPadVisibility = Constant.Hidden;
            }
            else
            {
                KeyPadVisibility = Constant.Visible;
            }
        }

        /// <summary>
        /// Method Press virtual key pad.
        /// </summary>
        /// <param name="value">Object parameter.</param>
        private void ExecuteNumPadPressed(object value)
        {
            var keyPadParameter = value.ToString();
            switch (keyPadParameter.ToUpper())
            {
                case "BACK":
                    if (SearchResult.Length > 0)
                    {
                        SearchResult = SearchResult.Remove(SearchResult.Length - 1);
                    }

                    break;

                case "ENTER":
                    string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
                    if (currentViewModel == Constant.WaitingMealViewModel)
                    {
                        ExecuteViewPendingFood();
                    }

                    if (currentViewModel == Constant.AwaitingDeliverViewModel)
                    {
                        ExecuteViewAwaitingDeliverFood();
                    }

                    if (currentViewModel == Constant.DeliveredFoodViewModel)
                    {
                        ExecuteViewDeliveredFood();
                    }

                    break;
                default:
                    string keypadValue = keyPadParameter.Substring(keyPadParameter.Length - 1, 1);
                    SearchResult += keypadValue;
                    break;
            }
        }

        /// <summary>
        /// Execute Deliver Order Food Command.
        /// </summary>
        /// <param name="value">Parameter Value.</param>
        private void ExecuteDeliverOrderFoodCommand(object value)
        {
            if (value != null)
            {
                int orderId = (int)value;
                bool printed = ExecutePrintInvoice(orderId, (int)InvoiceNumberStatus.DeliverOrderFoodInvoice, false);
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
                        if (InternetAvailability.IsInternetAvailable())
                        {
                            ProcessDataFromCurrentList(orderId);
                            SendMessage(orderId, Constant.AwaitingDeliverFoodScreen);
                        }
                        else
                        {
                            ExecuteViewAwaitingDeliverFood();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Execute Receive Order Food Command.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        private void ExecuteReceiveOrderFoodCommand(object value)
        {
            if (value != null)
            {
                int orderId = (int)value;
                bool printed = ExecutePrintInvoice(orderId, (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice, false);

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
                        if (InternetAvailability.IsInternetAvailable())
                        {
                            ProcessDataFromCurrentList(orderId);
                            SendMessage(orderId, Constant.WaitingFoodScreen);
                        }
                        else
                        {
                            ExecuteViewPendingFood();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage.ToString());
            }
        }

        /// <summary>
        /// Execute Undo Order Command.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        private void ExecuteUndoOrderCommand(object value)
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
                    if (InternetAvailability.IsInternetAvailable())
                    {
                        ProcessDataFromCurrentList(orderId);
                        SendMessage(orderId, Constant.DeliveredFoodScreen);
                    }
                    else
                    {
                        ExecuteViewDeliveredFood();
                    }
                }
            }
            else
            {
                MessageBox.Show(Lang.ErrorMessage);
            }
        }

        /// <summary>
        /// Edit order command.
        /// </summary>
        /// <param name="value">Current order id.</param>
        private void ExecuteEditOrderCommand(object value)
        {
            int orderId = 0;
            if (value == null)
            {
                MessageBox.Show(Lang.ErrorMessage);
            }
            else
            {
                orderId = (int)value;
                var getOrderDetailsCriteria = new GetOrderDetailsCriteria() { OrderId = orderId };
                Order order = KitchenService.GetOrderDetails(getOrderDetailsCriteria);
                CurrentViewModel = new OrderFoodViewModel(this, order);
                CheckCurrentViewModel();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove order from list.
        /// </summary>
        /// <param name="orderId">Current order id.</param>
        private void ProcessDataFromCurrentList(int orderId)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    ((WaitingMealViewModel)CurrentViewModel).DeleteDataInWaitingMeals(orderId);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    ((AwaitingDeliverViewModel)CurrentViewModel).DeleteDataInAwaitingDeliverMeals(orderId);
                    break;
                case Constant.DeliveredFoodViewModel:
                    ((DeliveredFoodViewModel)CurrentViewModel).DeleteDataInDeliveredMeals(orderId);
                    break;
            }
        }

        private void OnNewOrder(int orderId)
        {
            Order order = this.GetOrderById(orderId);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => NewOrder(order)));
        }

        private void NewOrder(Order order)
        {
            StartTimerGetData(Constant.GetNewOrder, order);
        }

        private void OnEditOrder(int orderId)
        {
            Order order = this.GetOrderById(orderId);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => EditOrder(order)));
        }

        /// <summary>
        /// Edit selected order.
        /// </summary>
        /// <param name="order">Order entity.</param>
        private void EditOrder(Order order)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    ((WaitingMealViewModel)CurrentViewModel).EditDataInWaitingMeals(order);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    ((AwaitingDeliverViewModel)CurrentViewModel).EditDataInAwaitingDeliverMeals(order);
                    break;
            }
        }

        private void OnDeleteOrder(int orderId)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => DeleteOrder(orderId)));
        }

        /// <summary>
        /// Deleted selected order.
        /// </summary>
        /// <param name="orderId">Selected orderId.</param>
        private void DeleteOrder(int orderId)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    ((WaitingMealViewModel)CurrentViewModel).DeleteDataInWaitingMeals(orderId);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    ((AwaitingDeliverViewModel)CurrentViewModel).DeleteDataInAwaitingDeliverMeals(orderId);
                    break;
            }
        }

        private void OnKitchenStaffReceiveOrder(int orderId)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => KitchenStaffReceiveOrder(orderId)));
        }

        private void OnKitchenStaffAwaitingDeliverOrder(int orderId)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => KitchenStaffAwaitingDeliverOrder(orderId)));
        }

        private void OnKitchenStaffUndoDeliveredOrder(int orderId)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => KitchenStaffUndoDeliveredOrder(orderId)));
        }

        /// <summary>
        /// Kitchen Staff receive order.
        /// </summary>
        /// <param name="orderId">Received order id.</param>
        private void KitchenStaffReceiveOrder(int orderId)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            Order order = GetOrderById(orderId);
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    ((WaitingMealViewModel)CurrentViewModel).NewOrderDataAndSort(order);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    ((AwaitingDeliverViewModel)CurrentViewModel).NewOrderDataInAwaitingDeliverMeals(order);
                    break;
            }
        }

        /// <summary>
        /// Kitchen Staff awaiting deliver order.
        /// </summary>
        /// <param name="orderId">Received order id.</param>
        private void KitchenStaffAwaitingDeliverOrder(int orderId)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.AwaitingDeliverViewModel:
                    ((AwaitingDeliverViewModel)CurrentViewModel).DeleteDataInAwaitingDeliverMeals(orderId);
                    break;
                case Constant.DeliveredFoodViewModel:
                    Order order = GetOrderById(orderId);
                    ((DeliveredFoodViewModel)CurrentViewModel).NewOrderDataInDeliveredMeals(order);
                    break;
            }
        }

        /// <summary>
        /// Kitchen Staff awaiting deliver order.
        /// </summary>
        /// <param name="orderId">Received order id.</param>
        private void KitchenStaffUndoDeliveredOrder(int orderId)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    Order newWaitingOrder = GetOrderById(orderId);
                    ((WaitingMealViewModel)CurrentViewModel).NewOrderDataInWaitingMeals(newWaitingOrder);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    Order newAwaitingOrder = GetOrderById(orderId);
                    ((AwaitingDeliverViewModel)CurrentViewModel).NewOrderDataInAwaitingDeliverMeals(newAwaitingOrder);
                    break;
                case Constant.DeliveredFoodViewModel:
                    ((DeliveredFoodViewModel)CurrentViewModel).DeleteDataInDeliveredMeals(orderId);
                    break;
            }
        }

        private Order GetOrderById(int orderId)
        {
            var criteria = new GetMealOrderByIdCriteria()
            {
                OrderId = orderId
            };
            Order order = KitchenService.GetMealOrderById(criteria);

            return order;
        }

        private Task SendMessage(int orderId, int screenControl)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                return Proxy.Invoke("KitchenNotify", orderId, screenControl);
            }
            else
            {
                return ProxyLocal.Invoke("KitchenNotify", orderId, screenControl);
            }
        }

        /// <summary>
        /// Connect to server hub.
        /// </summary>
        private void InitializeHubConnection()
        {
            Active = true;
            Thread = new Thread(() =>
            {
                Connection = new HubConnection(_host);
                Proxy = Connection.CreateHubProxy("order");
                Proxy.On<int>("newOrderForApp", (orderId) => OnNewOrder(orderId));
                Proxy.On<int>("appNotifyReceiveNewOrder", (orderId) => OnKitchenStaffReceiveOrder(orderId));
                Proxy.On<int>("appNotifyAwaitingDeliverOrder", (orderId) => OnKitchenStaffAwaitingDeliverOrder(orderId));
                Proxy.On<int>("appNotifyUndoDeliveredOrder", (orderId) => OnKitchenStaffUndoDeliveredOrder(orderId));
                Proxy.On<int>("deleteOrderForApp", (orderId) => OnDeleteOrder(orderId));
                Proxy.On<int>("editOrderForApp", (orderId) => OnEditOrder(orderId));
                Connection.Start().Wait();

                while (Active)
                {
                    Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        /// <summary>
        /// Connect to local Hub.
        /// </summary>
        private void InitializeHubConnectionLocal()
        {
            ActiveLocal = true;
            ThreadLocal = new Thread(() =>
            {
                ConnectionLocal = new HubConnection(_hostLocal);
                ProxyLocal = ConnectionLocal.CreateHubProxy("order");
                ProxyLocal.On<int>("newOrderForApp", (orderId) => OnNewOrder(orderId));
                ProxyLocal.On<int>("appNotifyReceiveNewOrder", (orderId) => OnKitchenStaffReceiveOrder(orderId));
                ProxyLocal.On<int>("appNotifyAwaitingDeliverOrder", (orderId) => OnKitchenStaffAwaitingDeliverOrder(orderId));
                ProxyLocal.On<int>("appNotifyUndoDeliveredOrder", (orderId) => OnKitchenStaffUndoDeliveredOrder(orderId));
                ProxyLocal.On<int>("deleteOrderForApp", (orderId) => OnDeleteOrder(orderId));
                ProxyLocal.On<int>("editOrderForApp", (orderId) => OnEditOrder(orderId));
                ConnectionLocal.Start().Wait();

                while (ActiveLocal)
                {
                    Thread.Sleep(10);
                }
            }) { IsBackground = true };
            ThreadLocal.Start();
        }

        /// <summary>
        /// Method Initialize RelayCommand.
        /// </summary>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed. Suppression is OK here.")]
        private void InitializeRelayCommand()
        {
            ViewPedingFoodCommand = new RelayCommand((x) => CanExecuteViewPendingFoood(), (y) => ExecuteViewPendingFood());
            ViewDeliveredFoodCommand = new RelayCommand((x) => CanExecuteViewDeliveredFood(), (y) => ExecuteViewDeliveredFood());
            ViewOrderFoodCommand = new RelayCommand((x) => CanExecuteViewOrderFood(), (y) => ExecuteViewOrderFood());
            ViewAwaitingDeliverFoodCommand = new RelayCommand((x) => CanExecuteViewAwaitingDeliverFood(), (y) => ExecuteViewAwaitingDeliverFood());
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadPressed(y));
            DisplayKeyPad = new RelayCommand((x) => CanDisplayKeyPad(), (y) => ExecuteDisplayKeyPad());
            CloseWindow = new RelayCommand((x) => ((Window)x).Close());
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (y) => ExecuteReceiveOrderFoodCommand(y));
            DeliverOrderFoodCommand = new RelayCommand((x) => CanExecuteDeliverOrderFoodCommand(), (y) => ExecuteDeliverOrderFoodCommand(y));
            UndoOrderCommand = new RelayCommand((x) => CanExecuteUndoOrderCommand(), (x) => ExecuteUndoOrderCommand(x));
            EditOrderCommand = new RelayCommand((x) => CanExecuteEditOrderCommand(), (x) => ExecuteEditOrderCommand(x));
            SettingCommand = new RelayCommand((x) => CanExecuteSettingCommand(), (y) => ExecuteSettingCommand());
        }

        /// <summary>
        /// Check Current View model.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            KeyPadVisibility = Constant.Hidden;
            SearchTextBoxVisibility = Constant.Visible;
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    if (string.IsNullOrEmpty(WaitingSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = WaitingSearchResult;
                    }
                    
                    Menu1.BackgroundColor = Constant.DefaultMenuColor;
                    Menu1.ForegroundColor = Constant.Menu1Color;
                    Menu1.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.AwaitingDeliverViewModel:
                    if (string.IsNullOrEmpty(AwaitingDeliverSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = AwaitingDeliverSearchResult;
                    }
                    
                    Menu2.BackgroundColor = Constant.DefaultMenuColor;
                    Menu2.ForegroundColor = Constant.Menu2Color;
                    Menu2.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.DeliveredFoodViewModel:
                    if (string.IsNullOrEmpty(DeliveredSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = DeliveredSearchResult;
                    }
                    
                    Menu3.BackgroundColor = Constant.DefaultMenuColor;
                    Menu3.ForegroundColor = Constant.Menu3Color;
                    Menu3.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.OrderFoodViewModel:
                    Menu4.BackgroundColor = Constant.DefaultMenuColor;
                    Menu4.ForegroundColor = Constant.Menu4Color;
                    Menu4.FontWeight = Constant.FontWeightBold;
                    SearchTextBoxVisibility = Constant.Hidden;
                    break;
                default:
                    break;
            }
        }

        private void TimerDigitalClock()
        {
            digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            digitalClockDispatcher.Tick += new EventHandler(Timer_Click);
            digitalClockDispatcher.Start();
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            TodayDate = dateTimeNow.ToString("dd/MM/yyyy");
            TodayTime = dateTimeNow.ToString("HH:mm");
        }

        /// <summary>
        /// Timer get data.
        /// </summary>
        /// <param name="getType">Get for all app or part of app.</param>
        /// <param name="order">New order for app.</param>
        private void StartTimerGetData(int getType, Order order)
        {
            DispatcherTimer timer = new DispatcherTimer();

            if (getType == Constant.GetNewOrder)
            {
                timer.Interval = TimeSpan.FromSeconds(DelayTime);
            }
            else
            {
                timer = refeshAppDataDispatcher;
                timer.Interval = TimeSpan.FromSeconds(AutoRefeshTime);
            }

            timer.Tick += (sender, e) => TimerTickToGetData(sender, e, getType, order);
            timer.Start();
        }

        /// <summary>
        /// Timer tick.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        /// <param name="getType">Get for all app or part of app.</param>
        /// <param name="order">New order for app.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Reviewed. Suppression is OK here.")]
        private void TimerTickToGetData(object sender, EventArgs e, int getType, Order order)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();

            if (getType == Constant.GetNewOrder)
            {
                if (currentViewModel == Constant.WaitingMealViewModel)
                {
                    if (order != null)
                    {
                        ((WaitingMealViewModel)CurrentViewModel).NewOrderDataAndSort(order);
                        SystemSounds.Asterisk.Play();
                    }
                }

                timer.Stop();
            }
            else
            {
                switch (currentViewModel)
                {
                    case Constant.WaitingMealViewModel:
                        ((WaitingMealViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.AwaitingDeliverViewModel:
                        ((AwaitingDeliverViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.DeliveredFoodViewModel:
                        ((DeliveredFoodViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                }
            }
        }

        /// <summary>
        /// Set Default Menu Color.
        /// </summary>
        private void DefaulMenuColor()
        {
            Menu1.BackgroundColor = Constant.Menu1Color;
            Menu1.ForegroundColor = Constant.DefaultMenuColor;
            Menu1.FontWeight = Constant.FontWeightNormal;

            Menu2.BackgroundColor = Constant.Menu2Color;
            Menu2.ForegroundColor = Constant.DefaultMenuColor;
            Menu2.FontWeight = Constant.FontWeightNormal;

            Menu3.BackgroundColor = Constant.Menu3Color;
            Menu3.ForegroundColor = Constant.DefaultMenuColor;
            Menu3.FontWeight = Constant.FontWeightNormal;

            Menu4.BackgroundColor = Constant.Menu4Color;
            Menu4.ForegroundColor = Constant.DefaultMenuColor;
            Menu4.FontWeight = Constant.FontWeightNormal;

            Menu5.BackgroundColor = Constant.Menu5Color;
            Menu5.ForegroundColor = Constant.DefaultMenuColor;
            Menu5.FontWeight = Constant.FontWeightNormal;
        }

        #endregion
    }
}
