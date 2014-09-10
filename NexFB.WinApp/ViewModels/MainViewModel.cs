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
    using System.Windows;
    using System.Windows.Input;
    using NexFB.WinApp.Core.Commands;

    /// <summary>
    /// Main ViewModel.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private static readonly OrderFoodViewModel OrderFoodViewModel = new OrderFoodViewModel();
        private ViewModelBase _currentViewModel;
        private string _searchResult;
        private string _waitingSearchResult;
        private string _awaitingDeliverSearchResult;
        private string _deliveredSearchResult;

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
                    if (string.IsNullOrEmpty(DeliveredSearchResult))
                    {
                        SearchResult = string.Empty;
                    }
                    else
                    {
                        SearchResult = DeliveredSearchResult;
                    }

                    break;
            }
        }

        #endregion
    }
}
