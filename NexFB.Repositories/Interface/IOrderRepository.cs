// --------------------------------------------------------------------------
// <copyright file="IOrderRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    public interface IOrderRepository
    {
        Menu GetMenuByUserId(ICriteria criteria);

        IList<MealOrderingModel> GetMyOrderByCurrentDay(ICriteria criteria);
    }
}
