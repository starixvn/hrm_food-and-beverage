// ----------------------------------------------------------------------
// <copyright file="FoodItemService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 10:05:42 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The food item service.
    /// </summary>
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _foodItemRepository;

        public FoodItemService()
            : this(null)
        {
        }

        public FoodItemService(IFoodItemRepository foodItemRepository)
        {
            _foodItemRepository = foodItemRepository ?? new FoodItemRepository();
        }

        /// <summary>
        /// Get all food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list food item model.</returns>
        public IList<FoodItemModel> GetAllFoodItem(GetAllFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<FoodItemModel> foodItems = new List<FoodItemModel>();

            if (criteria.IsValid())
            {
                foodItems = _foodItemRepository.GetAllFoodItem(criteria);
            }

            return foodItems;
        }
    }
}
