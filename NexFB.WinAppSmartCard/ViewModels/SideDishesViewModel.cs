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
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.ViewModels.Properties;

    /// <summary>
    /// Side Dishes ViewModel.
    /// </summary>
    public class SideDishesViewModel : SideDishModelProperty
    {
        private static DispatcherTimer _digitalClockDispatcher = new DispatcherTimer();
        private MainViewModel _mainViewModel;

        public SideDishesViewModel(Employee orderOwner, MainViewModel mainViewModel)
        {
            TakeAwayChecked = false;
            ShowOrderScreen();
            InitializeRelayCommand();
            InitializeSideFoodData(orderOwner, mainViewModel);
        }

        #region Properties

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

        #endregion

        #region Public Methods

        public void CancelTimerDispatcher()
        {
            SideDishTimerLive = false;
            _digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        public void ToggleMainMenu()
        {
            this.AddFoodEnable = false;
        }

        public void CheckDataForLanguageChange()
        {
            if (ObservableSideOrder.Count == 1)
            {
                ObservableSideOrder = new ObservableCollection<SideFood>();
                ExecuteAddFoodCommand();
            }
        }

        public void CheckAllowChangeLanguage()
        {
            MainViewModel.IsAllowChangeLanguage = this.ObservableSideOrder.Count < 2;
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

        /// <summary>
        /// Save data to database.
        /// </summary>
        private void ExecuteSaveDataCommand()
        {
            HasSelectedFood = new List<int>();
            if ((ObservableSideOrder.Count > 0 && ObservableSideOrder.All(x => x.SelectedValueDropDown == int.MaxValue)) || ObservableSideOrder.Count == 0)
            {
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ContainFood);
            }
            else
            {
                ObservableSideOrder = new ObservableCollection<SideFood>(this.ObservableSideOrder.Where(x => x.SelectedValueDropDown != int.MaxValue));
                var orderDetails = new StringBuilder(ConstantValue.BeginRootTag);
                if (ObservableSideOrder.Any() && ObservableSideOrder.Count > 0)
                {
                    foreach (SideFood item in ObservableSideOrder)
                    {
                        if (item.SelectedValueDropDown > 0 && item.SelectedValueDropDown != int.MaxValue)
                        {
                            if (!HasSelectedFood.Any(x => x == item.SelectedValueDropDown))
                            {
                                orderDetails.AppendFormat(
                                    ConstantValue.SideDishOrderDetails,
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
                        TakeAway = TakeAwayChecked,
                        ListDetail = orderDetails.Append(ConstantValue.EndRootTag).ToString()
                    };
                    OrderResult result = this.SmartCardOrderService.InsertOrderSideFood(insertSideOrderCriteria);
                    if (result.CommandResult.Code == 0)
                    {
                        OrderSuccessAction(result);
                    }
                    else
                    {
                        this.ShowErrorMessage(MainViewModel.CurrentCulture, result.CommandResult.Message);
                    }
                }
            }
        }

        private void OrderSuccessAction(OrderResult result)
        {
            OrderNo = result.OrderNo;
            MainViewModel.SendMessage(result.OrderId);
            CountDownTimer = ConstantValue.CountDownTimer;
            ShowNotificationScreen();
            StartCloseTimer();
        }

        private void StartCloseTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, e) => TimerTick(sender, e);
            timer.Start();
        }

        /// <summary>
        /// Timer tick count down.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event Args.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            CountDownTimer -= 1;
            SideDishTimerLive = true;
            if (CountDownTimer == 0)
            {
                SideDishTimerLive = false;
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
                    this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_NoMoreDish);
                }
                else
                {
                    // Default food.
                    Food dropdownHeader = new Food()
                    {
                        FoodId = int.MaxValue,
                        FoodVnmName = CommonResources.SelectDishesVietnamese,
                        FoodEngName = CommonResources.SelectDishesEnglish,
                        FoodChineseName = CommonResources.SelectDishesChinese
                    };
                    foodMapping.Insert(0, dropdownHeader);

                    if (foodMapping.Count == 1)
                    {
                        this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_UnavailableSideDish);
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

                CheckAllowChangeLanguage();
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
        }

        private void InitializeRelayCommand()
        {
            SaveDataCommand = new RelayCommand((y) => ExecuteSaveDataCommand());
            DeletedFoodCommand = new RelayCommand((y) => ExecuteDeleteFoodCommand(y));
            SubQuantityCommand = new RelayCommand((y) => ExecuteSubQuantityCommand(y));
            AddFoodCommand = new RelayCommand((x) => CanExecuteAddFoodCommand(), (y) => ExecuteAddFoodCommand());
            AddQuantityCommand = new RelayCommand((x) => CanExecuteAddQuantityCommand(), (y) => ExecuteAddQuantityCommand(y));
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
                this.AddFoodEnable = selectedFood != int.MaxValue && selectedFood > 0;
                DisplayTotalPrice();
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

                UpdateOrderNo();
                DisplayTotalPrice();
                CheckAllowChangeLanguage();
            }
        }

        private void UpdateOrderNo()
        {
            for (int i = 0; i < ObservableSideOrder.Count; i++)
            {
                ObservableSideOrder[i].OrderNo = i + 1;
            }
        }

        private void DisplayTotalPrice()
        {
            TotalPrice = ObservableSideOrder.Sum(x => x.ChangedPrice);
            TotalItem = ObservableSideOrder.Sum(x => x.Quantity);
        }

        private void ShowOrderScreen()
        {
            ScreenOrderVisible = CommonResources.Visible;
            ScreenNotificationVisible = CommonResources.Hidden;
        }

        private void ShowNotificationScreen()
        {
            ScreenOrderVisible = CommonResources.Hidden;
            ScreenNotificationVisible = CommonResources.Visible;
        }

        #endregion
    }
}
