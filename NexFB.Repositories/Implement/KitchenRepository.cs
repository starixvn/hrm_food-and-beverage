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
            List<Order> waitingMeals = new List<Order>();
            List<Menu> menus = new List<Menu>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    waitingMeals = multi.Read<Order>().ToList();
                    menus = multi.Read<Menu>().ToList();
                    if (waitingMeals.Count > 0)
                    {
                        foreach (var item in waitingMeals)
                        {
                            var selectedMenus = menus.Where(x => x.OrderId == item.OrderId).ToList();
                            foreach (var selectedMenu in selectedMenus)
                            {
                                selectedMenu.TotalOrder = item.QtyOrd;
                            }

                            item.ListMenu = selectedMenus;
                        } 
                    }
                }
            }

            return waitingMeals;
        }
    }
}
