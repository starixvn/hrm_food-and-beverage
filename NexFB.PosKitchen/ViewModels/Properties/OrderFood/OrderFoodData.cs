// --------------------------------------------------------------------------
// <copyright file="OrderFoodData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels.Properties.OrderFood
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using AutoMapper;
    using NexFB.Criteria.DeliveryArea;
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
                var getLocationCriteria = new GetAllDeliveryAreaCriteria();
                return this.GetDeliveryArea(getLocationCriteria);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllActiveLocation
        {
            get
            {
                var getLocationCriteria = new GetAllDeliveryAreaCriteria() { IsActive = true };
                return this.GetDeliveryArea(getLocationCriteria);
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

        public string DisplayEmployeeOrderSummary(KitchenEmployeeInfo currentSelectedMember)
        {
            return currentSelectedMember != null ? EmployeeOrderData(currentSelectedMember) : string.Empty;
        }

        /// <summary>
        /// Display employee order summary.
        /// </summary>
        /// <param name="currentSelectedMember">Current selected employee.</param>
        /// <returns>String order details.</returns>
        public string EmployeeOrderData(KitchenEmployeeInfo currentSelectedMember)
        {
            List<EmployeeOrderDetails> listMainOrder = currentSelectedMember.ListMainMenuOrderDeatails;
            List<EmployeeOrderDetails> listAlternativeOrder = currentSelectedMember.ListAlternativeMenuOrderDeatails;
            int sumMainOrder = listMainOrder.Sum(x => x.SelectedMenuItemQuantity);
            int sumAlternativeOrder = listAlternativeOrder.Sum(x => x.SelectedMenuItemQuantity);
            var orderInfoSummary = string.Empty;

            if (listMainOrder.Count > 0)
            {
                orderInfoSummary += CombineDataOrderSummary(listMainOrder);
            }

            if (listAlternativeOrder.Count > 0)
            {
                orderInfoSummary += " - ";
                orderInfoSummary += CombineDataOrderSummary(listAlternativeOrder);
            }

            if (orderInfoSummary.Count() > 100)
            {
                orderInfoSummary = string.Empty;
                if (listAlternativeOrder.Count > 0)
                {
                    orderInfoSummary += listMainOrder.Count > 0 ?
                        WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.CommaSeparated
                            + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2
                            :
                            WinFormMainResource.Parentheses1 + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
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
                orderInfoSummary += item.SelectedMenuItemQuantity > 1 ?
                    item.SelectedMenuItemQuantity.ToString() + WinFormMainResource.QuantityPerOrder
                        + WinFormMainResource.Parentheses1 + item.SelectedMenuItemName + WinFormMainResource.Parentheses2
                        :
                        WinFormMainResource.Parentheses1 + item.SelectedMenuItemName + WinFormMainResource.Parentheses2;

                if (item != lastItem)
                {
                    orderInfoSummary += WinFormMainResource.CommaSeparated;
                }
            }

            return orderInfoSummary;
        }

        /// <summary>
        /// Get Delivery Area.
        /// </summary>
        /// <param name="getLocationCriteria">Criteria to query DB.</param>
        /// <returns>List of CommonKitchenOrder.</returns>
        private ObservableCollection<CommonKitchenOrder> GetDeliveryArea(GetAllDeliveryAreaCriteria getAllDeliveryAreaCriteria)
        {
            List<Location> locations = this.DeliveryAreaService.GetAllDeliveryArea(getAllDeliveryAreaCriteria).ToList();
            if (locations != null)
            {
                List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                return new ObservableCollection<CommonKitchenOrder>(data);
            }

            return new ObservableCollection<CommonKitchenOrder>();
        }
    }
}
