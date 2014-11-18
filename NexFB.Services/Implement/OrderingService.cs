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
    using NexFB.Core.Translator;
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
                orders = _orderRepository.GetMyOrderByCurrentDay(criteria, criteria.UserId);
            }

            return orders;
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
            }

            if (order == null)
            {
                order = new MealOrderingModel();
            }

            if (order.Location == null)
            {
                order.Location = new Location();
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
            }

            if (order == null)
            {
                order = new MealOrderingModel();
            }

            if (order.Location == null)
            {
                order.Location = new Location();
            }

            order.IsSavedOrder = true;
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

        /// <summary>
        /// Function to add a member to group.
        /// </summary>
        /// <param name="criteria">AddMember Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult AddMember(AddMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;
            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria);
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
        /// Function to remove a member from group.
        /// </summary>
        /// <param name="criteria">RemoveMember Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult RemoveMember(RemoveMemberCriteria criteria)
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

        /// <summary>
        /// Get all menus by user Id include main menu and alternative menus.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Menu model.</returns>
        public IEnumerable<Menu> GetAlternativeMenuByUserId(GetAlternativeMenuByUserIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Menu> allMenus = null;

            if (criteria.IsValid())
            {
                allMenus = GetData<Menu>(criteria);
            }

            return allMenus;
        }

        /// <summary>
        /// Insert order side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertOrderSideDish(InsertOrderSideDishCriteria criteria)
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
        /// Update order side dish.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateOrderSideDish(UpdateOrderSideDishCriteria criteria)
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
        /// Get last modified date order Id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The order contains last modified date.</returns>
        public MealOrderingModel GetLastModifiedDateByOrderId(GetLastMofifiedDateByOrderIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var order = new MealOrderingModel();

            if (criteria.IsValid())
            {
                order = GetDataEntity<MealOrderingModel>(criteria);
            }

            return order;
        }

        /// <summary>
        /// Translate remark into vietnamese.
        /// </summary>
        /// <param name="remark">The remark of order.</param>
        /// <returns>The translated text.</returns>
        public string TranslateRemark(string remark)
        {
            string translatedText = string.Empty;

            if (!string.IsNullOrEmpty(remark))
            {
                translatedText = FBTranslator.AutoTranslation(remark);
            }

            return translatedText;
        }
    }
}
