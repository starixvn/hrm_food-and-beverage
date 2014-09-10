// --------------------------------------------------------------------------
// <copyright file="OrderRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Order Repository.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        /// <summary>
        /// Function to get menu in current meal by user id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Current Menu.</returns>
        public Menu GetMenuByUserId(ICriteria criteria)
        {
            Menu menu = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var data = multi.Read<Menu>();
                    if (data != null)
                    {
                        menu = data.FirstOrDefault();
                        var foods = multi.Read<Food>();
                        if (foods != null)
                        {
                            menu.FoodItems = foods.ToList();
                        }
                    }
                }
            }

            return menu;
        }

        /// <summary>
        /// Function to get my orders in current day.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of MealOrderingModel.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public IList<MealOrderingModel> GetMyOrderByCurrentDay(ICriteria criteria)
        {
            IList<MealOrderingModel> orders = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var data = multi.Read<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                        (od, e, s, m, l) => 
                    {
                        od.CreatorOrder = e;
                        od.MyMeal = m;
                        od.Status = s;
                        od.Location = l;
                        return od;
                    },
                    "OrderBy, CurrentStatusID, MealENGName, LocationID");

                    if (data != null)
                    {
                        orders = new List<MealOrderingModel>();
                        var ordersQuatity = multi.Read<MenuQuantityModel, Menu, MenuQuantityModel>(
                        (q, m) =>
                        {
                            q.Menu = m;
                            return q;
                        }, 
                        "MealENGName");

                        if (ordersQuatity != null)
                        {
                            foreach (MealOrderingModel order in data)
                            {
                                foreach (MenuQuantityModel menu in ordersQuatity)
                                {
                                    if (order.OrderId == menu.OrderId)
                                    {
                                        if (order.MenuQuantity == null)
                                        {
                                            order.MenuQuantity = new List<MenuQuantityModel>();
                                        }

                                        order.MenuQuantity.Add(menu);
                                    }
                                }

                                orders.Add(order);
                            }
                        }
                    }
                }
            }

            return orders;
        }
    }
}
