// --------------------------------------------------------------------------
// <copyright file="DeliveredFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Services.Implement;

    /// <summary>
    /// Delivered Food ViewModel.
    /// </summary>
    public class DeliveredFoodViewModel : ViewModelBase
    {
        #region Fields

        private ObservableCollection<Order> _deliveredMeals;
        private string _searchValue;
        private MainViewModel _mainViewModel;

        #endregion

        #region Constructors

        public DeliveredFoodViewModel()
            : this(null, null)
        {
        }

        public DeliveredFoodViewModel(string searchValue, MainViewModel mainViewModel)
        {
            SearchValue = searchValue;
            DeliveredMeals = this.GetDeliveredMealOrder;
            MainViewModel = mainViewModel;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> DeliveredMeals
        {
            get
            {
                return _deliveredMeals;
            }

            set
            {
                if (_deliveredMeals == value)
                {
                    return;
                }

                _deliveredMeals = value;
                OnPropertyChanged("DeliveredMeals");
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
                if (_mainViewModel == value)
                {
                    return;
                }

                _mainViewModel = value;
                OnPropertyChanged("MainViewModel");
            }
        }

        public ObservableCollection<Order> GetDeliveredMealOrder
        {
            get
            {
                List<Order> deliveredMeals = new List<Order>();
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    var getOrderDetailsCriteria = new GetOrderDetailsByOrderNoCriteria() { OrderNo = int.Parse(SearchValue) };
                    Order order = KitchenService.GetOrderDetailsByOrderNo(getOrderDetailsCriteria);
                    if (order != null &&
                        (order.CurrentStatusId == (int)OrderStatus.DeliveredOrder
                        || order.CurrentStatusId == (int)OrderStatus.EditedOrder))
                    {
                        deliveredMeals.Add(order);
                    }
                }
                else
                {
                    var criteria = new GetDeliveredMealOrderCriteria();
                    deliveredMeals = KitchenService.GetDeliveredMealOrder(criteria);
                }

                return new ObservableCollection<Order>(deliveredMeals);
            }
        }

        public string SearchValue
        {
            get
            {
                return _searchValue;
            }

            set
            {
                if (_searchValue == value)
                {
                    return;
                }

                _searchValue = value;
                OnPropertyChanged("SearchValue");
            }
        }

        #endregion

        #region Methods

        public void NewOrderDataInDeliveredMeals(Order order)
        {
            if (!DeliveredMeals.Any(x => x.OrderId == order.OrderId))
            {
                DeliveredMeals.Add(order);
            }
        }

        public void DeleteDataInDeliveredMeals(int orderId)
        {
            if (DeliveredMeals.Any(x => x.OrderId == orderId))
            {
                Order currentOrder = DeliveredMeals.Where(x => x.OrderId == orderId).FirstOrDefault();
                DeliveredMeals.Remove(currentOrder);
            }
        }

        public void GetNewDataForApp()
        {
            DeliveredMeals = this.GetDeliveredMealOrder;
        }

        #endregion
    }
}
