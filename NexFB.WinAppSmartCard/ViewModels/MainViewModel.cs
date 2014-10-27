// --------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.17">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using AutoMapper;
    using BlogsPrajeesh.BlogSpot.WPFControls;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.InternetBased;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Services;

    /// <summary>
    /// Main View Model.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private static string _host = ConfigurationManager.AppSettings[ConstantValue.HubHost];
        private static string _hostLocal = ConfigurationManager.AppSettings[ConstantValue.HubHostLocal];
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private static double _screeHeight = SystemParameters.PrimaryScreenHeight;

        private ViewModelBase _currentViewModel;
        private string _currentTime;
        private int _currentCulture;
        private string _mainScreenVisible;
        private string _standbyScreenVisible;
        private string _currentCardNumber;
        private Employee _orderOwner;
        private MenuColor _mainDishesMenu = new MenuColor();
        private MenuColor _sideDishesMenu = new MenuColor();
        private bool _mainButtonEnable;
        private bool _sideButtonEnable;

        public MainViewModel()
        {
            InitializeHubConnection();
            InitializeHubConnectionLocal();
            ExecuteMainDishesCommand();
            MainDishesCommand = new RelayCommand((x) => CanExecuteMainDishesCommand(), (y) => ExecuteMainDishesCommand());
            SideDishesCommand = new RelayCommand((x) => CanExecuteSideDishesCommand(), (y) => ExecuteSideDishesCommand());
            ChangeLanguageCommand = new RelayCommand((x) => CanExecuteChangeLanguageCommand(), (y) => ExecuteChangeLanguageCommand(y));
            CancelCommand = new RelayCommand((x) => CanExecuteCancelCommand(), (y) => ExecuteCancelCommand());
            ReturnStandByScreenCommand = new RelayCommand((x) => CanExecuteReturnStandByScreen(), (y) => ExecuteReturnToStandByScreen());
            ChangeCulture();
            CheckCurrentScreen();
            this.MainButtonEnable = true;
            this.SideButtonEnable = true;
        }

        #region Properties

        public Thread Thread { get; set; }

        public IHubProxy Proxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public Thread ThreadLocal { get; set; }

        public IHubProxy ProxyLocal { get; set; }

        public HubConnection ConnectionLocal { get; set; }

        public bool ActiveLocal { get; set; }

        // Relay Command
        public ICommand MainDishesCommand { get; set; }

        public ICommand SideDishesCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ReturnStandByScreenCommand { get; set; }

        public string CurrentTime
        {
            get
            {
                return _currentTime;
            }

            set
            {
                _currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

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

        public int CurrentCulture
        {
            get
            {
                return _currentCulture;
            }

            set
            {
                _currentCulture = value;
                this.OnPropertyChanged("CurrentCulture");
            }
        }

        public string MainScreenVisible
        {
            get
            {
                return _mainScreenVisible;
            }

            set
            {
                _mainScreenVisible = value;
                this.OnPropertyChanged("MainScreenVisible");
            }
        }

        public string StandbyScreenVisible
        {
            get
            {
                return _standbyScreenVisible;
            }

            set
            {
                _standbyScreenVisible = value;
                this.OnPropertyChanged("StandbyScreenVisible");
            }
        }

        public string CurrentCardNumber
        {
            get
            {
                return _currentCardNumber;
            }

            set
            {
                _currentCardNumber = value;
                this.OnPropertyChanged("CurrentCardNumber");
            }
        }

        public MenuColor MainDishesMenu
        {
            get
            {
                return _mainDishesMenu;
            }

            set
            {
                _mainDishesMenu = value;
                this.OnPropertyChanged("MainDishesMenu");
            }
        }

        public MenuColor SideDishesMenu
        {
            get
            {
                return _sideDishesMenu;
            }

            set
            {
                _sideDishesMenu = value;
                this.OnPropertyChanged("SideDishesMenu");
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

        public Employee OrderOwner
        {
            get
            {
                return _orderOwner;
            }

            set
            {
                _orderOwner = value;
                OnPropertyChanged("OrderOwner");
            }
        }

        public bool MainButtonEnable
        {
            get
            {
                return _mainButtonEnable;
            }

            set
            {
                _mainButtonEnable = value;
                OnPropertyChanged("MainButtonEnable");
            }
        }

        public bool SideButtonEnable
        {
            get
            {
                return _sideButtonEnable;
            }

            set
            {
                _sideButtonEnable = value;
                OnPropertyChanged("SideButtonEnable");
            }
        }

        #endregion
        #region Public Methods

        /// <summary>
        /// On change Card Number.
        /// </summary>
        /// <param name="newCardNumber">New Card number.</param>
        public void OnSmartCardNumberchanged(string newCardNumber)
        {
            if (!string.IsNullOrEmpty(newCardNumber))
            {
                this.CurrentCardNumber = newCardNumber;
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        string cardValue = newCardNumber.Substring(newCardNumber.Length - 5, 5);
                        var getEmployeeByCardNumber = new GetEmployeeByCardNumberCriteria() { CardNumber = cardValue };
                        EmployeeInfo employeeData = this.SmartCardOrderService.GetEmployeeByCardId(getEmployeeByCardNumber);
                        if (employeeData != null)
                        {
                            var getEmployeeInGroupCriteria = new GetEmployeeInGroupBySysIdCriteria() { SysId = employeeData.SysId };
                            EmployeeInGroup employeeInGrop = this.KitchenService.GetEmployeeInGroup(getEmployeeInGroupCriteria);
                            CheckDataToAddToGrid(employeeInGrop, employeeData);
                        }
                        else
                        {
                            this.ShowErrorMessage("Can't indentify your card number");
                        }

                        break;
                    case ConstantValue.SideDishesViewModel:
                        break;
                    default:
                        break;
                }
            }
        }

        public void ExecuteCancelCommand()
        {
            WPFMessageBoxResult cancelOrder = this.ShowWarningCancelOrder();
            if (cancelOrder == WPFMessageBoxResult.Yes)
            {
                ExecuteReturnToStandByScreen();
            }
        }

        public void ExecuteReturnToStandByScreen()
        {
            CurrentCardNumber = string.Empty;
            OrderOwner = null;
            ExecuteMainDishesCommand();
            CheckCurrentScreen();
        }

        public Task SendMessage(int orderId)
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

        #endregion

        #region Commands

        private bool CanExecuteMainDishesCommand()
        {
            return true;
        }

        private bool CanExecuteSideDishesCommand()
        {
            return true;
        }

        private bool CanExecuteChangeLanguageCommand()
        {
            return true;
        }

        private bool CanExecuteCancelCommand()
        {
            return true;
        }

        private bool CanExecuteReturnStandByScreen()
        {
            return true;
        }

        /// <summary>
        /// Execute Command.
        /// </summary>
        private void ExecuteMainDishesCommand()
        {
            CancelDispatcherTimer();
            CurrentViewModel = new MainDishesViewModel();
            if (OrderOwner != null)
            {
                ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(OrderOwner, CurrentCulture, this);
            }

            CheckCurrentViewModel();
        }

        private void ExecuteSideDishesCommand()
        {
            CancelDispatcherTimer();
            CurrentViewModel = new SideDishesViewModel(OrderOwner, CurrentCulture, this);
            CheckCurrentViewModel();
        }

        /// <summary>
        /// Cancel dispatcher timer when user want to place new order.
        /// </summary>
        private void CancelDispatcherTimer()
        {
            if (CurrentViewModel != null)
            {
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        ((MainDishesViewModel)CurrentViewModel).CancelTimerDispatcher();
                        break;
                    case ConstantValue.SideDishesViewModel:
                        ((SideDishesViewModel)CurrentViewModel).CancelTimerDispatcher();
                        break;
                }
            }
        }

        private void ExecuteChangeLanguageCommand(object value)
        {
            if (value != null)
            {
                CurrentCulture = int.Parse((string)value);
                ChangeCulture();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set Default Menu Color.
        /// </summary>
        private void DefaulMenuColor()
        {
            MainDishesMenu.BackgroundColor = ConstantValue.MainDishesMenuColor;
            MainDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            MainDishesMenu.FontWeight = ConstantValue.FontWeightNormal;

            SideDishesMenu.BackgroundColor = ConstantValue.SideDishesMenuColor;
            SideDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            SideDishesMenu.FontWeight = ConstantValue.FontWeightNormal;
        }

        /// <summary>
        /// Check Current View Model.
        /// </summary>
        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
            switch (currentViewModel)
            {
                case ConstantValue.MainDishesViewModel:
                    MainDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                    MainDishesMenu.ForegroundColor = ConstantValue.MainDishesMenuColor;
                    MainDishesMenu.FontWeight = ConstantValue.FontWeightBold;
                    break;
                case ConstantValue.SideDishesViewModel:
                    SideDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                    SideDishesMenu.ForegroundColor = ConstantValue.SideDishesMenuColor;
                    SideDishesMenu.FontWeight = ConstantValue.FontWeightBold;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Change Culture.
        /// </summary>
        private void ChangeCulture()
        {
            switch (CurrentCulture)
            {
                case ConstantValue.EnglishCulture:
                    ResourceManagerService.ChangeLocale("en-GB");
                    CurrentCulture = ConstantValue.EnglishCulture;
                    break;
                case ConstantValue.ChinaCulture:
                    ResourceManagerService.ChangeLocale("zh-CN");
                    CurrentCulture = ConstantValue.ChinaCulture;
                    break;
                default:
                    ResourceManagerService.ChangeLocale("vi-VN");
                    CurrentCulture = ConstantValue.VietNamCulture;
                    break;
            }
        }

        /// <summary>
        /// Show main Screen or Standby screen.
        /// </summary>
        private void CheckCurrentScreen()
        {
            if (string.IsNullOrEmpty(CurrentCardNumber))
            {
                MainScreenVisible = ConstantValue.Hidden;
                StandbyScreenVisible = ConstantValue.Visible;
            }
            else
            {
                MainScreenVisible = ConstantValue.Visible;
                StandbyScreenVisible = ConstantValue.Hidden;
            }
        }

        /// <summary>
        /// Check Data before add to Main Screen.
        /// </summary>
        /// <param name="employeeInGrop">Employee place order data.</param>
        /// <param name="employeeData">Current employee info.</param>
        private void CheckDataToAddToGrid(EmployeeInGroup employeeInGrop, EmployeeInfo employeeData)
        {
            if (employeeInGrop != null)
            {
                EmployeeInfo employeeInfo = employeeInGrop.Employees.Where(x => x.SysId == employeeData.SysId).SingleOrDefault();
                Employee employeeInfoMapping = Mapper.Map<EmployeeInfo, Employee>(employeeInfo);
                if (OrderOwner == null)
                {
                    OrderOwner = employeeInfoMapping;
                }

                if (employeeInfo.Quantity > 0)
                {
                    if (MainScreenVisible == ConstantValue.Visible)
                    {
                        this.ShowErrorMessage("You are already included in another order");
                    }
                    else
                    {
                        ExecuteSideDishesCommand();
                        ((SideDishesViewModel)CurrentViewModel).ToggleMainMenu();
                    }
                }
                else
                {
                    this.MainButtonEnable = true;
                    this.SideButtonEnable = true;
                    ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(employeeInfoMapping, CurrentCulture, this);
                }

                CheckCurrentScreen();
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
                Connection.Start().Wait();

                while (Active)
                {
                    Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        /// <summary>
        /// Connect to local hub.
        /// </summary>
        private void InitializeHubConnectionLocal()
        {
            ActiveLocal = true;
            ThreadLocal = new Thread(() =>
            {
                ConnectionLocal = new HubConnection(_hostLocal);
                ProxyLocal = ConnectionLocal.CreateHubProxy("order");
                ConnectionLocal.Start().Wait();

                while (ActiveLocal)
                {
                    Thread.Sleep(10);
                }
            }) { IsBackground = true };
            ThreadLocal.Start();
        }

        #endregion
    }
}
