// --------------------------------------------------------------------------
// <copyright file="IOrderingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.Ordering;
    using NexFB.Models.AlternativeMenuManagement;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    public interface IOrderingService
    {
        Menu GetMenuByUserId(GetMenuByUserIdCriteria criteria);

        IList<MealOrderingModel> GetMyOrderByCurrentDay(GetMyOrderInCurrentDayCriteria criteria);

        MealOrderingModel GetMemberNewOrder(GetMemberNewOrderCriteria criteria);

        MealOrderingModel GetMemberSavedOrder(GetMemberSavedOrderCriteria criteria);

        MealOrderingModel GetOrderById(GetOrderByIdCriteria criteria);

        CommandResult InserOrder(InsertOrderCriteria criteria);

        CommandResult UpdateOrder(UpdateOrderCriteria criteria);

        CommandResult DeleteOrder(DeleteOrderCriteria criteria);

        CommandResult AddMember(AddMemberCriteria criteria);

        CommandResult RemoveMember(RemoveMemberCriteria criteria);

        IEnumerable<Menu> GetAlternativeMenuByUserId(GetAlternativeMenuByUserIdCriteria criteria);

        CommandResult InsertOrderSideDish(InsertOrderSideDishCriteria criteria);

        CommandResult UpdateOrderSideDish(UpdateOrderSideDishCriteria criteria);

        MealOrderingModel GetLastModifiedDateByOrderId(GetLastMofifiedDateByOrderIdCriteria criteria);

        string TranslateRemark(string remark, bool takeAway);

        string GetRemark(string remark, bool takeAway);
    }
}
