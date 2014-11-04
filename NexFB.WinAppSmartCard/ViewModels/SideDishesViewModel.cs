// --------------------------------------------------------------------------
// <copyright file="SideDishesViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.16">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;

    /// <summary>
    /// Side Dishes ViewModel.
    /// </summary>
    public class SideDishesViewModel : ViewModelBase
    {
        private const string _OrderDetails = "<User FoodId='{0}' Qty='{1}'></User>";
        private const string _BeginRootTag = "<ROOT>";
        private const string _EndRootTag = "</ROOT>";

        private static DispatcherTimer digitalClockDispatcher = new DispatcherTimer();
        private Employee _orderOwner;
        private decimal _totalPrice;
        private int _totalItem;
        private int _countDownTimer;
        private int _orderNo;
        private ObservableCollection<SideFood> _observableSideOrder = new ObservableCollection<SideFood>();
        private int _currentCulture;
        private Food _selectedFood;
        private List<int> _hasSelectedFood = new List<int>();
        private MainViewModel _mainViewModel;
        private string _screenOrderVisible;
        private string _screenNotificationVisible;
        private bool _addFoodEnable;

        public SideDishesViewModel(Employee orderOwner, int currentCulture, MainViewModel mainViewModel)
        {
            ShowOrderScreen();
            CurrentCulture = currentCulture;
            InitializeSideFoodData(orderOwner, mainViewModel);
            AddFoodCommand = new RelayCommand((x) => CanExecuteAddFoodCommand(), (y) => ExecuteAddFoodCommand());
            AddQuantityCommand = new RelayCommand((x) => CanExecuteAddQuantityCommand(), (y) => ExecuteAddQuantityCommand(y));
            SubQuantityCommand = new RelayCommand((x) => CanExecuteSubQuantityCommand(), (y) => ExecuteSubQuantityCommand(y));
            DeletedFoodCommand = new RelayCommand((x) => CanExecuteDeleteFoodCommand(), (y) => ExecuteDeleteFoodCommand(y));
            SaveDataCommand = new RelayCommand((x) => CanExecuteSaveDataCommand(), (y) => ExecuteSaveDataCommand());
        }

        #region Properties

        public ICommand AddFoodCommand { get; set; }

        public ICommand DeletedFoodCommand { get; set; }

        public ICommand AddQuantityCommand { get; set; }

        public ICommand SubQuantityCommand { get; set; }

        public ICommand SaveDataCommand { get; set; }

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

        public int CurrentCulture
        {
            get
            {
                return _currentCulture;
            }

            set
            {
                _currentCulture = value;
                OnPropertyChanged("CurrentCulture");
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

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                _mainViewModel = value;
                this.OnPropertyChanged("MainViewModel");
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

        #endregion

        #region Public Methods

        public void CancelTimerDispatcher()
        {
            digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        public void ToggleMainMenu()
        {
            this.AddFoodEnable = false;
        }

        #endregion

        #region Private Methods

        private bool CanExecuteAddFoodCommand()
        {
            if (ObservableSideOrder.Count > 0 && ObservableSideOrder.Any(x => x.SelectedValueDropDown == int.MaxValue))
            {
                return false;
            }

            return true;
        }

        private bool CanExecuteAddQuantityCommand()
        {
            if (!ObservableSideOrder.Any(x => x.SelectedValueDropDown == int.MaxValue) || ObservableSideOrder.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool CanExecuteSubQuantityCommand()
        {
            return true;
        }

        private bool CanExecuteDeleteFoodCommand()
        {
            return true;
        }

        private bool CanExecuteSaveDataCommand()
        {
            return true;
        }

        /// <summary>
        /// Save data to database.
        /// </summary>
        private void ExecuteSaveDataCommand()
        {
            HasSelectedFood = new List<int>();
            if ((ObservableSideOrder.Count > 0 && ObservableSideOrder.All(x => x.SelectedValueDropDown == int.MaxValue)) || ObservableSideOrder.Count == 0)
            {
                this.ShowErrorMessage(MainScreenResources.Message_ContainFood);
            }
            else
            {
                ObservableSideOrder = new ObservableCollection<SideFood>(this.ObservableSideOrder.Where(x => x.SelectedValueDropDown != int.MaxValue));
                var orderDetails = new StringBuilder(_BeginRootTag);
                if (ObservableSideOrder.Any() && ObservableSideOrder.Count > 0)
                {
                    foreach (SideFood item in ObservableSideOrder)
                    {
                        if (item.SelectedValueDropDown > 0 && item.SelectedValueDropDown != int.MaxValue)
                        {
                            if (!HasSelectedFood.Any(x => x == item.SelectedValueDropDown))
                            {
                                orderDetails.AppendFormat(
                                    _OrderDetails,
                                    item.SelectedValueDropDown,
                                    ObservableSideOrder.Where(x => x.SelectedValueDropDown == item.SelectedValueDropDown).Sum(x => x.Quantity));
                            }

                            HasSelectedFood.Add(item.SelectedValueDropDown);
                        }
                    }

                    var insertSideOrderCriteria = new InsertOrderSideDishesByUserCriteria()
                    {
                        CreatedBy = OrderOwner.SysId,
                        CreatedDate = DateTime.Now,
                        ListDetail = orderDetails.Append(_EndRootTag).ToString()
                    };
                    OrderResult result = this.SmartCardOrderService.InsertOrderSideFood(insertSideOrderCriteria);
                    if (result.CommandResult.Code == 0)
                    {
                        OrderNo = result.OrderNo;
                        MainViewModel.SendMessage(result.OrderId);
                        CountDownTimer = 30;
                        ShowNotificationScreen();
                        StartCloseTimer();
                    }
                    else
                    {
                        this.ShowErrorMessage(result.CommandResult.Message);
                    }
                }
            }
        }

        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => TimerTick(sender, e);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CountDownTimer -= 1;
            if (CountDownTimer == 0)
            {
                DispatcherTimer timer = (DispatcherTimer)sender;
                timer.Stop();
                MainViewModel.ExecuteReturnToStandByScreen();
            }
        }

        /// <summary>
        /// Add new side dishes.
        /// </summary>
        private void ExecuteAddFoodCommand()
        {
            var getSideFoodCriteria = new GetSideFoodBySysIdCriteria() { SysId = OrderOwner.SysId };
            List<FoodOrder> sideFoods = this.SmartCardOrderService.GetSideFoodBySysId(getSideFoodCriteria);
            if (sideFoods != null)
            {
                List<Food> foodMapping = Mapper.Map<List<FoodOrder>, List<Food>>(sideFoods);

                if (foodMapping.Count == 0)
                {
                    this.ShowErrorMessage(MainScreenResources.Message_NoMoreDish);
                }
                else
                {
                    // Default food.
                    Food dropdownHeader = new Food()
                    {
                        FoodId = int.MaxValue,
                        FoodVnmName = ConstantValue.SelectDishesVietnamese,
                        FoodEngName = ConstantValue.SelectDishesEnglish,
                    };
                    foodMapping.Insert(0, dropdownHeader);

                    if (foodMapping.Count == 1)
                    {
                        this.ShowErrorMessage(MainScreenResources.Message_UnavailableSideDish);
                    }
                    else
                    {
                        SelectedFood = dropdownHeader;
                        SideFood sideFoodData = new SideFood()
                        {
                            ListSideFood = foodMapping,
                            OrderNo = ObservableSideOrder.Count + 1,
                            OriginalPrice = 0,
                            Quantity = 0,
                            ChangedPrice = 0
                        };

                        sideFoodData.DropDownChanged += new SideFood.OnDropDownValueChanged(GetPriceChanged);

                        sideFoodData.ListSideFood.ForEach(x => { x.CurrentCulture = MainViewModel.CurrentCulture; x.CurrentItemNo = sideFoodData.OrderNo; });
                        ObservableSideOrder.Add(sideFoodData);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize data.
        /// </summary>
        /// <param name="orderOwner">Order owner.</param>
        /// <param name="mainViewModel">Main view model.</param>
        private void InitializeSideFoodData(Employee orderOwner, MainViewModel mainViewModel)
        {
            if (OrderOwner == null)
            {
                OrderOwner = orderOwner;
            }

            if (mainViewModel != null)
            {
                MainViewModel = mainViewModel;
            }

            ExecuteAddFoodCommand();
            CanExecuteAddFoodCommand();
        }

        /// <summary>
        /// Method get price changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event arguments.</param>
        private void GetPriceChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                int selectedFood = (int)sender;
                if (selectedFood != int.MaxValue && selectedFood > 0)
                {
                    DisplayTotalPrice();
                    this.AddFoodEnable = true;
                }
                else
                {
                    this.AddFoodEnable = false;
                }
            }
        }

        private void ExecuteAddQuantityCommand(object value)
        {
            if (value != null)
            {
                int orderNo = (int)value;
                SideFood selectedItem = ObservableSideOrder.Where(x => x.OrderNo == orderNo).SingleOrDefault();
                selectedItem.Quantity += 1;
                selectedItem.ChangedPrice = selectedItem.OriginalPrice * selectedItem.Quantity;
                DisplayTotalPrice();
            }
        }

        /// <summary>
        /// Subtraction quantity.
        /// </summary>
        /// <param name="value">Current item id.</param>
        private void ExecuteSubQuantityCommand(object value)
        {
            if (value != null)
            {
                int orderNo = (int)value;
                SideFood selectedItem = ObservableSideOrder.Where(x => x.OrderNo == orderNo).SingleOrDefault();
                if (selectedItem.Quantity > 1)
                {
                    selectedItem.Quantity -= 1;
                    selectedItem.ChangedPrice = selectedItem.OriginalPrice * selectedItem.Quantity;
                }

                DisplayTotalPrice();
            }
        }

        /// <summary>
        /// Delete food from list.
        /// </summary>
        /// <param name="value">Current item id.</param>
        private void ExecuteDeleteFoodCommand(object value)
        {
            if (value != null)
            {
                int orderNo = (int)value;
                SideFood deletedItem = ObservableSideOrder.Where(x => x.OrderNo == orderNo).SingleOrDefault();
                ObservableSideOrder.Remove(deletedItem);
                if (ObservableSideOrder.Count == 0)
                {
                    AddFoodEnable = true;
                }

                DisplayTotalPrice();
            }
        }

        private void DisplayTotalPrice()
        {
            TotalPrice = ObservableSideOrder.Sum(x => x.ChangedPrice);
            TotalItem = ObservableSideOrder.Sum(x => x.Quantity);
        }

        private void ShowOrderScreen()
        {
            ScreenOrderVisible = ConstantValue.Visible;
            ScreenNotificationVisible = ConstantValue.Hidden;
        }

        private void ShowNotificationScreen()
        {
            ScreenOrderVisible = ConstantValue.Hidden;
            ScreenNotificationVisible = ConstantValue.Visible;
        }

        #endregion
    }
}
