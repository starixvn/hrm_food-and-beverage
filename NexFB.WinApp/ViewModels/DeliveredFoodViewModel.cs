// --------------------------------------------------------------------------
// <copyright file="DeliveredFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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

        #endregion

        #region Constructors

        public DeliveredFoodViewModel()
            : this(null)
        {
        }

        public DeliveredFoodViewModel(string searchValue)
        {
            SearchValue = searchValue;
            DeliveredMeals = this.GetDeliveredMealOrder;
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

        public ObservableCollection<Order> GetDeliveredMealOrder
        {
            get
            {
                var criteria = new GetDeliveredMealOrderCriteria();
                List<Order> deliveredMeals = KitchenService.GetDeliveredMealOrder(criteria);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    deliveredMeals = deliveredMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
                }

                ObservableCollection<Order> observabledeliveredMeals = new ObservableCollection<Order>(deliveredMeals);
                return observabledeliveredMeals;
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

        #endregion
    }
}
