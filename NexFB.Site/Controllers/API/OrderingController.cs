// --------------------------------------------------------------------------
// <copyright file="OrderingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    /// <summary>
    /// Ordering Controller.
    /// </summary>
    public class OrderingController : BaseController
    {
        /// <summary>
        /// Function to get my orders.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet]
        public IList<MealOrderingModel> GetMyOrder()
        {
            IList<MealOrderingModel> ordering = new List<MealOrderingModel>();
            MealOrderingModel order = new MealOrderingModel();
            OrderMemberModel member = new OrderMemberModel()
            {
                Member = new Employee()
                {
                   UserId = this.CurrentUser.UserId,
                   NickName = this.CurrentUser.NickName,
                   FullName = this.CurrentUser.FullName
                },
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch 3"
                }
            };
            OrderMemberModel member2 = new OrderMemberModel()
            {
                Member = new Employee()
                { 
                    UserId = 1, NickName = "Bradley.Mac", FullName = "Mạc Đức Trọng",
                },
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch 2"
                }
            };
            order.CreatorOrder = new Employee() 
            { 
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName
            };

            order.Members = new List<OrderMemberModel>();
            order.Remark = "Test 11";
            order.Location = new Location() 
            { 
                LocationId = 11,
                Name = "AAAAAA"
            };
            order.Phone = "0922121";
            order.Members.Add(member);
            order.Members.Add(member2);

            var selfOrder = order.Members.Where(t => t.Member.UserId == this.CurrentUser.UserId);
            if (selfOrder != null && selfOrder.Any())
            {
                order.MyMeal = selfOrder.SingleOrDefault().Menu;
            }
            else
            {
                order.MyMeal = new Menu()
                {
                    Name = "N/A"
                };
            }

            order.Status = new Status()
            {
                StatusId = 1,
                Name = "Preparing"
            };

            ordering.Add(order);

            return ordering;
        }

         /// <summary>
        /// Function to get menu.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public Menu GetMenu()
        {
            var menu = new Menu() 
            {
                MenuId = 1,
                Name = "Lunch 2"
            };

            var food1 = new Food()
            {
                FoodId = 1,
                Name = "Canh Củ Sen"
            };

            var food2 = new Food()
            {
                FoodId = 2,
                Name = "Chả Cá Chiên"
            };

            var food3 = new Food()
            {
                FoodId = 3,
                Name = "Thịt Kho Trứng"
            };

            var food4 = new Food()
            {
                FoodId = 4,
                Name = "Bí Xanh Xào Thịt Xay"
            };

            var food5 = new Food()
            {
                FoodId = 5,
                Name = "Cải Ngồng Luộc "
            };

            menu.FoodItems = new List<Food>();
            menu.FoodItems.Add(food1);
            menu.FoodItems.Add(food2);
            menu.FoodItems.Add(food3);
            menu.FoodItems.Add(food4);
            menu.FoodItems.Add(food5);

            return menu;
        }

        /// <summary>
        /// Add new order.
        /// </summary>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        public MealOrderingModel GetAddNew()
        {
            MealOrderingModel order = new MealOrderingModel();

            OrderMemberModel member = new OrderMemberModel()
            {
                Member = new Employee()
                {
                    UserId = this.CurrentUser.UserId,
                    NickName = this.CurrentUser.NickName,
                    FullName = this.CurrentUser.FullName
                },
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch"
                }
            };

            order.Members = new List<OrderMemberModel>();
            order.Remark = "Test";
            order.Location = new Location()
            {
                LocationId = 15,
                Name = "AAAAAA"
            };
            order.Phone = "092212";
            order.Members.Add(member);

            return order;
        }

        /// <summary>
        /// Function to get saved order.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public MealOrderingModel GetSavedOrder()
        {
            MealOrderingModel order = new MealOrderingModel();

            OrderMemberModel member = new OrderMemberModel()
            {
                Member = new Employee()
                {
                    UserId = this.CurrentUser.UserId,
                    NickName = this.CurrentUser.NickName,
                    FullName = this.CurrentUser.FullName
                },
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch 3"
                }
            };

            OrderMemberModel member2 = new OrderMemberModel()
            {
                Member = new Employee()
                {
                    UserId = 1,
                    NickName = "Bradley.Mac",
                    FullName = "Mạc Đức Trọng",
                },
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch 2"
                }
            };

            order.Members = new List<OrderMemberModel>();
            order.Remark = "Test 11";
            order.Location = new Location()
            {
                LocationId = 11,
                Name = "AAAAAA"
            };
            order.Phone = "0922121";
            order.Members.Add(member);
            order.Members.Add(member2);

            return order;
        }
    }
}