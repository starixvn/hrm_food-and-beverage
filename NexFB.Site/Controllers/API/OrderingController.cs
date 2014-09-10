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
    using System.Web.Http;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Ordering Controller.
    /// </summary>
    public class OrderingController : BaseController
    {
        private readonly IOrderingService _orderService;

        public OrderingController()
            : this(null)
        {
        }

        public OrderingController(IOrderingService orderService)
        {
            _orderService = orderService ?? new OrderingService();
        }

        /// <summary>
        /// Function to get my orders.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public IList<MealOrderingModel> GetMyOrder()
        {
            var criteria = new GetMyOrderInCurrentDayCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            IList<MealOrderingModel> orders = _orderService.GetMyOrderByCurrentDay(criteria);
            return orders;
        }

         /// <summary>
        /// Function to get menu.
        /// </summary>
        /// <returns>MealOrdering Model.</returns>
        [HttpGet]
        public Menu GetMenu()
        {
            var criteria = new GetMenuByUserIdCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            Menu menu = _orderService.GetMenuByUserId(criteria);
            return menu;
        }

        /// <summary>
        /// Add new order.
        /// </summary>
        /// <returns>The model meal ordering.</returns>
        [HttpGet]
        public MealOrderingModel GetAddNew()
        {
            var criteria = new GetMemberNewOrderCriteria()
            {
                UserId = this.CurrentUser.UserId
            };

            MealOrderingModel order = _orderService.GetMemberNewOrder(criteria);
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
                    NameEN = "Lunch 3"
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
                    NameEN = "Lunch 2"
                }
            };

            order.Members = new List<OrderMemberModel>();
            order.Remark = "Test 11";
            order.Location = new Location()
            {
                LocationId = 11,
                NameEN = "AAAAAA"
            };
            order.Phone = "0922121";
            order.Members.Add(member);
            order.Members.Add(member2);

            return order;
        }
    }
}