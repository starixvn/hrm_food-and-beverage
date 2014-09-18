// --------------------------------------------------------------------------
// <copyright file="WaitingMealViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Documents;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class WaitingMealViewModel : ViewModelBase
    {
        #region Fields
        private static ObservableCollection<Order> _waitingMeals = new ObservableCollection<Order>();
        private string _searchValue;

        #endregion

        #region Constructors
        public WaitingMealViewModel()
            : this(null, null)
        {
        }

        public WaitingMealViewModel(string searchValue, Order newOrder)
        {
            SearchValue = searchValue;
            WaitingMeals = this.GetWaitingMealOrder(newOrder);
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> WaitingMeals
        {
            get
            {
                return _waitingMeals;
            }

            set
            {
                if (_waitingMeals == value)
                {
                    return;
                }

                _waitingMeals = value;
                OnPropertyChanged("WaitingMeals");
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
        /// Get Waiting Meals.
        /// </summary>
        /// <param name="newOrder">Add newOrder if current does not exist.</param>
        /// <returns>Observable Collection Orders.</returns>
        public ObservableCollection<Order> GetWaitingMealOrder(Order newOrder)
        {
            var criteria = new GetWaitingMealOrderCriteria();
            List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
            if (newOrder != null)
            {
                if (!waitingMeals.Any(x => x.OrderId == newOrder.OrderId))
                {
                    waitingMeals.Add(newOrder);
                }
            }

            if (!string.IsNullOrEmpty(SearchValue))
            {
                waitingMeals = waitingMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
            }

            ObservableCollection<Order> observableWaitingMeals = new ObservableCollection<Order>(waitingMeals);

            return observableWaitingMeals;
        }

        #endregion
    }
}
