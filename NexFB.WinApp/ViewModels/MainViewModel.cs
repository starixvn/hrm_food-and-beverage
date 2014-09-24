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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Core.Models;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Models;
    using NexFB.WinApp.DependencyProperties;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private static readonly OrderFoodViewModel OrderFoodViewModel = new OrderFoodViewModel();
        private static string _host = ConfigurationManager.AppSettings[Constant.HubHost];

        private static DispatcherTimer digitalClock = new DispatcherTimer();
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private static double _screeHeight = SystemParameters.PrimaryScreenHeight;
        private static double _displayBanner = Constant.BannerResolution1280;

        private ViewModelBase _currentViewModel;
        private string _searchResult;
        private string _waitingSearchResult;
        private string _awaitingDeliverSearchResult;
        private string _deliveredSearchResult;
        private string _keypadVisibility;
        private double _displayWidth;

        private MenuColor _menu1 = new MenuColor();
        private MenuColor _menu2 = new MenuColor();
        private MenuColor _menu3 = new MenuColor();
        private MenuColor _menu4 = new MenuColor();
        private MenuColor _menu5 = new MenuColor();

        private string _todayDate;
        private string _todayTime;

        #endregion

        #region Contructors

        public MainViewModel()
        {
            ////var allScreens = System.Windows.Forms.Screen.AllScreens;
            CurrentViewModel = new WaitingMealViewModel();

            CheckCurrentViewModel();
            InitializeRelayCommand();
            InitializeHubConnection();

            digitalClock.Interval = new TimeSpan(0, 0, 1);
            digitalClock.Tick += new EventHandler(Timer_Click);
            digitalClock.Start();
        }

        #endregion

        #region Properties

        public Thread Thread { get; set; }

        public IHubProxy Proxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public ICommand DeliverOrderFoodCommand { get; set; }

        public ICommand CloseWindow { get; set; }

        public ICommand ViewAwaitingDeliverFoodCommand { get; set; }

        public ICommand ViewPedingFoodCommand { get; set; }

        public ICommand ViewDeliveredFoodCommand { get; set; }

        public ICommand ViewOrderFoodCommand { get; set; }

        public ICommand DisplayKeyPad { get; set; }

        public ICommand NumPadPressed { get; set; }

        public ICommand NumPadReleased { get; set; }

        public ICommand UndoOrderCommand { get; set; }

        public ICommand EditOrderCommand { get; set; }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        public string SearchResult
        {
            get
            {
                return _searchResult;
            }

            private set
            {
                _searchResult = value;
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
                if (currentViewModel == Constant.WaitingMealViewModel)
                {
                    WaitingSearchResult = _searchResult;
                }

                if (currentViewModel == Constant.AwaitingDeliverViewModel)
                {
                    AwaitingDeliverSearchResult = _searchResult;
                }

                if (currentViewModel == Constant.DeliveredFoodViewModel)
                {
                    DeliveredSearchResult = _searchResult;
                }

                this.OnPropertyChanged("SearchResult");
            }
        }

        public string WaitingSearchResult
        {
            get
            {
                return _waitingSearchResult;
            }

            private set
            {
                _waitingSearchResult = value;
                this.OnPropertyChanged("WaitingSearchResult");
            }
        }

        public string AwaitingDeliverSearchResult
        {
            get
            {
                return _awaitingDeliverSearchResult;
            }

            private set
            {
                _awaitingDeliverSearchResult = value;
                this.OnPropertyChanged("AwaitingDeliverSearchResult");
            }
        }

        public string DeliveredSearchResult
        {
            get
            {
                return _deliveredSearchResult;
            }

            private set
            {
                _deliveredSearchResult = value;
                this.OnPropertyChanged("DeliveredSearchResult");
            }
        }

        public MenuColor Menu1
        {
            get
            {
                return _menu1;
            }

            set
            {
                _menu1 = value;
                this.OnPropertyChanged("Menu1");
            }
        }

        public MenuColor Menu2
        {
            get
            {
                return _menu2;
            }

            set
            {
                _menu2 = value;
                this.OnPropertyChanged("Menu2");
            }
        }

        public MenuColor Menu3
        {
            get
            {
                return _menu3;
            }

            set
            {
                _menu3 = value;
                this.OnPropertyChanged("Menu3");
            }
        }

        public MenuColor Menu4
        {
            get
            {
                return _menu4;
            }

            set
            {
                _menu4 = value;
                this.OnPropertyChanged("Menu4");
            }
        }

        public MenuColor Menu5
        {
            get
            {
                return _menu5;
            }

            set
            {
                _menu5 = value;
                this.OnPropertyChanged("Menu5");
            }
        }

        public string TodayDate
        {
            get
            {
                return _todayDate;
            }

            set
            {
                _todayDate = value;
                _todayDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                this.OnPropertyChanged("TodayDate");
            }
        }

        public string TodayTime
        {
            get
            {
                return _todayTime;
            }

            set
            {
                _todayTime = value;
                this.OnPropertyChanged("TodayTime");
            }
        }

        public string KeyPadVisibility
        {
            get
            {
                return _keypadVisibility;
            }

            set
            {
                _keypadVisibility = value;
                this.OnPropertyChanged("KeyPadVisibility");
            }
        }

        public double ScreenWidth
        {
            get
            {
                return _screenWidth;
            }

            set
            {
                _screenWidth = value;
                this.OnPropertyChanged("ScreenWidth");
            }
        }

        public double ScreenHeight
        {
            get
            {
                return _screeHeight;
            }

            set
            {
                _screeHeight = value;
                this.OnPropertyChanged("ScreenHeight");
            }
        }

        public double DisplayWidth
        {
            get
            {
                _displayWidth = _screenWidth - Constant.RemainingWidth;
                return _displayWidth;
            }

            set
            {
                _displayWidth = value;
                this.OnPropertyChanged("DisplayWidth");
            }
        }

        public double DisplayBanner
        {
            get
            {
                if (_screenWidth <= 1024)
                {
                    _displayBanner = Constant.BannerResolution1024;
                }

                return _displayBanner;
            }

            set
            {
                _displayBanner = value;
                this.OnPropertyChanged("DisplayBanner");
            }
        }

        #endregion

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
            if (SearchResult != null && SearchResult.Length == 2)
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

        // Execute Command
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
            CurrentViewModel = new DeliveredFoodViewModel(DeliveredSearchResult);
            CheckCurrentViewModel();
        }

        private void ExecuteViewOrderFood()
        {
            CurrentViewModel = MainViewModel.OrderFoodViewModel;
            CheckCurrentViewModel();
        }

        private void ExecuteDisplayKeyPad()
        {
            KeyPadVisibility = Constant.Visible;
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
                        if (InternetAvailability.IsInternetAvailable())
                        {
                            DeleteOrderFromCurrentList(orderId);
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
                        if (InternetAvailability.IsInternetAvailable())
                        {
                            DeleteOrderFromCurrentList(orderId);
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
                        DeleteOrderFromCurrentList(orderId);
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

        private void ExecuteEditOrderCommand(object value)
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

        #region Methods

        /// <summary>
        /// Remove order from list.
        /// </summary>
        /// <param name="orderId">Current order id.</param>
        private void DeleteOrderFromCurrentList(int orderId)
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

        private void OnNewOrder(Order order)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => NewOrder(order)));
        }

        /// <summary>
        /// Receive new order for app.
        /// </summary>
        /// <param name="order">Order entity.</param>
        private void NewOrder(Order order)
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToUpper().ToString();
            if (currentViewModel == Constant.WaitingMealViewModel)
            {
                ((WaitingMealViewModel)CurrentViewModel).NewOrderDataInWaitingMeals(order);
            }
        }

        private void OnEditOrder(Order order)
        {
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
            switch (currentViewModel)
            {
                case Constant.WaitingMealViewModel:
                    ((WaitingMealViewModel)CurrentViewModel).DeleteDataInWaitingMeals(orderId);
                    break;
                case Constant.AwaitingDeliverViewModel:
                    Order order = GetOrderById(orderId);
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
                case Constant.AwaitingDeliverViewModel:
                    Order order = GetOrderById(orderId);
                    ((AwaitingDeliverViewModel)CurrentViewModel).NewOrderDataInAwaitingDeliverMeals(order);
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
            return Proxy.Invoke("KitchenNotify", orderId, screenControl);
        }

        /// <summary>
        /// Connect to server hub.
        /// </summary>
        private void InitializeHubConnection()
        {
            Active = true;
            Thread = new System.Threading.Thread(() =>
            {
                Connection = new HubConnection(_host);
                Proxy = Connection.CreateHubProxy("order");
                Proxy.On<Order>("newOrderForApp", (order) => OnNewOrder(order));
                Proxy.On<int>("appNotifyReceiveNewOrder", (orderId) => OnKitchenStaffReceiveOrder(orderId));
                Proxy.On<int>("appNotifyAwaitingDeliverOrder", (orderId) => OnKitchenStaffAwaitingDeliverOrder(orderId));
                Proxy.On<int>("appNotifyUndoDeliveredOrder", (orderId) => OnKitchenStaffUndoDeliveredOrder(orderId));
                Proxy.On<int>("deleteOrderForApp", (orderId) => OnDeleteOrder(orderId));
                Proxy.On<Order>("editOrderForApp", (order) => OnEditOrder(order));
                Connection.Start().Wait();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        /// <summary>
        /// Method Initialize RelayCommand.
        /// </summary>
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
        }

        /// <summary>
        /// Check Current View model.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            KeyPadVisibility = Constant.Hidden;
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
                    break;
                default:
                    break;
            }
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            TodayDate = dateTimeNow.ToString("dd/MM/yyyy");
            TodayTime = dateTimeNow.ToString("hh:mm");
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
