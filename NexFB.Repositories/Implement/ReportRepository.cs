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
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
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
                if (mealReport.Meal == null)
                {
                    mealReport.Meal = GetMeal(rawMeal);
                }

                MealItemMonthModel mealItem = GetItemOfMeal(rawMeal);
                mealReport.MealItems.Add(mealItem);
            }

            return mealReport;
        }

        private MealModel GetMeal(dynamic rawMeal)
        {
            var meal = new MealModel
            {
                MealId = rawMeal.MealID,
                MealEngName = rawMeal.MealEngName,
                MealVnmName = rawMeal.MeaVNMName
            };

            return meal;
        }

        /// <summary>
        /// Get item of meal in month.
        /// </summary>
        /// <param name="rawMeal">The data of current meal item.</param>
        /// <returns>The Meal Item Month model.</returns>
        private MealItemMonthModel GetItemOfMeal(dynamic rawMeal)
        {
            var mealItem = new MealItemMonthModel
            {
                Employee = GetEmployeeOfMealItem(rawMeal),
                EmployeeType = GetEmloyeeTypeOfMealItem(rawMeal),
                Department = GetDepartmentOfMealItem(rawMeal),
                Team = GetTeamOfMealItem(rawMeal)
                ////MealsMonth = GetNumberMealsOfItem(rawMeal)
            };

            return mealItem;
        }

        /// <summary>
        /// Get employee of meal item.
        /// </summary>
        /// <param name="rawMeal">The data of current meal item.</param>
        /// <returns>The Employee model.</returns>
        private Employee GetEmployeeOfMealItem(dynamic rawMeal)
        {
            int? userId = rawMeal.SysID;

            if (userId.HasValue)
            {
                var employee = new Employee
                {
                    UserId = (int)userId,
                    FullName = rawMeal.FullName,
                    NickName = rawMeal.NickName,
                    Code = rawMeal.Code,
                    DomainUsername = rawMeal.DomainUsername
                };

                return employee;
            }

            return null;
        }

        /// <summary>
        /// Get employee type of meal item.
        /// </summary>
        /// <param name="rawMeal">The data of current meal item.</param>
        /// <returns>The Employee Type model.</returns>
        private EmployeeTypeModel GetEmloyeeTypeOfMealItem(dynamic rawMeal)
        {
            int? employeeTypeId = rawMeal.EmployeeTypeID;

            if (employeeTypeId.HasValue)
            {
                var employeeType = new EmployeeTypeModel
                {
                    EmployeeTypeId = (int)employeeTypeId,
                    EmployeeTypeName = rawMeal.EmployeeTypeName
                };

                return employeeType;
            }

            return null;
        }

        private Department GetDepartmentOfMealItem(dynamic rawMeal)
        {
            var department = new Department
            {
                DepartmentId = rawMeal.DepartmentID,
                Name = rawMeal.DepartmentName
            };

            return department;
        }

        private Team GetTeamOfMealItem(dynamic rawMeal)
        {
            var team = new Team
            {
                TeamId = rawMeal.EmpTeamID,
                Code = rawMeal.EmpTeamCode,
                Name = rawMeal.EmpTeamName
            };

            return team;
        }

        ////private MealDayOfMonthModel GetNumberMealsOfItem(dynamic rawMeal)
        ////{
        ////    throw new System.NotImplementedException();
        ////}
    }
}
