// -----------------------------------------------------------------------
// <copyright file="MealForecastDetailReportRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Repositories.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Repositories.Interface.Report;

    /// <summary>
    /// Meal forecast detail repository.
    /// </summary>
    public class MealForecastDetailReportRepository : IMealForecastDetailReportRepository
    {
        /// <summary>
        /// Get meal forecast detail repository.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The meal forecast detail history.</returns>
        public MealForecastDetailReport GetMealForecastDetailReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var report = new MealForecastDetailReport();

                    report.MealForecastDetails = multi.Read<MealForecastDetailModel>().ToList();
                    List<MealModel> allMeals = multi.Read<MealModel>().ToList();

                    foreach (MealForecastDetailModel mealForecastDetail in report.MealForecastDetails)
                    {
                        List<MealModel> meals = allMeals.Where(x => x.MenuId == mealForecastDetail.MenuId).ToList();
                        mealForecastDetail.Meals = meals;
                    }

                    return report;
                }
            }
        }

        public IList<EmployeeRegistrationModel> GetEmployeeRegistration(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var employeeMeals = db.Query<EmployeeRegistrationModel, MealModel, Employee, EmployeeRegistrationModel>(
                    (reg, ml, e) =>
                    {
                        reg.Meal = ml;
                        reg.Employee = e;
                        return reg;
                    },
                    splitOn: "MealID, UserId",
                    parameterValues: criteria.GetSpParams()).ToList();

                return employeeMeals;
            }
        }
    }
}