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
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.FoodMealPlanning;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.Entities;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.FoodMealPlanning;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The Meal Planning service.
    /// </summary>
    public class MealPlanningService : BaseService, IMealPlanningService
    {
        private readonly IMealPlanningRepository _mealPlanningRepository;
        private readonly ICommonService _commonService;

        public MealPlanningService()
            : this(null, null)
        {
        }

        public MealPlanningService(
            IMealPlanningRepository mealPlanningRepository,
            ICommonService commonService)
        {
            _mealPlanningRepository = mealPlanningRepository ?? new MealPlanningRepository();
            _commonService = commonService ?? new CommonService();
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

                foreach (MealPlanningModel mealPlanning in mealPlannings)
                {
                    mealPlanning.IsDisable = CheckDisableMealPlanning(mealPlanning);
                }
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

        /// <summary>
        /// Check meal planning is disabled or not.
        /// </summary>
        /// <param name="mealPlanning">The meal planning.</param>
        /// <returns>Is disabled or not.</returns>
        public bool CheckDisableMealPlanning(MealPlanningModel mealPlanning)
        {
            bool isDisable = false;

            if (mealPlanning.Date.Date < DateTime.Now.Date)
            {
                isDisable = true;
            }
            else if (mealPlanning.Date.Date == DateTime.Now.Date)
            {
                int numberMeals = mealPlanning.Menu.Meals.Count;
                MealModel lastMeal = numberMeals > 0 ? mealPlanning.Menu.Meals[numberMeals - 1] : null;

                if (lastMeal != null)
                {
                    isDisable = CheckMealTimesheet(lastMeal);
                }
            }

            return isDisable;
        }

        /// <summary>
        /// Check meal timesheet.
        /// </summary>
        /// <param name="lastMeal">The last meal of current meal planning.</param>
        /// <returns>Is disabled or not.</returns>
        private bool CheckMealTimesheet(MealModel lastMeal)
        {
            bool isDisable = false;
            var criteria = new GetMealTimesheetCriteria();
            IEnumerable<MealTimesheet> mealTimesheets = _commonService.GetAllMealTimesheet(criteria);
            MealTimesheet lastTimesheet = GetLastTimesheet(mealTimesheets, lastMeal);

            if (lastTimesheet != null)
            {
                isDisable = CheckRightMeal(lastTimesheet);
            }

            return isDisable;
        }

        /// <summary>
        /// Get last timesheet.
        /// </summary>
        /// <param name="mealTimesheets">All meal timesheets.</param>
        /// <param name="lastMeal">The last meal of current meal planning.</param>
        /// <returns>The last meal timesheet.</returns>
        private MealTimesheet GetLastTimesheet(IEnumerable<MealTimesheet> mealTimesheets, MealModel lastMeal)
        {
            foreach (MealTimesheet mealTimesheet in mealTimesheets)
            {
                if (mealTimesheet.MealId == lastMeal.MealId)
                {
                    return mealTimesheet;
                }
            }

            return null;
        }

        private bool CheckRightMeal(MealTimesheet lastTimesheet)
        {
            if (DateTime.Now.TimeOfDay < lastTimesheet.StartTime.TimeOfDay)
            {
                return false;
            }

            return true;
        }
    }
}
