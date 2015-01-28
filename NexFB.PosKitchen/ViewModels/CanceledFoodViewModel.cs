// --------------------------------------------------------------------------
// <copyright file="CanceledFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.11">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Canceled Food View Model.
    /// </summary>
    public class CanceledFoodViewModel : ViewModelBase
    {
        private static ObservableCollection<Order> _canceledMeals = new ObservableCollection<Order>();
        private string _searchValue;

        public CanceledFoodViewModel()
            : this(null)
        {
        }

        public CanceledFoodViewModel(string searchValue)
        {
            SearchValue = searchValue;
            CanceledMeals = this.GetCanceledMealOrder;
        }

        #region Properties

        public ObservableCollection<Order> CanceledMeals
        {
            get
            {
                return _canceledMeals;
            }

            set
            {
                if (_canceledMeals == value)
                {
                    return;
                }

                _canceledMeals = value;
                OnPropertyChanged("CanceledMeals");
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

        public ObservableCollection<Order> GetCanceledMealOrder
        {
            get
            {
                List<Order> canceledMeals = new List<Order>();
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    if (SearchValue.Length <= 3)
                    {
                        var getOrderDetailsCriteria = new GetOrderDetailsByOrderNoCriteria() { OrderNo = int.Parse(SearchValue) };
                        IList<Order> orders = KitchenService.GetOrdersByOrderNo(getOrderDetailsCriteria);
                        foreach (Order item in orders)
                        {
                            if (item.CurrentStatusId == (int)OrderStatus.CanceledOrder)
                            {
                                canceledMeals.Add(item);
                            }
                        }
                    }
                    else
                    {
                        var getOrderDetailsCriteria = new GetMealOrderByIdCriteria() { OrderId = int.Parse(SearchValue) };
                        Order order = KitchenService.GetMealOrderById(getOrderDetailsCriteria);
                        if (order != null && order.CurrentStatusId == (int)OrderStatus.CanceledOrder)
                        {
                            canceledMeals.Add(order);
                        }
                    }
                }
                else
                {
                    var criteria = new GetCanceledMealOrderCriteria();
                    canceledMeals = KitchenService.GetCanceledMealOrder(criteria);
                }

                return new ObservableCollection<Order>(canceledMeals);
            }
        }

        #endregion

        #region Methods

        public void GetNewDataForApp()
        {
            CanceledMeals = this.GetCanceledMealOrder;
        }

        /// <summary>
        /// Method add new item to Rejected order list.
        /// </summary>
        /// <param name="order">New order item.</param>
        public void AddRejectedOrderToList(Order order)
        {
            if (order != null)
            {
                if (!CanceledMeals.Any(x => x.OrderId == order.OrderId))
                {
                    CanceledMeals.Add(order);
                }

                CanceledMeals = new ObservableCollection<Order>(this.CanceledMeals);
            }
        }

        #endregion
    }
}
