// -----------------------------------------------------------------------
// <copyright file="AdminOrderHistoryRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 12:40:00 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.Ordering;
    using NexFB.Models.Report.AdminOrderHistoryReport;
    using NexFB.Repositories.Interface.Report;

    /// <summary>
    /// Admin order history repository.
    /// </summary>
    public class AdminOrderHistoryRepository : IAdminOrderHistoryRepository
    {
        /// <summary>
        /// Get admin order history report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The admin order history report.</returns>
        public AdminOrderHistoryReport GetAdminOrderHistoryReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var report = new AdminOrderHistoryReport();

                    report.EmployeeOrders = multi.Read<AdminOrderModel, MealModel, MealOrderingModel, Status, Employee, AdminOrderModel>(
                        (empOrder, me, order, stt, orderBy) =>
                        {
                            empOrder.Meal = me;
                            empOrder.Order = order;
                            empOrder.Status = stt;
                            empOrder.OrderBy = orderBy;
                            return empOrder;
                        },
                        "MealID,OrderID,CurrentStatusID,OrderBy").OrderBy(p => p.OrderTime).ToList();
                    
                    var allMenus = multi.Read<dynamic>();
                    foreach (var employeeOrder in report.EmployeeOrders)
                    {
                        var menusOfOrder = allMenus.Where(x => x.OrderID == employeeOrder.Order.OrderId);
                        employeeOrder.Menus = CreateMenuOrder(menusOfOrder);
                    }

                    return report;
                }
            }
        }

        /// <summary>
        /// Create menu order.
        /// </summary>
        /// <param name="menusOfOrder">The raw menus of order.</param>
        /// <returns>The list menu of current order.</returns>
        private List<Menu> CreateMenuOrder(IEnumerable<dynamic> menusOfOrder)
        {
            var menus = new List<Menu>();

            foreach (dynamic rawMenu in menusOfOrder)
            {
                var menu = new Menu
                {
                    NameVN = rawMenu.MealVNMName,
                    NameEN = rawMenu.MealENGName,
                    IsMainMenu = GetBoolValue(rawMenu.IsMainMenu),
                    IsSideDish = GetBoolValue(rawMenu.isSideDish),
                    QuantityOrder = rawMenu.QtyOrder,
                    Price = rawMenu.Price
                };

                menus.Add(menu);
            }

            return menus;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>True or False.</returns>
        private bool GetBoolValue(dynamic value)
        {
            return value == 1;
        }
    }
}