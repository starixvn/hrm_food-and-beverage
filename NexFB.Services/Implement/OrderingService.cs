// --------------------------------------------------------------------------
// <copyright file="OrderingService.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface;

    /// <summary>
    /// Order Service.
    /// </summary>
    public class OrderingService : BaseService, IOrderingService
    {
        private readonly IOrderingRepository _orderRepository;

        public OrderingService()
            : this(null)
        {
        }

        public OrderingService(IOrderingRepository orderRepository)
        {
            _orderRepository = orderRepository ?? new OrderingRepository();
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

        public MealOrderingModel GetMyOrderById(GetMyOrderByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var order = new MealOrderingModel();
            if (criteria.IsValid())
            {
                order = _orderRepository.GetMyOrderById(criteria);
            }

            return order;
        }

        /// <summary>
        /// Function to get members for new order.
        /// </summary>
        /// <param name="criteria">GetMemberNewOrder Criteria.</param>
        /// <returns>MealOrdering Model.</returns>
        public MealOrderingModel GetMemberNewOrder(GetMemberNewOrderCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var order = new MealOrderingModel();
            if (criteria.IsValid())
            {
                order = _orderRepository.GetMemberNewOrder(criteria);
                if (order != null)
                {
                    order.IsSavedOrder = false;
                }
            }

            return order;
        }

        /// <summary>
        /// Function to get members for saved order.
        /// </summary>
        /// <param name="criteria">GetMemberSavedOrder Criteria.</param>
        /// <returns>MealOrdering Model.</returns>
        public MealOrderingModel GetMemberSavedOrder(GetMemberSavedOrderCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var order = new MealOrderingModel();
            if (criteria.IsValid())
            {
                order = _orderRepository.GetMemberSavedOrder(criteria);
                if (order != null)
                {
                    order.IsSavedOrder = true;
                }
            }

            return order;
        }

        /// <summary>
        /// Function to get order by order id.
        /// </summary>
        /// <param name="criteria">GetOrderById Criteria.</param>
        /// <returns>MealOrdering Model.</returns>
        public MealOrderingModel GetOrderById(GetOrderByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            MealOrderingModel order = null;
            if (criteria.IsValid())
            {
                order = _orderRepository.GetOrderById(criteria);
                if (order != null)
                {
                    order.IsSavedOrder = false;
                }
            }

            return order;
        }

        /// <summary>
        /// Function to insert a order.
        /// </summary>
        /// <param name="criteria">Insert Order Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult InserOrder(InsertOrderCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;
            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria, hasGetOutputParams: true);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Function to update a order.
        /// </summary>
        /// <param name="criteria">Update Order Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateOrder(UpdateOrderCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Function to update a order.
        /// </summary>
        /// <param name="criteria">Update Order Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult DeleteOrder(DeleteOrderCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;
            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }
    }
}
