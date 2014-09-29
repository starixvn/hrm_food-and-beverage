// ----------------------------------------------------------------------
// <copyright file="IFoodMenuService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:03:50 AM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMenuManagement;
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// The interface of food menu service.
    /// </summary>
    public interface IFoodMenuService
    {
        IList<FoodMenuModel> GetAllFoodMenu(GetAllFoodMenuCriteria criteria);

        IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(GetAllFoodMenuCriteria criteria);

        CommandResult InsertFoodMenu(InsertFoodMenuCriteria criteria);

        CommandResult DeleteFoodMenu(DeleteFoodMenuByMenuIdCriteria criteria);

        CommandResult UpdateFoodMenu(UpdateFoodMenuCriteria criteria);
    }
}
