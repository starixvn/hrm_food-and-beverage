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
            : this(null)
        {
        }

        public AwaitingDeliverViewModel(string searchValue)
        {
            SearchValue = searchValue;
            AwaitingDeliverMeals = GetAwaitingDeliverMealOrder;
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

        public ObservableCollection<Order> GetAwaitingDeliverMealOrder
        {
            get
            {
                List<Order> awaitingDeliverMeals = new List<Order>();
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    var getOrderDetailsCriteria = new GetOrderDetailsByOrderNoCriteria() { OrderNo = int.Parse(SearchValue) };
                    Order order = KitchenService.GetOrderDetailsByOrderNo(getOrderDetailsCriteria);
                    if (order != null)
                    {
                        awaitingDeliverMeals.Add(order);
                    }
                }
                else
                {
                    var criteria = new GetAwaitingDeliverMealOrderCriteria();
                    awaitingDeliverMeals = KitchenService.GetAwaitingDeliverMealOrder(criteria);
                }

                return new ObservableCollection<Order>(awaitingDeliverMeals);
            }
        }

        public void EditDataInAwaitingDeliverMeals(Order order)
        {
            if (AwaitingDeliverMeals.Any(x => x.OrderId == order.OrderId))
            {
                Order oldOrder = AwaitingDeliverMeals.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                AwaitingDeliverMeals.Remove(oldOrder);
                AwaitingDeliverMeals.Add(order);
            }
        }

        public void DeleteDataInAwaitingDeliverMeals(int orderId)
        {
            if (AwaitingDeliverMeals.Any(x => x.OrderId == orderId))
            {
                var oldOrder = AwaitingDeliverMeals.Where(x => x.OrderId == orderId).FirstOrDefault();
                AwaitingDeliverMeals.Remove(oldOrder);
            }
        }

        public void NewOrderDataInAwaitingDeliverMeals(Order order)
        {
            if (!AwaitingDeliverMeals.Any(x => x.OrderId == order.OrderId))
            {
                AwaitingDeliverMeals.Add(order);
            }
        }

        public void GetNewDataForApp()
        {
            AwaitingDeliverMeals = this.GetAwaitingDeliverMealOrder;
        }

        #endregion
    }
}
