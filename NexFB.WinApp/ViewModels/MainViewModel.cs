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
        private static readonly PendingFoodViewModel PendingFoodViewModel = new PendingFoodViewModel();

        private static readonly DeliveredFoodViewModel DeliveredFoodViewModel = new DeliveredFoodViewModel();

        private ViewModelBase _currentViewModel;

        public MainViewModel()
        {
            CurrentViewModel = MainViewModel.PendingFoodViewModel;
            
            ViewPedingFoodCommand = new RelayCommand((x) => CanExecuteViewPendingFoood(), (y) => ExecuteViewPendingFood());
            ViewDeliveredFoodCommand = new RelayCommand((x) => CanExecuteViewDeliveredFood(), (y) => ExecuteViewDeliveredFood());
            CloseWindow = new RelayCommand((x) => ((Window)x).Close());
        }

        public ICommand CloseWindow { get; set; }

        public ICommand ViewPedingFoodCommand { get; set; }

        public ICommand ViewDeliveredFoodCommand { get; set; }

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

        private bool CanExecuteViewPendingFoood()
        {
            return true;
        }

        private void ExecuteViewPendingFood()
        {
            CurrentViewModel = MainViewModel.PendingFoodViewModel;
        }

        private bool CanExecuteViewDeliveredFood()
        {
            return true;
        }

        private void ExecuteViewDeliveredFood()
        {
            MainViewModel.DeliveredFoodViewModel.IsDeliveredDefault = true;
            CurrentViewModel = MainViewModel.DeliveredFoodViewModel;
        }
    }
}
