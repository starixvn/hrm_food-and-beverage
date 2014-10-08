// ----------------------------------------------------------------------
// <copyright file="MealPlanningService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 4:01:27 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.FoodMealPlanning;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The Meal Planning service.
    /// </summary>
    public class MealPlanningService : BaseService, IMealPlanningService
    {
        private readonly IMealPlanningRepository _mealPlanningRepository;

        public MealPlanningService()
            : this(null)
        {
        }

        public MealPlanningService(IMealPlanningRepository mealPlanningRepository)
        {
            _mealPlanningRepository = mealPlanningRepository ?? new MealPlanningRepository();
        }

        /// <summary>
        /// Get meal planning by on date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Meal Planning model.</returns>
        public IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(GetAllMealPlanningByOnDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<MealPlanningModel> mealPlannings = null;

            if (criteria.IsValid())
            {
                mealPlannings = _mealPlanningRepository.GetMealPlanningByOnDate(criteria);
            }

            return mealPlannings;
        }

        /// <summary>
        /// Insert meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult InsertMealPlanning(InsertMealPlanningCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdateMealPlanning(UpdateMealPlanningCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Delete meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult DeleteMealPlanning(DeleteMealPlanningCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update meal planning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult ActivateMealPlanning(ActivateMealPlanningCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Get food items by on date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Food Item model.</returns>
        public IEnumerable<FoodItemModel> GetFoodByOnDate(GetFoodMealPlanningByOnDateCriteria criteria)
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
