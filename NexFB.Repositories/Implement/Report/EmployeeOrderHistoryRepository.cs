// ----------------------------------------------------------------------
// <copyright file="EmployeeOrderHistoryRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 2:12:52 PM">Created.</change>
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
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Repositories.Interface.Report;

    /// <summary>
    /// Employee order history repository.
    /// </summary>
    public class EmployeeOrderHistoryRepository : IEmployeeOrderHistoryRepository
    {
        /// <summary>
        /// Get employee order history report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The employee order history report.</returns>
        public EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var report = new EmployeeOrderHistoryReport();

                    report.EmployeeOrders = multi.Read<EmployeeOrderModel, MealModel, MealOrderingModel, Status, Employee, EmployeeOrderModel>(
                        (empOrder, me, order, stt, orderBy) =>
                        {
                            empOrder.Meal = me;
                            empOrder.Order = order;
                            empOrder.Status = stt;
                            empOrder.OrderBy = orderBy;
                            return empOrder;
                        },
                        "MealID,OrderID,CurrentStatusID,OrderBy").ToList();
                    IEnumerable<dynamic> allMenus = multi.Read<dynamic>();

                    foreach (EmployeeOrderModel employeeOrder in report.EmployeeOrders)
                    {
                        IEnumerable<dynamic> menusOfOrder = allMenus.Where(x => x.OrderID == employeeOrder.Order.OrderId);
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

        private bool GetBoolValue(dynamic value)
        {
            bool result = false;

            if (value == 1)
            {
                result = true;
            }

            return result;
        }
    }
}
