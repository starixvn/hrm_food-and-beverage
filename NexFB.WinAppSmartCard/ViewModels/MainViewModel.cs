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
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using AutoLogOffHelper;
    using AutoMapper;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.InternetBased;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Services;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using WPFMessageBoxCustomControl.Enums;

    /// <summary>
    /// Main View Model.
    /// </summary>
    public partial class MainViewModel : MainModelProperty
    {
        private static string _host = ConfigurationManager.AppSettings[ConstantValue.HubHost];
        private static string _hostLocal = ConfigurationManager.AppSettings[ConstantValue.HubHostLocal];
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            IsMainHub = DBConnectionAvailability.MainDBConnectionAvailable();
            InitializeHubConnection();
            InitializeHubConnectionLocal();
            ExecuteMainDishesCommand();
            CancelCommand = new RelayCommand((x) => CanExecuteCancelCommand(), (y) => ExecuteCancelCommand());
            MainDishesCommand = new RelayCommand((x) => CanExecuteMainDishesCommand(), (y) => ExecuteMainDishesCommand());
            SideDishesCommand = new RelayCommand((x) => CanExecuteSideDishesCommand(), (y) => ExecuteSideDishesCommand());
            ChangeLanguageCommand = new RelayCommand((x) => CanExecuteChangeLanguageCommand(), (y) => ExecuteChangeLanguageCommand(y));
            ReturnStandByScreenCommand = new RelayCommand((x) => CanExecuteReturnStandByScreen(), (y) => ExecuteReturnToStandByScreen());
            ChangeCulture();
            CheckCurrentScreen();
            this.SideButtonEnable = true;
        }

        #region Properties

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

        #endregion
        #region Public Methods

        /// <summary>
        /// On change Card Number.
        /// </summary>
        /// <param name="newCardNumber">New Card number.</param>
        /// <param name="mainWindow">Current Main Window.</param>
        public void OnSmartCardNumberchanged(string newCardNumber, MainWindow mainWindow)
        {
            if (!string.IsNullOrEmpty(newCardNumber))
            {
                this.CurrentCardNumber = newCardNumber;
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
                string cardValue = newCardNumber.Substring(newCardNumber.Length - 5, 5);
                var getEmployeeByCardNumber = new GetEmployeeByCardNumberCriteria() { CardNumber = cardValue };
                EmployeeInfo employeeData = this.SmartCardOrderService.GetEmployeeByCardId(getEmployeeByCardNumber);

                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        if (((MainDishesViewModel)CurrentViewModel).MainDishTimerLive)
                        {
                            ReturnToStandByScreenAndCreateNewSession();
                        }

                        AddDataWhenTimerLive(employeeData, false);

                        break;
                    case ConstantValue.SideDishesViewModel:
                        if (((SideDishesViewModel)CurrentViewModel).SideDishTimerLive)
                        {
                            ReturnToStandByScreenAndCreateNewSession();
                        }

                        AddDataWhenTimerLive(employeeData, true);

                        break;
                    default:
                        break;
                }

                Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    AutoLogOff.ResetLogoffTimer();
                    mainWindow.InitializeAutoLogoffFeature();
                }));
            }
        }

        public void ExecuteCancelCommand()
        {
            WPFMessageBoxResult cancelOrder = this.ShowWarningCancelOrder(this.CurrentCulture);
            if (cancelOrder == WPFMessageBoxResult.Yes)
            {
                ExecuteReturnToStandByScreen();
            }
        }

        public void ExecuteReturnToStandByScreen()
        {
            this.MainTabReadOnly = false;
            CurrentCardNumber = string.Empty;
            OrderOwner = null;
            IsMainDishTab = false;
            ExecuteMainDishesCommand();
            CheckCurrentScreen();
        }

        public void ReturnToStandByScreenAndCreateNewSession()
        {
            this.MainTabReadOnly = false;
            OrderOwner = null;
            IsMainDishTab = false;
            ExecuteMainDishesCommand();
            MainScreenVisible = ConstantValue.Hidden;
            StandbyScreenVisible = ConstantValue.Visible;
        }

        public Task SendMessage(int orderId)
        {
            if (IsMainHub)
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
            return IsAllowChangeLanguage;
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
            if (!IsMainDishTab)
            {
                if (OrderOwner == null)
                {
                    SelectMainDishAction();
                }
                else
                {
                    if (OrderOwner.MenuId == ConstantValue.NoMenuAvailableCode)
                    {
                        this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_NoMenu);
                    }
                    else
                    {
                        IsMainDishTab = true;
                        CancelDispatcherTimer();
                        CurrentViewModel = new MainDishesViewModel();
                        if (OrderOwner.AllowOrder)
                        {
                            ((MainDishesViewModel)CurrentViewModel).ShowOrderScreen();
                        }
                        else
                        {
                            // Show order details
                            ((MainDishesViewModel)CurrentViewModel).ShowNotificationScreen();
                        }

                        ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(OrderOwner, this);
                        CheckCurrentViewModel();
                    }
                }
            }
        }

        /// <summary>
        /// Action when press button Select Main dish.
        /// </summary>
        private void SelectMainDishAction()
        {
            IsMainDishTab = true;
            CancelDispatcherTimer();
            CurrentViewModel = new MainDishesViewModel();
            if (MainTabReadOnly)
            {
                ((MainDishesViewModel)CurrentViewModel).ShowNotificationScreen();
            }
            else
            {
                ((MainDishesViewModel)CurrentViewModel).ShowOrderScreen();
            }

            if (OrderOwner != null)
            {
                ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(OrderOwner, this);
            }

            CheckCurrentViewModel();
        }

        private void ExecuteSideDishesCommand()
        {
            IsMainDishTab = false;
            CancelDispatcherTimer();
            CurrentViewModel = new SideDishesViewModel(OrderOwner, this);
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
                CheckDataForLanguageChange();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Refresh data for language change.
        /// </summary>
        private void CheckDataForLanguageChange()
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
            switch (currentViewModel)
            {
                case ConstantValue.MainDishesViewModel:
                    ((MainDishesViewModel)CurrentViewModel).CheckDataForLanguageChange(OrderOwner, this);
                    break;
                case ConstantValue.SideDishesViewModel:
                    ((SideDishesViewModel)CurrentViewModel).CheckDataForLanguageChange();
                    break;
            }
        }

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
        /// <param name="employeeData">Current employee info.</param>
        /// /// <param name="isSideDishView">Check for main dish tab or side dish tab.</param>
        private void CheckDataToAddToGrid(EmployeeInfo employeeData, bool isSideDishView)
        {
            if (employeeData != null)
            {
                var getEmployeeOrderStatus = new GetEmployeeOrderStatusCriteria() { SysId = employeeData.SysId };
                EmployeeOrderStatus orderStatus = this.KitchenService.EmployeeOrderStatus(getEmployeeOrderStatus);
                if (orderStatus != null)
                {
                    employeeData.AllowOrder = orderStatus.IsAllowOrder;
                    employeeData.MenuEngName = orderStatus.MenuEngName;
                    employeeData.MenuVnmName = orderStatus.MenuVnmName;
                    employeeData.MenuId = orderStatus.MenuId;

                    Employee employeeInfoMapping = Mapper.Map<EmployeeInfo, Employee>(employeeData);
                    if (OrderOwner == null)
                    {
                        OrderOwner = employeeInfoMapping;
                    }

                    if (!employeeInfoMapping.AllowOrder)
                    {
                        if (MainScreenVisible == ConstantValue.Visible)
                        {
                            if (!isSideDishView)
                            {
                                this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_AlreadyOrder);
                            }
                        }
                        else
                        {
                            ExecuteSideDishesCommand();
                            ((SideDishesViewModel)CurrentViewModel).ToggleMainMenu();
                            this.MainTabReadOnly = true;
                        }
                    }
                    else
                    {
                        MainTabReadOnly = false;
                        ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(employeeInfoMapping, this);
                    }

                    CheckCurrentScreen();
                }
                else
                {
                    this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_UnhandledError);
                }
            }
        }

        /// <summary>
        /// Process continue to add data when timer is tick.
        /// </summary>
        /// <param name="employeeData">Employee data.</param>
        /// <param name="isSideDishView">Check for main dish tab or side dish tab.</param>
        private void AddDataWhenTimerLive(EmployeeInfo employeeData, bool isSideDishView)
        {
            if (employeeData != null)
            {
                CheckDataToAddToGrid(employeeData, isSideDishView);
            }
            else
            {
                this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_IdentifyCard);
            }
        }

        /// <summary>
        /// Connect to server hub.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void InitializeHubConnection()
        {
            Active = true;
            Thread = new Thread(() =>
            {
                Connection = new HubConnection(_host);
                Proxy = Connection.CreateHubProxy("order");
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
        /// Connect to local hub.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        private void InitializeHubConnectionLocal()
        {
            ActiveLocal = true;
            ThreadLocal = new Thread(() =>
            {
                ConnectionLocal = new HubConnection(_hostLocal);
                ProxyLocal = ConnectionLocal.CreateHubProxy("order");
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

        #endregion
    }
}
