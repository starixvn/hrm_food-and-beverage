// ----------------------------------------------------------------------
// <copyright file="ReportRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 9:26:56 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The implement of report repository.
    /// </summary>
    public class ReportRepository : IReportRepository
    {
        /// <summary>
        /// Get monthly main dish report by local.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The monthly local report.</returns>
        public MonthlyLocalReportModel GetMonthlyMainDishReportLocal(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var report = new MonthlyLocalReportModel();

                IEnumerable<dynamic> rawReport = db.Query<dynamic>(criteria.GetSpParams());

                if (rawReport != null)
                {
                    report.Meals = GetMealsInMonth(rawReport);
                }

                return report;
            }
        }

        /// <summary>
        /// Get meals in month.
        /// </summary>
        /// <param name="rawReport">The data that returned from database.</param>
        /// <returns>The list meals in month.</returns>
        private List<MealMonthModel> GetMealsInMonth(IEnumerable<dynamic> rawReport)
        {
            var meals = new List<MealMonthModel>();

            IEnumerable<dynamic> rawMealIds = rawReport.Select(x => x.MealID).Distinct();

            foreach (dynamic mealId in rawMealIds)
            {
                IEnumerable<dynamic> rawMeals = rawReport.Where(x => x.MealID == mealId);
                MealMonthModel meal = GetMealItemInMonth(rawMeals);

                meals.Add(meal);
            }

            return meals;
        }

        /// <summary>
        /// Get meal item in month.
        /// </summary>
        /// <param name="rawMeals">The enumerator of meal item.</param>
        /// <returns>The meal in month model.</returns>
        private MealMonthModel GetMealItemInMonth(IEnumerable<dynamic> rawMeals)
        {
            var mealReport = new MealMonthModel();

            foreach (dynamic rawMeal in rawMeals)
            {
                var meal = new MealModel
                {
                    MealId = rawMeal.MealID
                };

                mealReport.Meal = meal;
            }

            return mealReport;
        }
    }
}
