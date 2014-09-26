// ----------------------------------------------------------------------
// <copyright file="FoodMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:21:31 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Employee;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The food menu repository.
    /// </summary>
    public class FoodMenuRepository : IFoodMenuRepository
    {
        /// <summary>
        /// Get all food menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Food Menu model.</returns>
        public IList<FoodMenuModel> GetAllFoodMenu(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    IList<FoodMenuModel> foodMenus = multi.Read<FoodMenuModel>().ToList();
                    IList<dynamic> rawMeals = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawEmployeeTypes = multi.Read<dynamic>().ToList();
                    IList<dynamic> rawFoodItems = multi.Read<dynamic>().ToList();

                    foreach (FoodMenuModel foodMenu in foodMenus)
                    {
                        int menuId = foodMenu.MenuId;

                        foodMenu.Meals = GetMealsOfMenu(menuId, rawMeals);
                        foodMenu.EmployeeTypes = GetEmployeeTypeOfMenu(menuId, rawEmployeeTypes);
                        foodMenu.FoodItems = GetFoodItemsOfMenu(menuId, rawFoodItems);
                    }

                    return foodMenus;
                }
            }
        }

        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    return multi.Read<FoodMenuModel>();
                }
            }
        }

        /// <summary>
        /// Get meals of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawMeals">The meals from database.</param>
        /// <returns>The list Meal Model belong to current menu.</returns>
        private IList<MealModel> GetMealsOfMenu(int menuId, IList<dynamic> rawMeals)
        {
            IList<MealModel> meals = new List<MealModel>();
            IEnumerable<dynamic> rawMealsOfMenu = rawMeals.Where(x => x.MenuID == menuId);

            foreach (dynamic rawMeal in rawMealsOfMenu)
            {
                var meal = new MealModel
                {
                    MealId = rawMeal.MealID,
                    MealVnmName = rawMeal.MealVNMName,
                    MealEngName = rawMeal.MealENGName
                };

                meals.Add(meal);
            }

            return meals;
        }

        /// <summary>
        /// Get employee type of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawEmployeeTypes">The list employee types from database.</param>
        /// <returns>The list Employee Type belong to current menu.</returns>
        private IList<EmployeeTypeModel> GetEmployeeTypeOfMenu(int menuId, IList<dynamic> rawEmployeeTypes)
        {
            IList<EmployeeTypeModel> employeeTypes = new List<EmployeeTypeModel>();
            IEnumerable<dynamic> rawEmployeeTypesOfMenu = rawEmployeeTypes.Where(x => x.MenuID == menuId);

            foreach (dynamic rawEmployeeType in rawEmployeeTypesOfMenu)
            {
                var employeeType = new EmployeeTypeModel
                {
                    EmployeeTypeId = rawEmployeeType.EmployeeTypeID,
                    EmployeeTypeName = rawEmployeeType.EmployeeTypeName
                };

                employeeTypes.Add(employeeType);
            }

            return employeeTypes;
        }

        /// <summary>
        /// Get food items of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawFoodItems">The list food items from database.</param>
        /// <returns>The list Food Item belong to current menu.</returns>
        private IList<FoodItemModel> GetFoodItemsOfMenu(int menuId, IList<dynamic> rawFoodItems)
        {
            IList<FoodItemModel> foodItems = new List<FoodItemModel>();
            IEnumerable<dynamic> rawFoodItemsOfMenu = rawFoodItems.Where(x => x.MenuID == menuId);

            foreach (dynamic rawFoodItem in rawFoodItemsOfMenu)
            {
                var foodItem = new FoodItemModel
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
