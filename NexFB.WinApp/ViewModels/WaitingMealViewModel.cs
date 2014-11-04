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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Documents;
    using NexFB.Core.Enums;
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
                List<Order> waitingMeals = new List<Order>();
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    var getOrderDetailsCriteria = new GetOrderDetailsByOrderNoCriteria() { OrderNo = int.Parse(SearchValue) };
                    Order order = KitchenService.GetOrderDetailsByOrderNo(getOrderDetailsCriteria);
                    if (order != null)
                    {
                        waitingMeals.Add(order);
                    }
                }
                else
                {
                    var criteria = new GetWaitingMealOrderCriteria();
                    waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                }

                return new ObservableCollection<Order>(waitingMeals);
            }
        }

        public void EditDataInWaitingMeals(Order order)
        {
            if (WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                var oldOrder = WaitingMeals.Where(x => x.OrderId == order.OrderId).FirstOrDefault();
                WaitingMeals.Remove(oldOrder);
                NewOrderDataAndSort(order);
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

        /// <summary>
        /// Insert New Order and Sort.
        /// </summary>
        /// <param name="order">New order.</param>
        public void NewOrderDataAndSort(Order order)
        {
            if (!WaitingMeals.Any(x => x.OrderId == order.OrderId))
            {
                if (order.CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    Order lastNewOrder = WaitingMeals.Where(x => x.CurrentStatusId == (int)OrderStatus.NewOrder).LastOrDefault();
                    if (lastNewOrder != null)
                    {
                        int index = WaitingMeals.IndexOf(lastNewOrder);
                        WaitingMeals.Insert(index + 1, order);
                    }
                    else
                    {
                        WaitingMeals.Insert(0, order);
                    }
                }
                else
                {
                    WaitingMeals.Add(order);
                }
            }
        }

        #endregion
    }
}
