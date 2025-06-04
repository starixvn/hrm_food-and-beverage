namespace NexFB.WinAppSmartCard.ViewModels.Properties
{
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Models.Report.ViewMenuReport;
    using NexFB.Pos.Core.Models;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using Employee = Core.Models.Employee;

    /// <summary>
    /// Main Model Property.
    /// </summary>
    public class MainModelProperty : ViewModelBase
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private static double _screeHeight = SystemParameters.PrimaryScreenHeight;

        private string _currentTime;
        private int _currentCulture;
        private string _mainScreenVisible;
        private string _standbyScreenVisible;
        private string _currentCardNumber;
        private Employee _orderOwner;
        private ObservableCollection<string> _currentMeals = new ObservableCollection<string>(new string[10]);
        private ObservableCollection<string> _nextMeals = new ObservableCollection<string>(new string[10]);
        private ObservableCollection<string> _currentMealsEng = new ObservableCollection<string>(new string[10]);
        private ObservableCollection<string> _nextMealsEng = new ObservableCollection<string>(new string[10]);
        private ObservableCollection<MealTimesheet> _mealTime;
        private MenuColor _mainDishesMenu = new MenuColor();
        private MenuColor _sideDishesMenu = new MenuColor();
        private bool _mainTabReadOnly;
        private bool _sideButtonEnable;
        private bool _mainButtonEnable;
        private string _logoMargin;
        private bool _isMainDishTab;
        private bool _isAllowChangeLanguage;
        private string _idleScreenThemes;

        private bool _hasChangeLanguage;

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

        public ICommand CombinedDishesCommand { get; set; }

        public ICommand ChangeLanguageCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ReturnStandByScreenCommand { get; set; }

        public bool HasChangeLanguage
        {
            get
            {
                return _hasChangeLanguage;
            }

            set
            {
                _hasChangeLanguage = value;
                this.OnPropertyChanged("HasChangeLanguage");
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

        public bool IsAllowChangeLanguage
        {
            get
            {
                return _isAllowChangeLanguage;
            }

            set
            {
                _isAllowChangeLanguage = value;
                OnPropertyChanged("IsAllowChangeLanguage");
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

        public ObservableCollection<string> CurrentMeals
        {
            get
            {
                return _currentMeals;
            }

            set
            {
                _currentMeals = value;
                OnPropertyChanged("CurrentMeals");
            }
        }

        public ObservableCollection<string> NextMeals
        {
            get
            {
                return _nextMeals;
            }

            set
            {
                _nextMeals = value;
                OnPropertyChanged("NextMeals");
            }
        }

        public ObservableCollection<string> CurrentMealsEng
        {
            get
            {
                return _currentMealsEng;
            }

            set
            {
                _currentMealsEng = value;
                OnPropertyChanged("CurrentMealsEng");
            }
        }

        public ObservableCollection<string> NextMealsEng
        {
            get
            {
                return _nextMealsEng;
            }

            set
            {
                _nextMealsEng = value;
                OnPropertyChanged("NextMealsEng");
            }
        }

        public ObservableCollection<MealTimesheet> MealTime
        {
            get
            {
                return _mealTime;
            }

            set
            {
                _mealTime = value;
                OnPropertyChanged("MealTime");
            }
        }

        public bool MainTabReadOnly
        {
            get
            {
                return _mainTabReadOnly;
            }

            set
            {
                _mainTabReadOnly = value;
                OnPropertyChanged("MainTabReadOnly");
            }
        }

        public bool IsMainDishTab
        {
            get
            {
                return _isMainDishTab;
            }

            set
            {
                _isMainDishTab = value;
                OnPropertyChanged("IsMainDishTab");
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

        public string IdleScreenThemes
        {
            get
            {
                return _idleScreenThemes;
            }

            set
            {
                _idleScreenThemes = value;
                OnPropertyChanged("IdleScreenThemes");
            }
        }

        public string LogoMargin
        {
            get
            {
                if (_screeHeight > 900)
                {
                    _logoMargin = _screeHeight.Equals(960) ? ConstantValue.LogoMargin960 : ConstantValue.LogoMarginMoreThan960;
                }
                else
                {
                    _logoMargin = ConstantValue.LogoMarginLessThan900;
                }

                return _logoMargin;
            }

            set
            {
                _logoMargin = value;
                OnPropertyChanged("LogoMargin");
            }
        }
    }
}
