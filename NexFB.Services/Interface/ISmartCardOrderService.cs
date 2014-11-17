// --------------------------------------------------------------------------
// <copyright file="ISmartCardOrderService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;

    /// <summary>
    /// Interface SmartCardOrderService.
    /// </summary>
    public interface ISmartCardOrderService
    {
        EmployeeInfo GetEmployeeByCardId(GetEmployeeByCardNumberCriteria criteria);

        List<FoodOrder> GetAlternativeFoodBySysId(GetAlternativeFoodBySysIdCriteria criteria);

        OrderResult InsertOrderFood(InsertOrderMealByUserCriteria criteria);

        OrderResult InsertOrderSideFood(InsertOrderSideDishesByUserCriteria criteria);

        List<FoodOrder> GetSideFoodBySysId(GetSideFoodBySysIdCriteria criteria);

        List<SCEmployeeOrder> GetMyOrderBySysId(GetMyOrderBySysIdCriteria criteria);

        OrderStatus EmployeeOrderStatus(GetEmployeeOrderStatusCriteria criteria);
    }
}
