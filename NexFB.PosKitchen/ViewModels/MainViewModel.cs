// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Enums;
    using NexFB.Pos.Core.Helpers;
    using NexFB.PosKitchen.ViewModels.Properties.Main;
    using NexFB.Resources;
    using WPFMessageBoxCustomControl;
    using WPFMessageBoxCustomControl.Enums;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : MainModelProperty
    {
        private static string _host = ConfigurationManager.AppSettings[Constant.HubHost];
        private static string _hostLocal = ConfigurationManager.AppSettings[Constant.HubHostLocal];
        private static DispatcherTimer _digitalClockDispatcher = new DispatcherTimer();
        private static DispatcherTimer _refeshAppDataDispatcher = new DispatcherTimer();
        private static DispatcherTimer _newOrderNotify = new DispatcherTimer();

        public MainViewModel()
        {
            CurrentViewModel = new WaitingMealViewModel();
            this.CheckCurrentCulture();
            CheckCurrentViewModel();
            InitializeRelayCommand();
            InitializeHubConnection();
            InitializeHubConnectionLocal();
            TimerDigitalClock();
            StartTimerGetData((int)QueryData.GetAllData, null);
        }

        #region Tasks Send Data To Hub

        /// <summary>
        /// Send new order to hub.
        /// </summary>
        /// <param name="orderId">New order id.</param>
        /// <returns>Task executed.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Fix later.")]
        public Task SendNewOrder(int orderId)
        {
            if (Connection.State == ConnectionState.Connected)
            {
                try
                {
                    return Proxy.Invoke("SendNewOrder", orderId);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
            else
            {
                try
                {
                    return ProxyLocal.Invoke("SendNewOrder", orderId);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
        }

        /// <summary>
        /// Send Edit order to hub.
        /// </summary>
        /// <param name="orderId">Edited order id.</param>
        /// <returns>Task executed.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Fix later.")]
        public Task SendEditOrder(int orderId)
        {
            if (Connection.State == ConnectionState.Connected)
            {
                try
                {
                    return Proxy.Invoke("SendEditOrder", orderId);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
            else
            {
                try
                {
                    return ProxyLocal.Invoke("SendEditOrder", orderId);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
        }

        #endregion

        #region RelayCommands

        // Can Execute Command
        private bool CanExecuteNumPadPressed()
        {
            return string.IsNullOrEmpty(SearchResult) || SearchResult.Length < 7;
        }

        private void ExecuteViewPendingFood()
        {
            _newOrderNotify.Stop();
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

        private void ExecuteViewCanceledFood()
        {
            CurrentViewModel = new CanceledFoodViewModel(CanceledSearchResult);
            CheckCurrentViewModel();
        }

        private void ExecuteViewOrderFood()
        {
            CurrentViewModel = new OrderFoodViewModel(this);
            CheckCurrentViewModel();
        }

        private void ExecuteDisplayKeyPad()
        {
            KeyPadVisibility = KeyPadVisibility == Constant.Visible ? Constant.Hidden : Constant.Visible;
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
                    switch (currentViewModel)
                    {
                        case Constant.WaitingMealViewModel:
                            ExecuteViewPendingFood();
                            break;
                        case Constant.AwaitingDeliverViewModel:
                            ExecuteViewAwaitingDeliverFood();
                            break;
                        case Constant.DeliveredFoodViewModel:
                            ExecuteViewDeliveredFood();
                            break;
                        case Constant.CanceledFoodViewModel:
                            ExecuteViewCanceledFood();
                            break;
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
                        WPFMessageBox.Show(CurrentCulture, received.Message.ToString());
                    }
                    else
                    {
                        if (Connection.State == ConnectionState.Connected || ConnectionLocal.State == ConnectionState.Connected)
                        {
                            ProcessDataFromCurrentList(orderId, false);
                            SendMessage(orderId, (int)KitchenScreen.AwaitingDeliverFoodScreen);
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
                WPFMessageBox.Show(CurrentCulture, Lang.ErrorMessage.ToString());
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
                        WPFMessageBox.Show(CurrentCulture, received.Message.ToString());
                    }
                    else
                    {
                        if (Connection.State == ConnectionState.Connected || ConnectionLocal.State == ConnectionState.Connected)
                        {
                            ProcessDataFromCurrentList(orderId, false);
                            SendMessage(orderId, (int)KitchenScreen.WaitingFoodScreen);
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
                WPFMessageBox.Show(CurrentCulture, Lang.ErrorMessage.ToString());
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
                    WPFMessageBox.Show(CurrentCulture, result.Message);
                }
                else
                {
                    if (Connection.State == ConnectionState.Connected || ConnectionLocal.State == ConnectionState.Connected)
                    {
                        ProcessDataFromCurrentList(orderId, false);
                        SendMessage(orderId, (int)KitchenScreen.DeliveredFoodScreen);
                    }
                    else
                    {
                        ExecuteViewDeliveredFood();
                    }
                }
            }
            else
            {
                WPFMessageBox.Show(CurrentCulture, Lang.ErrorMessage);
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
                WPFMessageBox.Show(CurrentCulture, Lang.ErrorMessage);
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

        /// <summary>
        /// Execute Cancel Command.
        /// </summary>
        /// <param name="value">Current Order Id.</param>
        private void ExecuteCancelCommand(object value)
        {
            if (value != null)
            {
                WPFMessageBoxResult confirmCancel = WPFMessageBox.Show(
                    (int)LanguageLocalization.VietNamCulture,
                    WinFormMainResource.Confirmation,
                    WinFormMainResource.Message_PermanentCancelOrder,
                    WPFMessageBoxButtons.YesNo,
                    WPFMessageBoxImage.Alert);

                if (confirmCancel == WPFMessageBoxResult.Yes)
                {
                    int orderId = (int)value;
                    var cancelOrderCriteria = new CancelOrderCriteria()
                    {
                        OrderId = orderId,
                        ModifiedDate = DateTime.Now
                    };

                    CommandResult result = this.KitchenService.CancelOrder(cancelOrderCriteria);
                    if (result.Code != 0)
                    {
                        this.ShowErrorMessage((int)LanguageLocalization.VietNamCulture, result.Message.ToString());
                    }
                    else
                    {
                        ProcessDataFromCurrentList(orderId, false);
                        SendMessage(orderId, (int)KitchenScreen.CanceledFoodScreen);
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Remove order from list.
        /// </summary>
        /// <param name="orderId">Current order id.</param>
        /// <param name="isRejectedOrder">Check for rejected order case.</param>
        private void ProcessDataFromCurrentList(int orderId, bool isRejectedOrder)
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
                case Constant.CanceledFoodViewModel:
                    if (isRejectedOrder)
                    {
                        Order rejectedOrder = GetOrderById(orderId);
                        ((CanceledFoodViewModel)CurrentViewModel).AddRejectedOrderToList(rejectedOrder);
                    }

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
            StartTimerGetData((int)QueryData.GetNewOrder, order);
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

        private void OnKitchenRejectedOrder(int orderId)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => KitchenStaffRejectedOrder(orderId)));
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
                    ((WaitingMealViewModel)CurrentViewModel).DeleteDataInWaitingMeals(order.OrderId);
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

        private void KitchenStaffRejectedOrder(int orderId)
        {
            ProcessDataFromCurrentList(orderId, true);
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

        /// <summary>
        /// Send notify to hub server.
        /// </summary>
        /// <param name="orderId">Current order id.</param>
        /// <param name="screenControl">From screen control.</param>
        /// <returns>Executed task.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Fix later.")]
        private Task SendMessage(int orderId, int screenControl)
        {
            if (Connection.State == ConnectionState.Connected)
            {
                try
                {
                    return Proxy.Invoke("KitchenNotify", orderId, screenControl);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
            else
            {
                try
                {
                    return ProxyLocal.Invoke("KitchenNotify", orderId, screenControl);
                }
                catch
                {
                    return Task.Factory.StartNew(new Action(() => { }));
                }
            }
        }

        /// <summary>
        /// Connect to server hub.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Fix later.")]
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
                Proxy.On<int>("appNotifyRejectedOrder", (orderId) => OnKitchenRejectedOrder(orderId));
                Proxy.On<int>("deleteOrderForApp", (orderId) => OnDeleteOrder(orderId));
                Proxy.On<int>("editOrderForApp", (orderId) => OnEditOrder(orderId));
                try
                {
                    Connection.Start().Wait();
                }
                catch (Exception)
                {
                    return;
                }

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
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Fix later.")]
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
                ProxyLocal.On<int>("appNotifyRejectedOrder", (orderId) => OnKitchenRejectedOrder(orderId));
                ProxyLocal.On<int>("deleteOrderForApp", (orderId) => OnDeleteOrder(orderId));
                ProxyLocal.On<int>("editOrderForApp", (orderId) => OnEditOrder(orderId));
                try
                {
                    ConnectionLocal.Start().Wait();
                }
                catch (Exception)
                {
                    return;
                }

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
        private void InitializeRelayCommand()
        {
            DisplayKeyPad = new RelayCommand((x) => ExecuteDisplayKeyPad());
            NumPadReleased = new RelayCommand((x) => ExecuteNumPadPressed(x));
            ViewOrderFoodCommand = new RelayCommand((x) => ExecuteViewOrderFood());
            ViewPedingFoodCommand = new RelayCommand((x) => ExecuteViewPendingFood());
            ViewCanceledFoodCommand = new RelayCommand((x) => ExecuteViewCanceledFood());
            ViewDeliveredFoodCommand = new RelayCommand((x) => ExecuteViewDeliveredFood());
            ViewAwaitingDeliverFoodCommand = new RelayCommand((x) => ExecuteViewAwaitingDeliverFood());
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            InitializeCommondCommand();
        }

        private void InitializeCommondCommand()
        {
            CloseWindow = new RelayCommand((x) => ((Window)x).Close());
            CancelOrderCommand = new RelayCommand((x) => ExecuteCancelCommand(x));
            UndoOrderCommand = new RelayCommand((x) => ExecuteUndoOrderCommand(x));
            EditOrderCommand = new RelayCommand((x) => ExecuteEditOrderCommand(x));
            ReceiveOrderFoodCommand = new RelayCommand((x) => ExecuteReceiveOrderFoodCommand(x));
            DeliverOrderFoodCommand = new RelayCommand((x) => ExecuteDeliverOrderFoodCommand(x));
        }

        /// <summary>
        /// Check Current View model.
        /// </summary>
        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            KeyPadVisibility = Constant.Hidden;
            SearchTextBoxVisibility = Constant.Visible;
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    SearchResult = WaitingSearchResult ?? string.Empty;
                    Menu1.BackgroundColor = Constant.DefaultMenuColor;
                    Menu1.ForegroundColor = Constant.Menu1Color;
                    Menu1.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.AwaitingDeliverViewModel:
                    SearchResult = AwaitingDeliverSearchResult ?? string.Empty;
                    Menu2.BackgroundColor = Constant.DefaultMenuColor;
                    Menu2.ForegroundColor = Constant.Menu2Color;
                    Menu2.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.DeliveredFoodViewModel:
                    SearchResult = DeliveredSearchResult ?? string.Empty;
                    Menu3.BackgroundColor = Constant.DefaultMenuColor;
                    Menu3.ForegroundColor = Constant.Menu3Color;
                    Menu3.FontWeight = Constant.FontWeightBold;
                    break;
                case Constant.CanceledFoodViewModel:
                    SearchResult = CanceledSearchResult ?? string.Empty;
                    Menu5.BackgroundColor = Constant.DefaultMenuColor;
                    Menu5.ForegroundColor = Constant.Menu5Color;
                    Menu5.FontWeight = Constant.FontWeightBold;
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
            _digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            _digitalClockDispatcher.Tick += new EventHandler(Timer_Click);
            _digitalClockDispatcher.Start();
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

            if (getType == (int)QueryData.GetNewOrder)
            {
                timer.Interval = TimeSpan.FromSeconds(DelayTime);
            }
            else
            {
                timer = _refeshAppDataDispatcher;
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

            if (getType == (int)QueryData.GetNewOrder)
            {
                if (currentViewModel == Constant.WaitingMealViewModel)
                {
                    if (order != null)
                    {
                        ((WaitingMealViewModel)CurrentViewModel).NewOrderDataAndSort(order);
                    }
                }

                NotificationSound.PlayingNotificationSound();
                timer.Stop();
            }
            else
            {
                List<Order> waitingMeal = GetAllWaitingMeal();
                CheckConditionToStartNewOrderNotity(waitingMeal);
                switch (currentViewModel)
                {
                    case Constant.WaitingMealViewModel:
                        ((WaitingMealViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.AwaitingDeliverViewModel:
                        ((AwaitingDeliverViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.DeliveredFoodViewModel:
                        ////((DeliveredFoodViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.CanceledFoodViewModel:
                        ((CanceledFoodViewModel)CurrentViewModel).GetNewDataForApp();
                        break;
                    case Constant.OrderFoodViewModel:
                        break;
                }
            }
        }

        private void CheckConditionToStartNewOrderNotity(List<Order> orders)
        {
            if (orders != null && orders.Any(x => x.CurrentStatusId == (int)OrderStatus.NewOrder))
            {
                StartNewOrderSoundTimer();
            }
        }

        private void StartNewOrderSoundTimer()
        {
            if (!_newOrderNotify.IsEnabled)
            {
                _newOrderNotify.Interval = TimeSpan.FromSeconds(2);
                _newOrderNotify.Tick += (sender, e) => TimerTick(sender, e);
                _newOrderNotify.Start();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            NotificationSound.PlayingNotificationSound();
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
        }

        private List<Order> GetAllWaitingMeal()
        {
            var criteria = new GetWaitingMealOrderCriteria();
            List<Order> waitingMeal = KitchenService.GetWaitngMealOrder(criteria);
            return waitingMeal;
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
