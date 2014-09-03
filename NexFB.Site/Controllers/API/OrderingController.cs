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
    using System.Web.Http;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    /// <summary>
    /// Ordering Controller.
    /// </summary>
    public class OrderingController : BaseController
    {
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
                UserId = this.CurrentUser.UserId,
                NickName = this.CurrentUser.NickName,
                FullName = this.CurrentUser.FullName,
                IsOrder = true,
                Menu = new Menu()
                {
                    MenuId = 1,
                    Name = "Lunch"
                }
            };

            order.Members = new List<OrderMemberModel>();
            order.Remark = "Test";
            order.LocationId = 10;
            order.Phone = "092212";
            order.Members.Add(member);

            return order;
        }
    }
}