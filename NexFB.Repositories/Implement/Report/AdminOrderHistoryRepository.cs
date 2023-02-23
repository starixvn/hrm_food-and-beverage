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

    public class AdminOrderHistoryRepository : IAdminOrderHistoryRepository
    {
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
                        "MealID,OrderID,CurrentStatusID,OrderBy").ToList();

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

        private static bool GetBoolValue(dynamic value)
        {
            return value == 1;
        }
    }
}