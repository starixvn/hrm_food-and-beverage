// ----------------------------------------------------------------------
// <copyright file="IFoodItemRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:22:23 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodItemManagement;

    /// <summary>
    /// The food item management repository.
    /// </summary>
    public interface IFoodItemRepository
    {
        IList<FoodItemModel> GetAllFoodItem(ICriteria criteria);
    }
}
