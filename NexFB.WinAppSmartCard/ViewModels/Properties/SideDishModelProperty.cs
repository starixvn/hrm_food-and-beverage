// --------------------------------------------------------------------------
// <copyright file="SideDishModelProperty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.18">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels.Properties
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using NexFB.WinAppSmartCard.Core.Models;

    /// <summary>
    /// Side Dish Model Property.
    /// </summary>
    public class SideDishModelProperty : ViewModelBase
    {
        private Employee _orderOwner;
        private decimal _totalPrice;
        private int _totalItem;
        private int _countDownTimer;
        private int _orderNo;
        private ObservableCollection<SideFood> _observableSideOrder = new ObservableCollection<SideFood>();
        private Food _selectedFood;
        private List<int> _hasSelectedFood = new List<int>();
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private bool _addFoodEnable;
        private bool _sideDishTimerLive;
        private bool _takeAwayChecked;

        public ICommand AddFoodCommand { get; set; }

        public ICommand DeletedFoodCommand { get; set; }

        public ICommand AddQuantityCommand { get; set; }

        public ICommand SubQuantityCommand { get; set; }

        public ICommand SaveDataCommand { get; set; }

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

        public decimal TotalPrice
        {
            get
            {
                return _totalPrice;
            }

            set
            {
                _totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public ObservableCollection<SideFood> ObservableSideOrder
        {
            get
            {
                return _observableSideOrder;
            }

            set
            {
                _observableSideOrder = value;
                OnPropertyChanged("ObservableSideOrder");
            }
        }

        public Food SelectedFood
        {
            get
            {
                return _selectedFood;
            }

            set
            {
                _selectedFood = value;
                this.OnPropertyChanged("SelectedFood");
            }
        }

        public List<int> HasSelectedFood
        {
            get
            {
                return _hasSelectedFood;
            }

            set
            {
                _hasSelectedFood = value;
                this.OnPropertyChanged("HasSelectedFood");
            }
        }

        public int TotalItem
        {
            get
            {
                return _totalItem;
            }

            set
            {
                _totalItem = value;
                this.OnPropertyChanged("TotalItem");
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

        public bool AddFoodEnable
        {
            get
            {
                return _addFoodEnable;
            }

            set
            {
                _addFoodEnable = value;
                this.OnPropertyChanged("AddFoodEnable");
            }
        }
    }
}
