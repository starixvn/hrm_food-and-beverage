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

        public IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(GetAllMealPlanningByOnDateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<MealPlanningModel> mealPlannings = null;

            if (criteria.IsValid())
            {
                mealPlannings = _mealPlanningRepository.GetMealPlanningByOnDate(criteria);
                bool isDisable = false;
                bool isCurrentMeal = false;

                foreach (MealPlanningModel mealPlanning in mealPlannings)
                {
                    CheckDisableMealPlanning(mealPlanning, ref isDisable, ref isCurrentMeal);
                    mealPlanning.IsDisable = isDisable;
                    mealPlanning.IsCurrentMeal = isCurrentMeal;
                    isDisable = false;
                    isCurrentMeal = false;
                }
            }

            return mealPlannings;
        }

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

        private void CheckDisableMealPlanning(MealPlanningModel mealPlanning, ref bool isDisable, ref bool isCurrentMeal)
        {
            if (mealPlanning.Date.Date < DateTime.Now.Date)
            {
                isDisable = true;
            }
            else
            {
                if (mealPlanning.Date.Date == DateTime.Now.Date)
                {
                    int numberMeals = mealPlanning.Menu.Meals.Count;
                    MealModel firstMeal = numberMeals > 0 ? mealPlanning.Menu.Meals[0] : null;
                    MealModel lastMeal = numberMeals > 0 ? mealPlanning.Menu.Meals[numberMeals - 1] : null;

                    if (firstMeal != null)
                    {
                        CheckMealTimesheet(firstMeal, lastMeal, ref isDisable, ref isCurrentMeal);
                    }
                }
            }
        }

        private void CheckMealTimesheet(MealModel firstMeal, MealModel lastMeal, ref bool isDisable, ref bool isCurrentMeal)
        {
            var criteria = new GetMealTimesheetCriteria();
            IEnumerable<MealTimesheet> mealTimesheets = _commonService.GetAllMealTimesheet(criteria);
            MealTimesheet firstTimesheet = GetTimesheet(mealTimesheets, firstMeal);
            MealTimesheet lastTimesheet = GetTimesheet(mealTimesheets, lastMeal);

            if (firstTimesheet != null)
            {
                isDisable = CheckRightMeal(firstTimesheet);
                isCurrentMeal = CheckCurrentMeal(lastTimesheet);
            }
        }

        private MealTimesheet GetTimesheet(IEnumerable<MealTimesheet> mealTimesheets, MealModel meal)
        {
            foreach (MealTimesheet mealTimesheet in mealTimesheets)
            {
                if (mealTimesheet.MealId == meal.MealId)
                {
                    return mealTimesheet;
                }
            }

            return null;
        }

        private bool CheckRightMeal(MealTimesheet firstTimesheet)
        {
            if (DateTime.Now.TimeOfDay < firstTimesheet.StartTime.TimeOfDay)
            {
                return false;
            }

            return true;
        }

        private bool CheckCurrentMeal(MealTimesheet lastTimesheet)
        {
            if (DateTime.Now.TimeOfDay < lastTimesheet.StartTime.TimeOfDay)
            {
                return true;
            }

            return false;
        }
    }
}