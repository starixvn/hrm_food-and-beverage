// --------------------------------------------------------------------------
// <copyright file="KitchenRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Core.Enums;
    using NexFB.Models.Kitchen;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Kitchen Repository.
    /// </summary>
    public class KitchenRepository : IKitchenRepository
    {
        /// <summary>
        /// Get Waiting Meal Order.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>List Order..</returns>
        public List<Order> GetMealOrder(ICriteria criteria)
        {
            var waitingMeals = new List<Order>();
            var menus = new List<OrderMenu>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    waitingMeals = multi.Read<Order>().ToList();
                    menus = multi.Read<OrderMenu>().ToList();
                    if (waitingMeals.Count > 0)
                    {
                        foreach (var item in waitingMeals)
                        {
                            var selectedMenus = menus.Where(x => x.OrderId == item.OrderId).ToList();
                            foreach (var selectedMenu in selectedMenus)
                            {
                                selectedMenu.TotalOrder = item.QuantityOrder;
                            }

                            item.ListMenu = selectedMenus;
                        } 
                    }
                }
            }

            return waitingMeals;
        }

        /// <summary>
        /// Get Order Details.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>Order Entity.</returns>
        public Order GetOrderDetails(ICriteria criteria)
        {
            var orderDetails = new Order();
            var menus = new List<OrderMenu>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    orderDetails = multi.Read<Order>().SingleOrDefault();
                    menus = multi.Read<OrderMenu>().ToList();
                    if (orderDetails != null)
                    {
                        orderDetails.ListMenu = menus;
                        if (orderDetails.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                            || orderDetails.CurrentStatusId == (int)OrderStatus.ReducedOrder)
                        {
                            bool isEdited = menus.Any(x => x.IsDeleted);
                            orderDetails.IsEdited = isEdited;
                        }
                    }
                }
            }

            return orderDetails;
        }

        /// <summary>
        /// Get Meal Order By Id.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>Order Entity.</returns>
        public Order GetMealOrderById(ICriteria criteria)
        {
            var mealOrder = new Order();
            var menus = new List<OrderMenu>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    mealOrder = multi.Read<Order>().SingleOrDefault();
                    menus = multi.Read<OrderMenu>().ToList();
                    if (mealOrder != null)
                    {
                        mealOrder.ListMenu = menus;
                        foreach (var item in menus)
                        {
                            item.TotalOrder = mealOrder.QuantityOrder;
                        }
                    }
                }
            }

            return mealOrder;
        }

        /// <summary>
        /// Check Employee belong to a group.
        /// </summary>
        /// <param name="criteria">Criteria to query database.</param>
        /// <returns>Employees and group belonged to.</returns>
        public EmployeeInGroup GetEmployeeInGroup(ICriteria criteria)
        {
            var employeeInGroup = new EmployeeInGroup();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    employeeInGroup.GroupOrder = multi.Read<GroupOrder>().ToList();
                    employeeInGroup.Employees = multi.Read<EmployeeInfo>().ToList();
                }
            }

            return employeeInGroup;
        }
    }
}
