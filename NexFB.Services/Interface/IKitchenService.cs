// --------------------------------------------------------------------------
// <copyright file="IKitchenService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Interface Kitchen Service.
    /// </summary>
    public interface IKitchenService
    {
        List<Order> GetWaitngMealOrder(GetWaitingMealOrderCriteria criteria);

        List<Order> GetAwaitingDeliverMealOrder(GetAwaitingDeliverMealOrderCriteria criteria);

        List<Order> GetDeliveredMealOrder(GetDeliveredMealOrderCriteria criteria);

        CommandResult ReceiveOrder(ProcessReceiveOrderCriteria criteria);

        CommandResult DeliverOrder(ProcessDeliverOrderCriteria criteria);

        CommandResult UndoOrder(UndoOrderCriteria criteria);

        Order GetOrderDetails(GetOrderDetailsCriteria criteria);

        Order GetOrderDetailsByOrderNo(GetOrderDetailsByOrderNoCriteria criteria);

        Order GetMealOrderById(GetMealOrderByIdCriteria criteria);

        List<GroupOrder> GetAllGroupOrder(GetGroupNameCriteria criteria);

        List<FoodOrder> GetAllFoodOrder(GetFoodNameCriteria criteria);

        List<EmployeeInfo> GetUserByGroupId(GetUserByGroupIdCriteria criteria);

        List<EmployeeInfo> GetEmployeeOrderInfo(GetEmployeeByCodeCriteria criteria);

        EmployeeInGroup GetEmployeeInGroup(GetEmployeeInGroupBySysIdCriteria criteria);

        EmployeeInfo GetEmployeeBySysId(GetEmployeeBySysIdCriteria criteria);

        List<FoodOrder> GetAllAlternativeFoodOrder(GetAlternativeFoodCriteria criteria);

        OrderResult InsertOrderFood(InsertOrderMealCriteria criteria);

        CommandResult EditOrderFood(EditOrderMealCriteria criteria);

        CommandResult CancelOrder(CancelOrderCriteria criteria);
    }
}
