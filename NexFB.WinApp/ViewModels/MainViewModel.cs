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
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private static readonly OrderFoodViewModel OrderFoodViewModel = new OrderFoodViewModel();
        private static DispatcherTimer digitalClock = new DispatcherTimer();
        private ViewModelBase _currentViewModel;
        private string _searchResult;
        private string _waitingSearchResult;
        private string _awaitingDeliverSearchResult;
        private string _deliveredSearchResult;
        private string _gridColor;
        private string _todayDate;
        private string _todayTime;

        #endregion

        #region Contructors

        public MainViewModel()
        {
            CurrentViewModel = new WaitingMealViewModel();
            CheckCurrentViewModel();
            ViewPedingFoodCommand = new RelayCommand((x) => CanExecuteViewPendingFoood(), (y) => ExecuteViewPendingFood());
            ViewDeliveredFoodCommand = new RelayCommand((x) => CanExecuteViewDeliveredFood(), (y) => ExecuteViewDeliveredFood());
            ViewOrderFoodCommand = new RelayCommand((x) => CanExecuteViewOrderFood(), (y) => ExecuteViewOrderFood());
            ViewAwaitingDeliverFoodCommand = new RelayCommand((x) => CanExecuteViewAwaitingDeliverFood(), (y) => ExecuteViewAwaitingDeliverFood());
            CloseWindow = new RelayCommand((x) => ((Window)x).Close());

            digitalClock.Interval = new TimeSpan(0, 0, 1);
            digitalClock.Tick += new EventHandler(Timer_Click);
            digitalClock.Start();
        }

        #endregion

        #region Properties

        public ICommand CloseWindow { get; set; }

        public ICommand ViewAwaitingDeliverFoodCommand { get; set; }

        public ICommand ViewPedingFoodCommand { get; set; }

        public ICommand ViewDeliveredFoodCommand { get; set; }

        public ICommand ViewOrderFoodCommand { get; set; }

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
                string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
                if (currentViewModel == "WaitingMealViewModel")
                {
                    WaitingSearchResult = _searchResult;
                }

                if (currentViewModel == "AwaitingDeliverViewModel")
                {
                    AwaitingDeliverSearchResult = _searchResult;
                }

                if (currentViewModel == "DeliveredFoodViewModel")
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

        public string GridColor
        {
            get
            {
                return _gridColor;
            }

            private set
            {
                _gridColor = value;
                this.OnPropertyChanged("GridColor");
            }
        }

        public string TodayDate
        {
            get
            {
                return _todayDate;
            }

            private set
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

            private set
            {
                _todayTime = value;
                this.OnPropertyChanged("TodayTime");
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

        #endregion

        #region Methods

        /// <summary>
        /// Check Current View model.
        /// </summary>
        private void CheckCurrentViewModel()
        {
            string currentViewModel = this.CurrentViewModel.GetType().Name.ToString();
            switch (currentViewModel)
            {
                case "WaitingMealViewModel":
                    GridColor = Constant.DefaultMenuColor;
                    if (string.IsNullOrEmpty(WaitingSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = WaitingSearchResult;
                    }

                    break;
                case "AwaitingDeliverViewModel":
                    GridColor = Constant.Menu2Color;
                    if (string.IsNullOrEmpty(AwaitingDeliverSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = AwaitingDeliverSearchResult;
                    }

                    break;
                case "DeliveredFoodViewModel":
                    GridColor = Constant.Menu3Color;
                    if (string.IsNullOrEmpty(DeliveredSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = DeliveredSearchResult;
                    }

                    break;
                case "OrderFoodViewModel":
                    GridColor = Constant.Menu4Color;
                    break;
                default:
                    GridColor = Constant.DefaultMenuColor;
                    break;
            }
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            TodayDate = dateTimeNow.ToString("dd/MM/yyyy");
            TodayTime = dateTimeNow.ToString("HH:mm");
        }

        #endregion
    }
}
