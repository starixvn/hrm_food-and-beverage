namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Core.Enums;
    using NexFB.Models.Kitchen;
    using NexFB.Repositories.Interface;

    public class KitchenRepository : IKitchenRepository
    {
        public List<Order> GetMealOrder(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var waitingMeals = multi.Read<Order>().ToList();
                    var menus = multi.Read<OrderMenu>().ToList();
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

                    return waitingMeals;
                }
            }
        }

        public Order GetOrderDetails(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var orderDetails = multi.Read<Order>().SingleOrDefault();
                    var menus  = multi.Read<OrderMenu>().ToList();
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

                    return orderDetails;
                }
            }
        }

        public IList<Order> GetOrderByQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var orders = multi.Read<Order>().ToList();
                    var menus = multi.Read<OrderMenu>().ToList();
                    foreach (Order item in orders)
                    {
                        var selectedMenus = menus.Where(x => x.OrderId == item.OrderId).ToList();

                        item.ListMenu = selectedMenus;
                        if (item.CurrentStatusId == (int)OrderStatus.ReceivedOrder
                            || item.CurrentStatusId == (int)OrderStatus.ReducedOrder)
                        {
                            bool isEdited = menus.Any(x => x.IsDeleted);
                            item.IsEdited = isEdited;
                        }
                    }

                    return orders;
                }
            }
        }

        public Order GetMealOrderById(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var mealOrder = multi.Read<Order>().SingleOrDefault();
                    var menus = multi.Read<OrderMenu>().ToList();
                    if (mealOrder != null)
                    {
                        mealOrder.ListMenu = menus;
                        foreach (var item in menus)
                        {
                            item.TotalOrder = mealOrder.QuantityOrder;
                        }
                    }

                    return mealOrder;
                }
            }
        }

        public EmployeeInGroup GetEmployeeInGroup(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    return new EmployeeInGroup
                    {
                        GroupOrder = multi.Read<GroupOrder>().ToList(),
                        Employees = multi.Read<EmployeeInfo>().ToList()
                    };
                }
            }
        }
    }
}