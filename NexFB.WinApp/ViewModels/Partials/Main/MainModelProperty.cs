// --------------------------------------------------------------------------
// <copyright file="MainModelProperty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.27">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels.Partials.Main
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Models;

    /// <summary>
    /// Main Model Property.
    /// </summary>
    public class MainModelProperty : ViewModelBase
    {
        private static string _delayTime = ConfigurationManager.AppSettings[Constant.DatabaseDelayTime];
        private static string _autoRefeshTime = ConfigurationManager.AppSettings[Constant.AutoRefeshTime];
        private static double _displayBanner = Constant.BannerResolution1280;

        private ViewModelBase _currentViewModel;
        private string _searchResult;
        private string _waitingSearchResult;
        private string _awaitingDeliverSearchResult;
        private string _deliveredSearchResult;
        private string _keypadVisibility;
        private string _searchTextBoxVisibility;
        private double _displayWidth;
        private double _keypadHeight;
        private int _keypadFontSize;

        private MenuColor _menu1 = new MenuColor();
        private MenuColor _menu2 = new MenuColor();
        private MenuColor _menu3 = new MenuColor();
        private MenuColor _menu4 = new MenuColor();
        private MenuColor _menu5 = new MenuColor();

        private string _todayDate;
        private string _todayTime;

        public Thread Thread { get; set; }

        public IHubProxy Proxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public Thread ThreadLocal { get; set; }

        public IHubProxy ProxyLocal { get; set; }

        public HubConnection ConnectionLocal { get; set; }

        public bool ActiveLocal { get; set; }

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

        public ICommand SettingCommand { get; set; }

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

        public double DelayTime
        {
            get
            {
                double delayTime = 0;
                if (!string.IsNullOrEmpty(_delayTime))
                {
                    delayTime = double.Parse(_delayTime);
                }

                return delayTime;
            }
        }

        public double AutoRefeshTime
        {
            get
            {
                double refeshTime = 0;
                if (!string.IsNullOrEmpty(_autoRefeshTime))
                {
                    refeshTime = double.Parse(_autoRefeshTime);
                }

                return refeshTime;
            }
        }

        public string SearchResult
        {
            get
            {
                return _searchResult;
            }

            set
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

            set
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

            set
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

            set
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

        public string SearchTextBoxVisibility
        {
            get
            {
                return _searchTextBoxVisibility;
            }

            set
            {
                _searchTextBoxVisibility = value;
                this.OnPropertyChanged("SearchTextBoxVisibility");
            }
        }

        public double DisplayWidth
        {
            get
            {
                _displayWidth = ScreenWidth - Constant.RemainingWidth;
                return _displayWidth;
            }

            set
            {
                _displayWidth = value;
                this.OnPropertyChanged("DisplayWidth");
            }
        }

        public double KeypadHeight
        {
            get
            {
                if (ScreenHeight > 800)
                {
                    _keypadHeight = 400;
                }
                else
                {
                    _keypadHeight = 250;
                }

                return _keypadHeight;
            }

            set
            {
                _keypadHeight = value;
                this.OnPropertyChanged("KeypadHeight");
            }
        }

        public int KeypadFontSize
        {
            get
            {
                if (ScreenHeight > 800)
                {
                    _keypadFontSize = 38;
                }
                else
                {
                    _keypadFontSize = 25;
                }

                return _keypadFontSize;
            }

            set
            {
                _keypadFontSize = value;
                this.OnPropertyChanged("KeypadFontSize");
            }
        }

        public double DisplayBanner
        {
            get
            {
                if (ScreenWidth <= 1024)
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
    }
}
