// ----------------------------------------------------------------------
// <copyright file="IFoodMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:20:05 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// The interface of food menu repository.
    /// </summary>
    public interface IFoodMenuRepository
    {
        IList<FoodMenuModel> GetAllFoodMenu(ICriteria criteria);

        IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(ICriteria criteria);

        IEnumerable<FoodMenuHistoryModel> GetFoodMenuHistory(ICriteria criteria);
    }
}