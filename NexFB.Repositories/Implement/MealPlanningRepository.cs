// ----------------------------------------------------------------------
// <copyright file="MealPlanningRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 2:30:49 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.FoodMealPlanning;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Models.SetMenuManagement;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The Meal Planning repository.
    /// </summary>
    public class MealPlanningRepository : IMealPlanningRepository
    {
        private readonly ICommonRepository _commonRepository;

        public MealPlanningRepository()
            : this(null)
        {
        }

        public MealPlanningRepository(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository ?? new CommonRepository();
        }

        /// <summary>
        /// Get meal planning bu on date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Meal Planning model.</returns>
        public IEnumerable<MealPlanningModel> GetMealPlanningByOnDate(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IEnumerable<MealPlanningModel> mealPlannings = 
                        multi.Read<MealPlanningModel, FoodMenuModel, SetMenuModel, MealPlanningModel>(
                        (mepl, menu, setMenu) =>
                        {
                            mepl.Menu = menu;
                            mepl.SetMenu = setMenu;
                            return mepl;
                        },
                        "MenuId, FoodSetId");

                    IList<dynamic> rawMeals = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawEmployeeTypes = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawFoodItems = multi.Read<dynamic>().ToList();

                    foreach (MealPlanningModel mealPlanning in mealPlannings)
                    {
                        int menuId = mealPlanning.Menu.MenuId;
                        mealPlanning.Menu.Meals = _commonRepository.GetMealsOfMenu(menuId, rawMeals);
                        mealPlanning.Menu.EmployeeTypes = _commonRepository.GetEmployeeTypeOfMenu(menuId, rawEmployeeTypes);

                        if (mealPlanning.SetMenu == null)
                        {
                            mealPlanning.SetMenu = new SetMenuModel();
                        }

                        mealPlanning.SetMenu.FoodItems = GetFoodItemsOfSetMenu(menuId, mealPlanning.Date, rawFoodItems);
                    }

                    return mealPlannings;
                }
            }
        }

        /// <summary>
        /// Get food items of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="date"> The date to plan.</param>
        /// <param name="rawFoodItems">The list food items from database.</param>
        /// <returns>The list Food Item belong to current menu.</returns>
        private IList<FoodSetMenuModel> GetFoodItemsOfSetMenu(
            int menuId,
            DateTime date,
            IList<dynamic> rawFoodItems)
        {
            IList<FoodSetMenuModel> foodItems = new List<FoodSetMenuModel>();
            IEnumerable<dynamic> rawFoodItemsOfMenu = rawFoodItems.Where(x => x.MenuID == menuId && date == x.Date);

            foreach (dynamic rawFoodItem in rawFoodItemsOfMenu)
            {
                var foodItem = new FoodSetMenuModel
                {
                    FoodId = rawFoodItem.FoodID,
                    FoodEngName = rawFoodItem.FoodEngName,
                    FoodVnmName = rawFoodItem.FoodVNMName
                };

                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
