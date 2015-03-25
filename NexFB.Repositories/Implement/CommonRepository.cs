// ----------------------------------------------------------------------
// <copyright file="CommonRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 3:22:46 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NexFB.Models.Employee;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The Common repository.
    /// </summary>
    public class CommonRepository : ICommonRepository
    {
        /// <summary>
        /// Get meals of menu.
        /// </summary>
        /// <param name="menuId">The menu ID.</param>
        /// <param name="rawMeals">The meals from database.</param>
        /// <param name="date">The On Date.</param>
        /// <returns>The list Meal Model belong to current menu.</returns>
        public IList<MealModel> GetMealsOfMenu(int menuId, IList<dynamic> rawMeals, DateTime? date = null)
        {
            IList<MealModel> meals = new List<MealModel>();
            IEnumerable<dynamic> rawMealsOfMenu = date.HasValue ? rawMeals.Where(x => x.MenuID == menuId && x.OnDate.Date == date.Value.Date)
                : rawMeals.Where(x => x.MenuID == menuId);

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
        /// <param name="date">The On Date.</param>
        /// <returns>The list Employee Type belong to current menu.</returns>
        public IList<EmployeeTypeModel> GetEmployeeTypeOfMenu(int menuId, IList<dynamic> rawEmployeeTypes, DateTime? date = null)
        {
            IList<EmployeeTypeModel> employeeTypes = new List<EmployeeTypeModel>();
            IEnumerable<dynamic> rawEmployeeTypesOfMenu = date.HasValue ? rawEmployeeTypes.Where(x => x.MenuID == menuId && x.OnDate.Date == date.Value.Date)
                : rawEmployeeTypes.Where(x => x.MenuID == menuId);

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
    }
}
