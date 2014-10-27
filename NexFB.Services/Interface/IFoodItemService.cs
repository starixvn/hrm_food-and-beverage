// ----------------------------------------------------------------------
// <copyright file="IFoodItemService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 10:03:47 AM">Created.</change>
//   <change who="Quinn.Vo" date="2014.10.27">Add Update Stastus of food item.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;

    /// <summary>
    /// The interface of Food Item service.
    /// </summary>
    public interface IFoodItemService
    {
        IList<FoodItemModel> GetAllFoodItem(GetAllFoodItemCriteria criteria);

        CommandResult InsertFoodItem(InsertFoodItemCriteria criteria);

        CommandResult UpdateFoodItem(UpdateFoodItemCriteria criteria);

        CommandResult DeleteFoodItem(DeleteFoodItemCriteria criteria);

        CommandResult UpdateStatusFoodItem(UpdateStatusFoodItemCriteria criteria);

        IEnumerable<FoodItemModel> GetBasicFoodItem(GetBasicFoodItemCriteria criteria);
    }
}
