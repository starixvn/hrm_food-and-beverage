// --------------------------------------------------------------------------
// <copyright file="OrderService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// Order Service.
    /// </summary>
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService()
            : this(null)
        {
        }

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? new OrderRepository();
        }

        public Menu GetMenuByUserId(GetMenuByUserIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            Menu menu = null;
            if (criteria.IsValid())
            {
                menu = _orderRepository.GetMenuByUserId(criteria);
            }

            return menu;
        }

        public IList<MealOrderingModel> GetMyOrderByCurrentDay(GetMyOrderInCurrentDayCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MealOrderingModel> orders = null;
            if (criteria.IsValid())
            {
                orders = _orderRepository.GetMyOrderByCurrentDay(criteria);
            }

            return orders;
        }
    }
}
