// --------------------------------------------------------------------------
// <copyright file="CombinedDishesProperty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.9">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels.Properties
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NexFB.WinAppSmartCard.Core.Models;

    /// <summary>
    /// Combined Dishes Property.
    /// </summary>
    public class CombinedDishesProperty : ViewModelBase
    {
        private Employee _orderOwner;
        private Employee _currentEmployeeOrder;
        private ObservableCollection<Employee> _observableListEmployee = new ObservableCollection<Employee>();
        private ObservableCollection<Food> _currentListSideFood = new ObservableCollection<Food>();
        private ObservableCollection<Food> _selectedSideFood = new ObservableCollection<Food>();

        private MainViewModel _mainViewModel;
        private string _totalOrder;
        private string _totalPayment;
        private string _deleteButtonVisible;
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private bool _takeAwayChecked;
        private bool _canTakeAway;
        private int _countDownTimer;
        private int _orderNo;
        private bool _sideDishTimerLive;

        public ObservableCollection<Employee> ObservableListEmployee
        {
            get
            {
                return _observableListEmployee;
            }

            set
            {
                _observableListEmployee = value;
                this.OnPropertyChanged("ObservableListEmployee");
            }
        }

        public ICommand AddSideFoodPopup { get; set; }

        public ICommand ConfirmSideFoodPopup { get; set; }

        public ICommand CloseSideFoodPopup { get; set; }

        public ICommand DeleteEmployeeCommand { get; set; }

        public ICommand DeletedFoodCommand { get; set; }

        public ICommand SubQuantityCommand { get; set; }

        public ICommand AddQuantityCommand { get; set; }

        public ICommand SaveOrderFood { get; set; }

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                _mainViewModel = value;
                OnPropertyChanged("MainViewModel");
            }
        }

        public bool TakeAwayChecked
        {
            get
            {
                return _takeAwayChecked;
            }

            set
            {
                _takeAwayChecked = value;
                this.OnPropertyChanged("TakeAwayChecked");
            }
        }

        public bool CanTakeAway
        {
            get
            {
                return _canTakeAway;
            }

            set
            {
                _canTakeAway = value;
                this.OnPropertyChanged("CanTakeAway");
            }
        }

        public string TotalOrder
        {
            get
            {
                return _totalOrder;
            }

            set
            {
                _totalOrder = value;
                OnPropertyChanged("TotalOrder");
            }
        }

        public string TotalPayment
        {
            get
            {
                return _totalPayment;
            }

            set
            {
                _totalPayment = value;
                OnPropertyChanged("TotalPayment");
            }
        }

        public string DeleteButtonVisible
        {
            get
            {
                return _deleteButtonVisible;
            }

            set
            {
                _deleteButtonVisible = value;
                OnPropertyChanged("DeleteButtonVisible");
            }
        }

        public string ScreenOrderVisible
        {
            get
            {
                return _screenOrderVisible;
            }

            set
            {
                _screenOrderVisible = value;
                OnPropertyChanged("ScreenOrderVisible");
            }
        }

        public string ScreenNotificationVisible
        {
            get
            {
                return _screenNotificationVisible;
            }

            set
            {
                _screenNotificationVisible = value;
                OnPropertyChanged("ScreenNotificationVisible");
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

        public Employee CurrentEmployeeOrder
        {
            get
            {
                return _currentEmployeeOrder;
            }

            set
            {
                _currentEmployeeOrder = value;
                OnPropertyChanged("CurrentEmployeeOrder");
            }
        }

        public ObservableCollection<Food> CurrentListSideFood
        {
            get
            {
                return _currentListSideFood;
            }

            set
            {
                _currentListSideFood = value;
                this.OnPropertyChanged("CurrentListSideFood");
            }
        }

        public ObservableCollection<Food> SelectedSideFood
        {
            get
            {
                return _selectedSideFood;
            }

            set
            {
                _selectedSideFood = value;
                this.OnPropertyChanged("SelectedSideFood");
            }
        }

        public int CountDownTimer
        {
            get
            {
                return _countDownTimer;
            }

            set
            {
                _countDownTimer = value;
                this.OnPropertyChanged("CountDownTimer");
            }
        }

        public int OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
                this.OnPropertyChanged("OrderNo");
            }
        }

        public bool SideDishTimerLive
        {
            get
            {
                return _sideDishTimerLive;
            }

            set
            {
                _sideDishTimerLive = value;
                OnPropertyChanged("SideDishTimerLive");
            }
        }
    }
}
