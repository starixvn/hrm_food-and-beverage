namespace NexFB.WinAppSmartCard.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml;
    using AutoLogOffHelper;
    using AutoMapper;
    using Fanex.Logging;
    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using Newtonsoft.Json;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Enums;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Services;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using WPFMessageBoxCustomControl.Enums;
    using Employee = Core.Models.Employee;

    public partial class MainViewModel : MainModelProperty
    {
        private static readonly string _host = ConfigurationManager.AppSettings[ConstantValue.HubHost];
        private static readonly DispatcherTimer _dailyClockDispatcher = new DispatcherTimer();
        private bool isUpdateBackground = false;
        private bool isUpdateMeal = false;
        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            GetMealTime();
            GetMealsToShowOnScreen();
            SetApplicationThemes();
            InitializeHubConnection();
            ExecuteMainDishesCommand();
            CancelCommand = new RelayCommand((y) => ExecuteCancelCommand());
            MainDishesCommand = new RelayCommand((y) => ExecuteMainDishesCommand());
            SideDishesCommand = new RelayCommand((y) => ExecuteSideDishesCommand());
            CombinedDishesCommand = new RelayCommand((y) => ExecuteCombineDishesCommand());
            ReturnStandByScreenCommand = new RelayCommand((y) => ExecuteReturnToStandByScreen());
            ChangeLanguageCommand = new RelayCommand((x) => CanExecuteChangeLanguageCommand(), (y) => ExecuteChangeLanguageCommand(y));

            ChangeCulture();
            CheckCurrentScreen();
            this.SideButtonEnable = true;
            this.MainButtonEnable = true;

            TimerDailyClock();
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

        #endregion Properties

        #region Public Methods

        public void OnSmartCardNumberChanged(string newCardNumber, MainWindow mainWindow)
        {
            if (!string.IsNullOrEmpty(newCardNumber))
            {
                Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() =>
                {
                    this.CurrentCardNumber = newCardNumber;
                    string currentViewModel = this.CurrentViewModel.GetType().Name;
                    string cardValue = newCardNumber.Substring(newCardNumber.Length - 5, 5);
                    var criteria = new GetEmployeeByCardNumberCriteria { CardNumber = cardValue };
                    EmployeeInfo employeeData = this.SmartCardOrderService.GetEmployeeByCardId(criteria);
                    CheckForValidCard(employeeData, currentViewModel);

                    AutoLogOff.ResetLogoffTimer();
                    mainWindow.InitializeAutoLogoffFeature();
                }));
            }
        }

        public void GetMealTime()
        {
            var criteria = new GetMealTimesheetCriteria();
            MealTime = new ObservableCollection<MealTimesheet>(this.CommonService.GetAllMealTimesheet(criteria));
        }

        public void GetMealsToShowOnScreen()
        {
            var criteria = new GetMenuShowOnScreenCriteria();
            var menu = this.SmartCardOrderService.GetMenuShowOnScreen(criteria);

            var currentMenu = menu.Where(item => item.IsCurrentMeal).ToList();
            var nextMenu = menu.Where(item => !item.IsCurrentMeal).ToList();

            NextMeals = new ObservableCollection<string>(new string[10]);
            NextMealsEng = new ObservableCollection<string>(new string[10]);
            CurrentMeals = new ObservableCollection<string>(new string[10]);
            CurrentMealsEng = new ObservableCollection<string>(new string[10]);

            GetCurrentNameMeals(currentMenu);
            GetNextNameMeals(nextMenu);

            LogService.Info("CurrentMenu: " + JsonConvert.SerializeObject(currentMenu)
                + "\nNextMenu: " + JsonConvert.SerializeObject(nextMenu));
        }

        private void GetNextNameMeals(List<FoodMenu> menu)
        {
            if (menu.Count != 0)
            {
                for (int i = 0; i < menu.Count; i++)
                {
                    NextMeals[i] = $"{i + 1}. {menu[i].FoodVNMName}";
                    NextMealsEng[i] = $"({menu[i].FoodEngName})";
                }
            }
        }

        private void GetCurrentNameMeals(List<FoodMenu> menu)
        {
            if (menu.Count != 0)
            {
                for (int i = 0; i < menu.Count; i++)
                {
                    CurrentMeals[i] = $"{i + 1}. {menu[i].FoodVNMName}";
                    CurrentMealsEng[i] = $"({menu[i].FoodEngName})";
                }
            }
        }

        public void CheckForValidCard(EmployeeInfo employeeData, string currentViewModel)
        {
            if (employeeData != null)
            {
                ChangeLanguageAccordingToEmployeeType(employeeData.IsLocal, false);
                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        if (((MainDishesViewModel)CurrentViewModel).MainDishTimerLive)
                        {
                            ReturnToStandByScreenAndCreateNewSession();
                        }

                        AddDataWhenTimerLive(employeeData);
                        break;

                    case ConstantValue.SideDishesViewModel:
                        if (((SideDishesViewModel)CurrentViewModel).SideDishTimerLive)
                        {
                            ReturnToStandByScreenAndCreateNewSession();
                            AddDataWhenTimerLive(employeeData);
                        }

                        break;

                    case ConstantValue.CombinedDishesViewModel:
                        if (((CombinedDishesViewModel)CurrentViewModel).SideDishTimerLive)
                        {
                            ReturnToStandByScreenAndCreateNewSession();
                            AddDataWhenTimerLive(employeeData);
                        }

                        break;

                    default:
                        // nothing
                        break;
                }
            }
            else
            {
                this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_IdentifyCard);
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
            CurrentCardNumber = string.Empty;
            this.MainTabReadOnly = false;
            MainButtonEnable = true;
            OrderOwner = null;
            IsMainDishTab = false;
            ExecuteMainDishesCommand();
            CheckCurrentScreen();
            HasChangeLanguage = false;
        }

        public void ReturnToStandByScreenAndCreateNewSession()
        {
            this.MainTabReadOnly = false;
            MainButtonEnable = true;
            OrderOwner = null;
            IsMainDishTab = false;
            ExecuteMainDishesCommand();
            MainScreenVisible = CommonResources.Hidden;
            StandbyScreenVisible = CommonResources.Visible;
            HasChangeLanguage = false;
        }

        public Task SendMessage(int orderId)
        {
            if (Connection.State != ConnectionState.Connected)
            {
                InitializeHubConnection();
            }

            try
            {
                return Proxy.Invoke("SendNewOrder", orderId);
            }
            catch
            {
                return Task.Factory.StartNew(new Action(() => { }));
            }
        }

        public void ChangeCulture()
        {
            switch (CurrentCulture)
            {
                case (int)LanguageLocalization.VietNamCulture:
                    ResourceManagerService.ChangeLocale("vi-VN");
                    CurrentCulture = (int)LanguageLocalization.VietNamCulture;
                    break;

                case (int)LanguageLocalization.ChinaCulture:
                    ResourceManagerService.ChangeLocale("zh-CN");
                    CurrentCulture = (int)LanguageLocalization.ChinaCulture;
                    break;

                default:
                    ResourceManagerService.ChangeLocale("en-GB");
                    CurrentCulture = (int)LanguageLocalization.EnglishCulture;
                    break;
            }
        }

        public void UpdateOrderOwnerInfoAfterOrderSuccessful()
        {
            if (OrderOwner != null)
            {
                var criteria = new GetEmployeeOrderStatusCriteria { SysId = OrderOwner.SysId };
                EmployeeOrderStatus orderStatus = this.KitchenService.EmployeeOrderStatus(criteria);
                OrderOwner.AllowOrder = orderStatus.IsAllowOrder;
            }
        }

        public void ChangeLanguageAccordingToEmployeeType(bool employeeInLocal, bool notificationScreen)
        {
            if (!HasChangeLanguage && (MainScreenVisible == CommonResources.Hidden || notificationScreen))
            {
                CurrentCulture = employeeInLocal ? (int)LanguageLocalization.VietNamCulture : (int)LanguageLocalization.EnglishCulture;
                ChangeCulture();
            }
        }

        #endregion Public Methods

        #region Commands

        private bool CanExecuteChangeLanguageCommand()
        {
            return IsAllowChangeLanguage;
        }

        private void ExecuteMainDishesCommand()
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
                        IsAllowChangeLanguage = false;
                    }

                    ((MainDishesViewModel)CurrentViewModel).AddEmployeeOrderToList(OrderOwner, this);
                    CheckCurrentViewModel();
                }
            }
        }

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
            string currentViewModel = this.CurrentViewModel.GetType().Name;
            if (currentViewModel == ConstantValue.MainDishesViewModel)
            {
                IsMainDishTab = false;
                CancelDispatcherTimer();
                CurrentViewModel = new SideDishesViewModel(OrderOwner, this);
                CheckCurrentViewModel();
            }
        }

        private void ExecuteCombineDishesCommand()
        {
            IsMainDishTab = false;
            MainButtonEnable = false;
            var listEmployee = ((MainDishesViewModel)CurrentViewModel).ObservableListEmployee;
            CurrentViewModel = new CombinedDishesViewModel(OrderOwner, listEmployee, this);
            CheckCurrentViewModel();
        }

        private void CancelDispatcherTimer()
        {
            if (CurrentViewModel != null)
            {
                string currentViewModel = this.CurrentViewModel.GetType().Name;
                switch (currentViewModel)
                {
                    case ConstantValue.MainDishesViewModel:
                        ((MainDishesViewModel)CurrentViewModel).CancelTimerDispatcher();
                        break;

                    case ConstantValue.SideDishesViewModel:
                        ((SideDishesViewModel)CurrentViewModel).CancelTimerDispatcher();
                        break;

                    case ConstantValue.CombinedDishesViewModel:
                        ((CombinedDishesViewModel)CurrentViewModel).CancelTimerDispatcher();
                        break;

                    default:
                        // do nothing
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
                HasChangeLanguage = true;
            }
        }

        #endregion Commands

        #region Private Methods

        private void CheckDataForLanguageChange()
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name;
            switch (currentViewModel)
            {
                case ConstantValue.MainDishesViewModel:
                    ((MainDishesViewModel)CurrentViewModel).CheckDataForLanguageChange(OrderOwner, this);
                    break;

                case ConstantValue.SideDishesViewModel:
                    ((SideDishesViewModel)CurrentViewModel).CheckDataForLanguageChange();
                    break;

                case ConstantValue.CombinedDishesViewModel:
                    ((CombinedDishesViewModel)CurrentViewModel).CheckDataForLanguageChange();
                    break;

                default:
                    // do nothing
                    break;
            }
        }

        private void DefaulMenuColor()
        {
            MainDishesMenu.BackgroundColor = ConstantValue.MainDishesMenuColor;
            MainDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            MainDishesMenu.FontWeight = ConstantValue.FontWeightNormal;

            SideDishesMenu.BackgroundColor = ConstantValue.SideDishesMenuColor;
            SideDishesMenu.ForegroundColor = ConstantValue.DefaultMenuColor;
            SideDishesMenu.FontWeight = ConstantValue.FontWeightNormal;
        }

        private void CheckCurrentViewModel()
        {
            DefaulMenuColor();
            string currentViewModel = this.CurrentViewModel.GetType().Name;
            if (currentViewModel == ConstantValue.MainDishesViewModel)
            {
                MainDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                MainDishesMenu.ForegroundColor = ConstantValue.MainDishesMenuColor;
                MainDishesMenu.FontWeight = ConstantValue.FontWeightBold;
            }
            else
            {
                SideDishesMenu.BackgroundColor = ConstantValue.DefaultMenuColor;
                SideDishesMenu.ForegroundColor = ConstantValue.SideDishesMenuColor;
                SideDishesMenu.FontWeight = ConstantValue.FontWeightBold;
            }
        }

        private void CheckCurrentScreen()
        {
            if (string.IsNullOrEmpty(CurrentCardNumber))
            {
                MainScreenVisible = CommonResources.Hidden;
                StandbyScreenVisible = CommonResources.Visible;
            }
            else
            {
                MainScreenVisible = CommonResources.Visible;
                StandbyScreenVisible = CommonResources.Hidden;
            }
        }

        private void CheckDataToAddToGrid(EmployeeInfo employeeData)
        {
            if (employeeData != null)
            {
                var criteria = new GetEmployeeOrderStatusCriteria { SysId = employeeData.SysId };
                EmployeeOrderStatus orderStatus = this.KitchenService.EmployeeOrderStatus(criteria);
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
                        if (MainScreenVisible == CommonResources.Visible)
                        {
                            ShowMessageCannotOrder(employeeInfoMapping);
                        }
                        else
                        {
                            this.MainTabReadOnly = false;
                            ExecuteMainDishesCommand();
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

        private void ShowMessageCannotOrder(Employee employeeInfoMapping)
        {
            this.ShowErrorMessage(
                this.CurrentCulture,
                employeeInfoMapping.MenuId == ConstantValue.NoMenuAvailableCode
                    ? MainScreenResources.Message_NoMenu
                    : MainScreenResources.Message_AlreadyOrder);
        }

        private void AddDataWhenTimerLive(EmployeeInfo employeeData)
        {
            if (employeeData != null)
            {
                CheckDataToAddToGrid(employeeData);
            }
            else
            {
                this.ShowErrorMessage(this.CurrentCulture, MainScreenResources.Message_IdentifyCard);
            }
        }

        private void SetApplicationThemes()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var xd = new XmlDocument();
                var currentDate = DateTime.Now.Date;
                int currentYear = DateTime.Now.Year;
                var currentCulture = CultureInfo.CurrentCulture;
                const string xmlDateFormat = "dd/MM/yyyy";

                xd.Load(Path.Combine(baseDirectory, "Themes.xml"));
                XmlNodeList nodelist = xd.SelectNodes("/Theme/ImageTheme");

                var themeList = nodelist.Cast<XmlNode>().Select(x => new
                {
                    From = string.Format(x.SelectSingleNode("From").InnerText, currentYear),
                    To = string.Format(x.SelectSingleNode("To").InnerText, currentYear),
                    ImageSource = Path.Combine(baseDirectory, string.Format("Images\\Themes\\{0}", x.SelectSingleNode("ImageName").InnerText))
                }).ToList();

                IdleScreenThemes = ConstantValue.NormalBackground;
                foreach (var item in themeList)
                {
                    var fromDate = DateTime.ParseExact(item.From, xmlDateFormat, currentCulture);
                    var toDate = DateTime.ParseExact(item.To, xmlDateFormat, currentCulture);

                    if (fromDate > toDate)
                    {
                        fromDate = fromDate.AddYears(-1);
                    }

                    if (currentDate >= fromDate && currentDate <= toDate)
                    {
                        IdleScreenThemes = item.ImageSource;
                    }
                }
            }
            catch (Exception)
            {
                IdleScreenThemes = ConstantValue.NormalBackground;
            }
        }

        private void TimerDailyClock()
        {
            _dailyClockDispatcher.Interval = new TimeSpan(0, 1, 0);
            _dailyClockDispatcher.Tick += new EventHandler(TimerTick);
            _dailyClockDispatcher.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var currentTime = DateTimeOffset.Now;
            var time = DateTime.Parse("1900-01-01 " + currentTime.ToString("HH:mm"), DateTimeFormatInfo.InvariantInfo);

            if (!isUpdateBackground && currentTime.Hour == 0)
            {
                SetApplicationThemes();
                isUpdateBackground = true;
            }
            else if (isUpdateBackground && currentTime.Hour != 0)
            {
                isUpdateBackground = false;
            }

            if (MealTime.Any(meal => (time - meal.StartTime).TotalMinutes < 5 && (time - meal.StartTime).TotalMinutes >= 0))
            {
                GetMealsToShowOnScreen();
            }
        }

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
                    const byte MillisecondsTimeout = 10;
                    Thread.Sleep(MillisecondsTimeout);
                }
            })
            { IsBackground = true };
            Thread.Start();
        }

        #endregion Private Methods
    }
}