// --------------------------------------------------------------------------
// <copyright file="OrderFoodData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels.Partials.OrderFood
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using AutoMapper;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Models;
    using NexFB.Resources;

    /// <summary>
    /// Order Food Data.
    /// </summary>
    public class OrderFoodData : ViewModelBase
    {
        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                return new ObservableCollection<Order>(waitingMeals);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllLocation
        {
            get
            {
                var getAllLocation = new GetAllLocationCriteria();
                List<Location> locations = this.CommonService.GetAllLocation(getAllLocation).ToList();
                if (locations != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllFood
        {
            get
            {
                var getAlternativeFood = new GetAlternativeFoodCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllAlternativeFoodOrder(getAlternativeFood).ToList();
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllAlternativeFood
        {
            get
            {
                var getAllFood = new GetFoodNameCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllFoodOrder(getAllFood);
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllGroup
        {
            get
            {
                var getAllGroup = new GetGroupNameCriteria();
                List<GroupOrder> groups = this.KitchenService.GetAllGroupOrder(getAllGroup);
                if (groups != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        /// <summary>
        /// Display employee order summary.
        /// </summary>
        /// <param name="currentSelectedMember">Current selected employee.</param>
        /// <returns>String order details.</returns>
        public string DisplayEmployeeOrderSummary(KitchenEmployeeInfo currentSelectedMember)
        {
            if (currentSelectedMember != null)
            {
                var listMainOrder = currentSelectedMember.ListMainMenuOrderDeatails;
                var listAlternativeOrder = currentSelectedMember.ListAlternativeMenuOrderDeatails;
                int sumMainOrder = listMainOrder.Sum(x => x.SelectedMenuItemQuantity);
                int sumAlternativeOrder = listAlternativeOrder.Sum(x => x.SelectedMenuItemQuantity);
                string orderInfoSummary = string.Empty;

                if (listMainOrder.Count > 0)
                {
                    orderInfoSummary += CombineDataOrderSummary(listMainOrder);
                }

                orderInfoSummary += " - ";
                if (listAlternativeOrder.Count > 0)
                {
                    orderInfoSummary += CombineDataOrderSummary(listAlternativeOrder);
                }

                if (orderInfoSummary.Count() > 100)
                {
                    orderInfoSummary = string.Empty;
                    if (listAlternativeOrder.Count > 0)
                    {
                        if (listMainOrder.Count > 0)
                        {
                            orderInfoSummary += WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.CommaSeparated
                                + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
                        }
                        else
                        {
                            orderInfoSummary += WinFormMainResource.Parentheses1 + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
                        }
                    }
                    else
                    {
                        if (listMainOrder.Count > 0)
                        {
                            orderInfoSummary += WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.Parentheses2;
                        }
                    }
                }

                return orderInfoSummary;
            }

            return string.Empty;
        }

        /// <summary>
        /// Display order details.
        /// </summary>
        /// <param name="listOrderDetails">List order details.</param>
        /// <returns>String order details.</returns>
        public string CombineDataOrderSummary(List<EmployeeOrderDetails> listOrderDetails)
        {
            string orderInfoSummary = string.Empty;
            var lastItem = listOrderDetails.Last();
            foreach (var item in listOrderDetails)
            {
                if (item.SelectedMenuItemQuantity > 1)
                {
                    orderInfoSummary += item.SelectedMenuItemQuantity.ToString()
                        + WinFormMainResource.QuantityPerOrder
                        + WinFormMainResource.Parentheses1
                        + item.SelectedMenuItemName
                        + WinFormMainResource.Parentheses2;
                }
                else
                {
                    orderInfoSummary += WinFormMainResource.Parentheses1
                         + item.SelectedMenuItemName
                         + WinFormMainResource.Parentheses2;
                }

                if (item != lastItem)
                {
                    orderInfoSummary += WinFormMainResource.CommaSeparated;
                }
            }

            return orderInfoSummary;
        }
    }
}
