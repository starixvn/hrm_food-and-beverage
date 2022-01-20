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

    public class OrderingRepository : IOrderingRepository
    {
        public Menu GetMenuByUserId(ICriteria criteria)
        {
            Menu menu = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    menu = multi.Read<Menu>().FirstOrDefault();
                    if (menu != null)
                    {
                        var foods = multi.Read<Food>().ToList();
                        if (foods != null)
                        {
                            menu.FoodItems = foods.ToList();
                        }
                    }
                }
            }

            return menu;
        }

        public IList<MealOrderingModel> GetMyOrderByCurrentDay(ICriteria criteria, int userId)
        {
            IList<MealOrderingModel> orders = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var data = multi.Read<MealOrderingModel, Employee, Status, Location, MealOrderingModel>(
                        (od, e, s, l) => 
                    {
                        od.CreatorOrder = e;
                        od.Status = s;
                        od.Location = l;
                        return od;
                    },
                    "OrderBy, CurrentStatusID, MealENGName, LocationID");

                    if (data != null)
                    {
                        orders = CreateMealOrdering(multi, data, userId);
                    }
                }
            }

            return orders;
        }

        public MealOrderingModel GetOrderById(ICriteria criteria)
        {
            MealOrderingModel order = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var orders = multi.Read<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                        (od, e, s, m, l) =>
                        {
                            od.CreatorOrder = e;
                            od.Status = s;
                            od.Location = l;
                            return od;
                        },
                    "OrderBy, CurrentStatusID, MealENGName, LocationID").ToList();

                    if (orders != null)
                    {
                        order = orders.SingleOrDefault();

                        var ordersQuatity = multi.Read<MenuQuantityModel, dynamic, MenuQuantityModel>(
                        (q, m) =>
                        {
                            q.Menu = new Menu
                            {
                                NameEN = m.MealENGName,
                                NameVN = m.MealVNMName
                            };
                            return q;
                        },
                        "MealENGName");

                        if (ordersQuatity != null)
                        {
                            foreach (MenuQuantityModel menu in ordersQuatity)
                            {
                                order.MenuQuantity.Add(menu);
                            }
                        }

                        var members = multi.Read<OrderMemberModel, dynamic, OrderMemberModel>(
                            (mb, d) =>
                            {
                                return GetMember(mb, d);
                            },
                            "SysID");

                        members = GroupMenusOfMember(members, orders);
                        order.Members = members.ToList();
                        order.IsSideDishOrder = CheckOrderHasSideDish(order.Members);

                        IEnumerable<OrderMemberModel> creator = members.Where(x => x.Member.UserId == order.CreatorOrder.UserId);

                        if (creator != null && creator.Count() > 0)
                        {
                            order.CurrentMenus = creator.First().Menus;
                        }
                    }
                }
            }

            return order;
        }

        public MealOrderingModel GetMemberNewOrder(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    return multi.Read<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                        (od, e, s, m, l) =>
                        {
                            OrderMemberModel memberOrder = new OrderMemberModel
                            {
                                IsOrder = s == null,
                                Member = e,
                                CurrentStatus = s,
                                IsAllowOrder = od.RawIsAllowOrder
                            };

                            memberOrder.Menus.Add(m);
                            od.Members = new List<OrderMemberModel>();
                            od.Members.Add(memberOrder);

                            od.CreatorOrder = e;
                            od.CurrentMenus.Add(m);
                            od.Status = s;
                            od.Location = l;
                            return od;
                        },
                        "SysId, CurrentStatusID, MenuID, LocationID").FirstOrDefault();
                }
            }
        }

        public MealOrderingModel GetMemberSavedOrder(ICriteria criteria)
        {
            MealOrderingModel order = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.Query<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                    (od, e, s, m, l) =>
                    {
                        if (order == null)
                        {
                            order = od;
                            od.CreatorOrder = e;
                            od.Status = s;
                            od.Location = l;
                            od.Members = new List<OrderMemberModel>();
                        }

                        order.CurrentMenus.Add(m);
                        var memberOrder = new OrderMemberModel
                        {
                            IsOrder = od.RawIsAllowOrder ? od.IsOrder : false,
                            Member = e,
                            CurrentStatus = s,
                            IsAllowOrder = od.RawIsAllowOrder
                        };

                        if (m != null)
                        {
                            m.IsMainMenu = true;
                            memberOrder.Menus.Add(m);
                        }

                        order.Members.Add(memberOrder);
                        return od;
                    },
                    "SysId, CurrentStatusID, MenuID, LocationID",
                    criteria.GetSpParams());
            }

            return order;
        }

        private OrderMemberModel GetMember(OrderMemberModel orderMember, dynamic dynamicObject)
        {
            var member = new Employee
            {
                UserId = dynamicObject.SysID,
                NickName = dynamicObject.NickName,
                FullName = dynamicObject.FullName
            };

            Menu menu = GetMenuOfMember(dynamicObject);
            Status currentStatus = GetStatusOfMember(dynamicObject);

            orderMember.IsOrder = true;
            orderMember.Member = member;
            orderMember.Menus.Add(menu);
            orderMember.CurrentStatus = currentStatus;

            return orderMember;
        }

        private Status GetStatusOfMember(dynamic dynamicObject)
        {
            var status = new Status
            {
                StatusId = dynamicObject.CurrentStatusID,
                Name = dynamicObject.CurrentStatusName
            };

            return status;
        }

        private IList<MealOrderingModel> CreateMealOrdering(
            GridReader multipleData,
            IEnumerable<MealOrderingModel> data,
            int userId)
        {
            var orders = new List<MealOrderingModel>();
            var ordersQuantity = multipleData.Read<MenuQuantityModel, dynamic, MenuQuantityModel>(
            (q, m) =>
            {
                q.Menu = new Menu
                {
                    NameEN = m.MealENGName,
                    NameVN = m.MealVNMName
                };
                return q;
            },
            "MealENGName");

            var members = multipleData.Read<OrderMemberModel, dynamic, OrderMemberModel>(
            (mb, d) =>
            {
                return GetMember(mb, d);
            },
            "SysID");

            members = GroupMenusOfMember(members, data.ToList());

            if (ordersQuantity != null)
            {
                foreach (MealOrderingModel order in data)
                {
                    foreach (MenuQuantityModel menu in ordersQuantity)
                    {
                        if (order.OrderId == menu.OrderId)
                        {
                            order.MenuQuantity = order.MenuQuantity ?? new List<MenuQuantityModel>();

                            order.MenuQuantity.Add(menu);
                        }
                    }

                    IEnumerable<OrderMemberModel> membersOfCurrentOrder = members.Where(x => x.OrderId == order.OrderId);

                    foreach (OrderMemberModel member in membersOfCurrentOrder)
                    {
                        order.Members = order.Members ?? new List<OrderMemberModel>();

                        order.Members.Add(member);

                        if (userId == member.Member.UserId)
                        {
                            order.CurrentMenus = member.Menus;
                        }
                    }

                    orders.Add(order);
                }
            }

            return orders;
        }

        private IList<OrderMemberModel> GroupMenusOfMember(
            IEnumerable<OrderMemberModel> members,
            IList<MealOrderingModel> orders)
        {
            IList<OrderMemberModel> newMembers = new List<OrderMemberModel>();

            foreach (MealOrderingModel order in orders)
            {
                IEnumerable<int> distinctUserIds = members.Select(x => x.Member.UserId).Distinct();

                foreach (int userId in distinctUserIds)
                {
                    IEnumerable<OrderMemberModel> rowsSameId = members.Where(x => x.Member.UserId == userId && x.OrderId == order.OrderId);

                    if (rowsSameId != null && rowsSameId.Count() > 0)
                    {
                        var groupMember = new OrderMemberModel
                        {
                            OrderId = rowsSameId.First().OrderId,
                            Member = rowsSameId.First().Member,
                            IsOrder = rowsSameId.First().IsOrder,
                            CurrentStatus = rowsSameId.First().CurrentStatus,
                        };

                        foreach (OrderMemberModel row in rowsSameId)
                        {
                            groupMember.Menus.Add(row.Menus.First());
                        }

                        newMembers.Add(groupMember);
                    }
                }
            }

            return newMembers;
        }

        private Menu GetMenuOfMember(dynamic dynamicObject)
        {
            var menu = new Menu()
            {
                MenuId = dynamicObject.MenuID,
                NameEN = dynamicObject.MenuEngName,
                NameVN = dynamicObject.MenuVNMName,
                IsHaveAlternativeMenu = GetBoolValue(dynamicObject.IsHaveAltMenu),
                IsMainMenu = GetBoolValue(dynamicObject.IsMainMenu),
                IsSideDish = GetBoolValue(dynamicObject.IsSideDish),
                QuantityOrder = GetIntValue(dynamicObject.QtyOrder),
                Price = GetDoubleValue(dynamicObject.Price)
            };

            return menu;
        }

        private bool CheckOrderHasSideDish(IEnumerable<OrderMemberModel> members)
        {
            bool hasSideDish = false;

            foreach (OrderMemberModel member in members)
            {
                foreach (Menu menu in member.Menus)
                {
                    if (menu.IsSideDish)
                    {
                        hasSideDish = true;
                    }
                }
            }

            return hasSideDish;
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

        private int GetIntValue(dynamic value)
        {
            int result = 0;

            if (value != null)
            {
                result = value;
            }

            return result;
        }

        private decimal GetDoubleValue(dynamic value) 
        {
            decimal result = 0;

            if (value != null)
            {
                result = value;
            }

            return result;
        }
    }
}