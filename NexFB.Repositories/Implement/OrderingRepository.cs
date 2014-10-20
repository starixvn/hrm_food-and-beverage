// --------------------------------------------------------------------------
// <copyright file="OrderingRepository.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Order Repository.
    /// </summary>
    public class OrderingRepository : IOrderingRepository
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
                        od.Status = s;
                        od.Location = l;
                        return od;
                    },
                    "OrderBy, CurrentStatusID, MealENGName, LocationID");

                    if (data != null)
                    {
                        orders = CreateMealOrdering(orders, multi, data);
                    }
                }
            }

            return orders;
        }

        /// <summary>
        /// Function order by order id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>MealOrdering Model.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public MealOrderingModel GetOrderById(ICriteria criteria)
        {
            MealOrderingModel order = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    order = multi.Read<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                        (od, e, s, m, l) =>
                        {
                            od.CreatorOrder = e;
                            od.Status = s;
                            od.Location = l;
                            return od;
                        },
                    "OrderBy, CurrentStatusID, MealENGName, LocationID").SingleOrDefault();

                    if (order != null)
                    {
                        var ordersQuatity = multi.Read<MenuQuantityModel, dynamic, MenuQuantityModel>(
                        (q, m) =>
                        {
                            q.Menu = new Menu()
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
                                if (order.MenuQuantity == null)
                                {
                                    order.MenuQuantity = new List<MenuQuantityModel>();
                                }

                                order.MenuQuantity.Add(menu);
                            }
                        }

                        var members = multi.Read<OrderMemberModel, dynamic, OrderMemberModel>(
                            (mb, d) =>
                            {
                                return GetMember(mb, d);
                            },
                            "SysID");

                        order.Members = members.ToList();
                        order.MyMeal = members != null && members.Count() > 0 ? members.FirstOrDefault().Menu : null;
                    }
                }
            }

            return order;
        }

        /// <summary>
        /// Function to get member for new order.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>MealOrdering Model.</returns>
        public MealOrderingModel GetMemberNewOrder(ICriteria criteria)
        {
            var order = new MealOrderingModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    order = multi.Read<MealOrderingModel, Employee, Status, Menu, Location, MealOrderingModel>(
                        (od, e, s, m, l) =>
                        {
                            OrderMemberModel memberOrder = new OrderMemberModel()
                            {
                                IsOrder = s == null,
                                Member = e,
                                Menu = m,
                                CurrentStatus = s
                            };

                            od.Members = new List<OrderMemberModel>();
                            od.Members.Add(memberOrder);

                            od.CreatorOrder = e;
                            od.MyMeal = m;
                            od.Status = s;
                            od.Location = l;
                            return od;
                        },
                        "SysId, CurrentStatusID, MenuID, LocationID").FirstOrDefault();
                }
            }

            return order;
        }

        /// <summary>
        /// Function to get member for new order.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>MealOrdering Model.</returns>
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
                            od.MyMeal = m;
                            od.Status = s;
                            od.Location = l;
                            od.Members = new List<OrderMemberModel>();
                        }

                        var memberOrder = new OrderMemberModel()
                        {
                            IsOrder = s == null,
                            Member = e,
                            Menu = m,
                            CurrentStatus = s
                        };

                        if (memberOrder.Menu != null)
                        {
                            memberOrder.Menu.IsMainMenu = true;
                        }

                        order.Members.Add(memberOrder);
                        return od;
                    },
                    "SysId, CurrentStatusID, MenuID, LocationID",
                    criteria.GetSpParams());
            }

            return order;
        }

        /// <summary>
        /// Function to get member from dynamic object.
        /// </summary>
        /// <param name="orderMember">Order Member Model.</param>
        /// <param name="dynamicObject">Dynamic Object.</param>
        /// <returns>OrderMember Model.</returns>
        private OrderMemberModel GetMember(OrderMemberModel orderMember, dynamic dynamicObject)
        {
            var member = new Employee()
            {
                UserId = dynamicObject.SysID,
                NickName = dynamicObject.NickName,
                FullName = dynamicObject.FullName
            };

            var menu = new Menu()
            {
                MenuId = dynamicObject.MenuID,
                NameEN = dynamicObject.MenuEngName,
                NameVN = dynamicObject.MenuVNMName,
                IsHaveAlternativeMenu = GetBoolValue(dynamicObject.IsHaveAltMenu),
                IsMainMenu = GetBoolValue(dynamicObject.IsMainMenu),
                IsSideDish = GetBoolValue(dynamicObject.IsSideDish),
                QuantityOrder = GetIntValue(dynamicObject.QtyOrder)
            };

            orderMember.IsOrder = true;
            orderMember.Member = member;
            orderMember.Menu = menu;

            return orderMember;
        }

        /// <summary>
        /// Create meal ordering.
        /// </summary>
        /// <param name="orders">The list order.</param>
        /// <param name="multipleData">The grid data.</param>
        /// <param name="data">The data from database.</param>
        /// <returns>The list Meal Ordering model.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IList<MealOrderingModel> CreateMealOrdering(
            IList<MealOrderingModel> orders,
            GridReader multipleData,
            IEnumerable<MealOrderingModel> data)
        {
            orders = new List<MealOrderingModel>();
            var ordersQuantity = multipleData.Read<MenuQuantityModel, dynamic, MenuQuantityModel>(
            (q, m) =>
            {
                q.Menu = new Menu()
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

            members = GroupMembers(members);

            if (ordersQuantity != null)
            {
                foreach (MealOrderingModel order in data)
                {
                    foreach (MenuQuantityModel menu in ordersQuantity)
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

                    IEnumerable<OrderMemberModel> membersOfCurrentOrder = members.Where(x => x.OrderId == order.OrderId);

                    foreach (OrderMemberModel member in membersOfCurrentOrder)
                    {
                        if (order.Members == null)
                        {
                            order.Members = new List<OrderMemberModel>();
                        }

                        order.Members.Add(member);

                        if (order.CreatorOrder.UserId == member.Member.UserId)
                        {
                            order.MyMeal = member.Menu;
                        }
                    }

                    orders.Add(order);
                }
            }

            return orders;
        }

        /// <summary>
        /// Group side dish order of same member.
        /// </summary>
        /// <param name="members">The enumerator members in order.</param>
        /// <returns>The list member has group side dish.</returns>
        private IList<OrderMemberModel> GroupMembers(IEnumerable<OrderMemberModel> members)
        {
            IList<OrderMemberModel> newMembers = new List<OrderMemberModel>();
            IEnumerable<int> distinctUserIds = members.Select(x => x.Member.UserId).Distinct();

            foreach (int userId in distinctUserIds)
            {
                IEnumerable<OrderMemberModel> rowsSameId = members.Where(x => x.Member.UserId == userId);

                if (rowsSameId.Count() == 1 && !rowsSameId.First().Menu.IsSideDish)
                {
                    //// New Order or Favorite Order
                    newMembers.Add(rowsSameId.First());
                }
                else if (rowsSameId.Count() > 1 || (rowsSameId.Count() == 1 && rowsSameId.First().Menu.IsSideDish))
                {
                    //// Side Dish Order
                    var groupMember = new OrderMemberModel
                    {
                        OrderId = rowsSameId.First().OrderId,
                        Member = rowsSameId.First().Member,
                        IsOrder = rowsSameId.First().IsOrder,
                        CurrentStatus = rowsSameId.First().CurrentStatus,
                        Menu = new Menu
                        {
                            FoodItems = new List<Food>()
                        }
                    };

                    foreach (OrderMemberModel rowSameId in rowsSameId)
                    {
                        var foodItem = new Food
                        {
                            FoodId = rowSameId.Menu.MenuId,
                            NameEN = rowSameId.Menu.NameEN,
                            NameVN = rowSameId.Menu.NameVN,
                            Quantity = rowSameId.Menu.QuantityOrder
                        };

                        groupMember.Menu.FoodItems.Add(foodItem);
                    }

                    newMembers.Add(groupMember);
                }
            }

            return newMembers;
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
    }
}
