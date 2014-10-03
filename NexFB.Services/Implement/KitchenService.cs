// --------------------------------------------------------------------------
// <copyright file="KitchenService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// Kitchen Service.
    /// </summary>
    public class KitchenService : BaseService, IKitchenService
    {
        private IKitchenRepository _kitchenRepository;

        public KitchenService()
            : this(null)
        {
        }

        public KitchenService(IKitchenRepository kitchenRepository)
        {
            _kitchenRepository = kitchenRepository ?? new KitchenRepository();
        }

        public List<Order> GetWaitngMealOrder(GetWaitingMealOrderCriteria criteria)
        {
            List<Order> waitingMeals = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                waitingMeals = this._kitchenRepository.GetMealOrder(criteria);
            }

            return waitingMeals;
        }

        public List<Order> GetAwaitingDeliverMealOrder(GetAwaitingDeliverMealOrderCriteria criteria)
        {
            List<Order> awaitingDeliverMeals = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                awaitingDeliverMeals = this._kitchenRepository.GetMealOrder(criteria);
            }

            return awaitingDeliverMeals;
        }

        public List<Order> GetDeliveredMealOrder(GetDeliveredMealOrderCriteria criteria)
        {
            List<Order> waitingMeals = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                waitingMeals = this._kitchenRepository.GetMealOrder(criteria);
            }

            return waitingMeals;
        }

        public CommandResult ReceiveOrder(ProcessReceiveOrderCriteria criteria)
        {
            CommandResult received = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                received = Repository.Update(criteria);
            }

            return received;
        }

        public CommandResult DeliverOrder(ProcessDeliverOrderCriteria criteria)
        {
            CommandResult received = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                received = Repository.Update(criteria);
            }

            return received;
        }

        public CommandResult UndoOrder(UndoOrderCriteria criteria)
        {
            CommandResult result = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        public Order GetOrderDetails(GetOrderDetailsCriteria criteria)
        {
            Order orderDetails = null;
            if (criteria.IsValid())
            {
                orderDetails = this._kitchenRepository.GetOrderDetails(criteria);
            }

            return orderDetails;
        }

        public Order GetMealOrderById(GetMealOrderByIdCriteria criteria)
        {
            Order orderDetails = null;
            if (criteria.IsValid())
            {
                orderDetails = this._kitchenRepository.GetMealOrderById(criteria);
            }

            return orderDetails;
        }

        public List<GroupOrder> GetAllGroupOrder(GetGroupNameCriteria criteria)
        {
            List<GroupOrder> groups = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                groups = this.GetData<GroupOrder>(criteria).ToList();
            }

            return groups;
        }

        public List<FoodOrder> GetAllFoodOrder(GetFoodNameCriteria criteria)
        {
            List<FoodOrder> foods = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                foods = this.GetData<FoodOrder>(criteria).ToList();
            }

            return foods;
        }

        public List<EmployeeInfo> GetUserByGroupId(GetUserByGroupIdCriteria criteria)
        {
            List<EmployeeInfo> users = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                users = this.GetData<EmployeeInfo>(criteria).ToList();
            }

            return users;
        }

        public List<EmployeeInfo> GetEmployeeOrderInfo(GetEmployeeByCodeCriteria criteria)
        {
            List<EmployeeInfo> employees = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employees = this.GetData<EmployeeInfo>(criteria).ToList();
            }

            return employees;
        }

        public EmployeeInGroup GetEmployeeInGroup(GetEmployeeInGroupBySysIdCriteria criteria)
        {
            EmployeeInGroup employeeInGroup = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employeeInGroup = _kitchenRepository.GetEmployeeInGroup(criteria);
            }

            return employeeInGroup;
        }

        public EmployeeInfo GetEmployeeBySysId(GetEmployeeBySysIdCriteria criteria)
        {
            EmployeeInfo employee = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employee = this.GetDataEntity<EmployeeInfo>(criteria);
            }

            return employee;
        }
    }
}
