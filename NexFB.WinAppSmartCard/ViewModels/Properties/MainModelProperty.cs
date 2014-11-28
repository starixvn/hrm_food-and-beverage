// --------------------------------------------------------------------------
// <copyright file="MainModelProperty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.18">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels.Properties
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Pos.Core.Models;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;

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
        private MenuColor _mainDishesMenu = new MenuColor();
        private MenuColor _sideDishesMenu = new MenuColor();
        private bool _mainTabReadOnly;
        private bool _sideButtonEnable;
        private string _logoMargin;
        private string _sentenceMargin;
        private bool _isMainDishTab;
        private bool _isMainHub;
        private bool _isAllowChangeLanguage;

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

        public bool IsMainHub
        {
            get
            {
                return _isMainHub;
            }

            set
            {
                _isMainHub = value;
                OnPropertyChanged("IsMainHub");
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

        public string LogoMargin
        {
            get
            {
                if (_screeHeight > 900)
                {
                    _logoMargin = ConstantValue.LogoMarginMoreThan900;
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

        public string SentenceMargin
        {
            get
            {
                if (_screeHeight > 900)
                {
                    _sentenceMargin = ConstantValue.SentenceMarginMoreThan900;
                }
                else
                {
                    _sentenceMargin = ConstantValue.SentenceMarginLessThan900;
                }

                return _sentenceMargin;
            }

            set
            {
                _sentenceMargin = value;
                OnPropertyChanged("SentenceMargin");
            }
        }
    }
}
