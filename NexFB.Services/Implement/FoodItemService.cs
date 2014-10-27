// ----------------------------------------------------------------------
// <copyright file="FoodItemService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 10:05:42 AM">Created.</change>
//   <change who="Quinn.Vo" date="2014.10.27">Add Update Status of food item.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface;

    /// <summary>
    /// The food item service.
    /// </summary>
    public class FoodItemService : BaseService, IFoodItemService
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

        /// <summary>
        /// Insert food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertFoodItem(InsertFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Update food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateFoodItem(UpdateFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Delete food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteFoodItem(DeleteFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Update status of Food item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateStatusFoodItem(UpdateStatusFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult result = null;

            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }
            else
            {
                result = new CommandResult()
                {
                    Code = 1,
                    Message = Lang.InvalidInputData
                };
            }

            return result;
        }

        /// <summary>
        /// Get basic information of food items.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator basic information of food items.</returns>
        public IEnumerable<FoodItemModel> GetBasicFoodItem(GetBasicFoodItemCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<FoodItemModel> foodItems = null;

            if (criteria.IsValid())
            {
                foodItems = GetData<FoodItemModel>(criteria);
            }

            return foodItems;
        }
    }
}
