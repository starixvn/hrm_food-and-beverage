// --------------------------------------------------------------------------
// <copyright file="AwaitingDeliverViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.8">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Awaiting Deliver ViewModel.
    /// </summary>
    public class AwaitingDeliverViewModel : ViewModelBase
    {
        #region Fields

        private static ObservableCollection<Order> _awaitingDeliverMeals = new ObservableCollection<Order>();
        private string _searchValue;

        #endregion

        #region Constructors
        public AwaitingDeliverViewModel()
            : this(null, null)
        {
        }

        public AwaitingDeliverViewModel(string searchValue, Order order)
        {
            SearchValue = searchValue;
            AwaitingDeliverMeals = GetAwaitingDeliverMealOrder(order);
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> AwaitingDeliverMeals
        {
            get
            {
                return _awaitingDeliverMeals;
            }

            set
            {
                if (_awaitingDeliverMeals == value)
                {
                    return;
                }

                _awaitingDeliverMeals = value;
                OnPropertyChanged("AwaitingDeliverMeals");
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

        /// <summary>
        /// Method Get Awaiting Deliver Meals.
        /// </summary>
        /// <param name="order">Check for existing order.</param>
        /// <returns>Observable Collection Orders.</returns>
        public ObservableCollection<Order> GetAwaitingDeliverMealOrder(Order order)
        {
            var criteria = new GetAwaitingDeliverMealOrderCriteria();
            List<Order> awaitingDeliverMeals = KitchenService.GetAwaitingDeliverMealOrder(criteria);
            if (order != null)
            {
                ////to do
            }

            if (!string.IsNullOrEmpty(SearchValue))
            {
                awaitingDeliverMeals = awaitingDeliverMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
            }

            ObservableCollection<Order> observableWaitingDeliverMeals = new ObservableCollection<Order>(awaitingDeliverMeals);

            return observableWaitingDeliverMeals;
        }

        #endregion
    }
}
