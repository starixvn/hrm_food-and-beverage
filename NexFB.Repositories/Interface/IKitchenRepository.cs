// --------------------------------------------------------------------------
// <copyright file="IKitchenRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Kitchen Repository.
    /// </summary>
    public interface IKitchenRepository
    {
        List<Order> GetMealOrder(ICriteria criteria);

        Order GetOrderDetails(ICriteria criteria);

        IList<Order> GetOrderByQuery(ICriteria criteria);

        Order GetMealOrderById(ICriteria criteria);

        EmployeeInGroup GetEmployeeInGroup(ICriteria criteria);
    }
}
