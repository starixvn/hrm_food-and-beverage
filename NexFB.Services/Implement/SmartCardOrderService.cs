// --------------------------------------------------------------------------
// <copyright file="SmartCardOrderService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Services.Interface;

    /// <summary>
    /// Smart Card Order Service.
    /// </summary>
    public class SmartCardOrderService : BaseService, ISmartCardOrderService
    {
        public EmployeeInfo GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria)
        {
            EmployeeInfo employeeInfo = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employeeInfo = this.GetDataEntity<EmployeeInfo>(criteria);
            }

            return employeeInfo;
        }

        public List<FoodOrder> GetAlternativeFoodBySysId(GetAlternativeFoodBySysIdCriteria criteria)
        {
            List<FoodOrder> alternativeFoods = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                alternativeFoods = this.GetData<FoodOrder>(criteria).ToList();
            }

            return alternativeFoods;
        }

        public List<FoodOrder> GetSideFoodBySysId(GetSideFoodBySysIdCriteria criteria)
        {
            List<FoodOrder> sideFoods = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                sideFoods = this.GetData<FoodOrder>(criteria).ToList();
            }

            return sideFoods;
        }

        /// <summary>
        /// Insert Order Food.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>Order Result.</returns>
        public OrderResult InsertOrderFood(InsertOrderMealByUserCriteria criteria)
        {
            OrderResult result = new OrderResult();
            result.CommandResult = new CommandResult();
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                result.CommandResult = this.Repository.Insert(criteria, hasGetOutputParams: true);
            }

            if (result.CommandResult.Code == 0)
            {
                result.OrderId = criteria.OrderId;
                result.OrderNo = criteria.OrderNo;
            }

            return result;
        }

        /// <summary>
        /// Insert Order Side Food.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>Order Result.</returns>
        public OrderResult InsertOrderSideFood(InsertOrderSideDishesByUserCriteria criteria)
        {
            OrderResult result = new OrderResult();
            result.CommandResult = new CommandResult();
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                result.CommandResult = this.Repository.Insert(criteria, hasGetOutputParams: true);
            }

            if (result.CommandResult.Code == 0)
            {
                result.OrderId = criteria.OrderId;
                result.OrderNo = criteria.OrderNo;
            }

            return result;
        }

        /// <summary>
        /// Get My Order By SysId.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>List employee orders.</returns>
        public List<SCEmployeeOrder> GetMyOrderBySysId(GetMyOrderBySysIdCriteria criteria)
        {
            List<SCEmployeeOrder> employeeOrders = null;
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                employeeOrders = this.GetData<SCEmployeeOrder>(criteria).ToList();
            }

            return employeeOrders;
        }
    }
}
