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
            : this(null)
        {
        }

        public WaitingMealViewModel(string searchValue)
        {
            SearchValue = searchValue;
            WaitingMeals = this.GetWaitingMealOrder;
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

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    waitingMeals = waitingMeals.Where(x => x.TwoDigitOrderNo.StartsWith(SearchValue, StringComparison.Ordinal)).ToList();
                }

                ObservableCollection<Order> observableWaitingMeals = new ObservableCollection<Order>(waitingMeals);
                return observableWaitingMeals;
            }
        }

        public void EditDataInWaitingMeals(Order order)
        {
            if (WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                var oldOrder = WaitingMeals.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                WaitingMeals.Remove(oldOrder);
                WaitingMeals.Add(order);
            }
        }

        public void DeleteDataInWaitingMeals(int orderId)
        {
            if (WaitingMeals.Any(x => x.OrderId == orderId))
            {
                Order oldOrder = WaitingMeals.Where(x => x.OrderId == orderId).FirstOrDefault();
                WaitingMeals.Remove(oldOrder);
            }
        }

        public void NewOrderDataInWaitingMeals(Order order)
        {
            if (!WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                WaitingMeals.Add(order);
            }
        }

        public void GetNewDataForApp()
        {
            WaitingMeals = this.GetWaitingMealOrder;
        }

        #endregion
    }
}
