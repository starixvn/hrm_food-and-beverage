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
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
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
                    report.WeekDays = GetWeekDay(rawReport);
                    report.Total = CalculateTotalMeals(report.Meals);
                }

                return report;
            }
        }

        /// <summary>
        /// Get daily main dish price per meal.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The report.</returns>
        public DailyMainDishReportModel GetDailyMainDishReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var report = new DailyMainDishReportModel();
                    IEnumerable<dynamic> rawReport = multi.Read<dynamic>();
                    report.MainDishes = GroupMainDishByDepartmentTeam(rawReport);

                    report.SubTotal = multi.Read<TotalPriceModel>().FirstOrDefault();
                    report.GrandTotal = multi.Read<TotalPriceModel>().FirstOrDefault();

                    return report;
                }
            }
        }

        private List<DailyMainDishDepartmentTeamModel> GroupMainDishByDepartmentTeam(IEnumerable<dynamic> rawReport)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Get meals in month.
        /// </summary>
        /// <param name="rawReport">The raw data that returned from database.</param>
        /// <returns>The list meals in month.</returns>
        private List<MealMonthModel> GetMealsInMonth(IEnumerable<dynamic> rawReport)
        {
            var meals = new List<MealMonthModel>();
            IEnumerable<dynamic> rawMealIds = rawReport.Where(x => x.MealID != null).Select(x => x.MealID).Distinct();

            foreach (dynamic mealId in rawMealIds)
            {
                IEnumerable<dynamic> rawMeals = rawReport.Where(x => x.MealID == mealId);
                MealMonthModel meal = CreateMealInMonth(rawMeals);

                meals.Add(meal);
            }

            return meals;
        }

        /// <summary>
        /// Get meal items in month.
        /// </summary>
        /// <param name="rawMeals">The enumerator of meal items.</param>
        /// <returns>The meal in month.</returns>
        private MealMonthModel CreateMealInMonth(IEnumerable<dynamic> rawMeals)
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
                MealVnmName = rawMeal.MealVNMName
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
                Team = GetTeamOfMealItem(rawMeal),
                NumberMealsMonth = GetNumberMealsOfItem(rawMeal)
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

        /// <summary>
        /// Get department of meal item.
        /// </summary>
        /// <param name="rawMeal">The data of current meal item.</param>
        /// <returns>The department.</returns>
        private Department GetDepartmentOfMealItem(dynamic rawMeal)
        {
            int? departmentId = rawMeal.DepartmentID;

            if (departmentId.HasValue)
            {
                var department = new Department
                {
                    DepartmentId = (int)departmentId,
                    Name = rawMeal.DepartmentName
                };

                return department;
            }

            return null;
        }

        /// <summary>
        /// Get team of meal item.
        /// </summary>
        /// <param name="rawMeal">The data of current meal item.</param>
        /// <returns>The Team model.</returns>
        private Team GetTeamOfMealItem(dynamic rawMeal)
        {
            int? teamId = rawMeal.EmpTeamID;

            if (teamId.HasValue)
            {
                var team = new Team
                {
                    TeamId = (int)teamId,
                    Code = rawMeal.EmpTeamCode,
                    Name = rawMeal.EmpTeamName
                };

                return team;
            }

            return null;
        }

        /// <summary>
        /// Get number meals of item in month.
        /// </summary>
        /// <param name="rawMeal">The raw data of current meal item.</param>
        /// <returns>The model contain number meals of item in month.</returns>
        private MealDayOfMonthModel GetNumberMealsOfItem(dynamic rawMeal)
        {
            var mealDayOfMonth = new MealDayOfMonthModel();

            mealDayOfMonth.NumberMeals = GetNumberMealsInMonth(rawMeal);
            mealDayOfMonth.TotalQuantity = rawMeal.TotalQty;
            mealDayOfMonth.Amount = rawMeal.Amount;

            return mealDayOfMonth;
        }

        /// <summary>
        /// Get weekday in month.
        /// </summary>
        /// <param name="rawReport">The raw data of report.</param>
        /// <returns>The model contain weekdays in month.</returns>
        private MealDayOfMonthModel GetWeekDay(IEnumerable<dynamic> rawReport)
        {
            var weekDays = new MealDayOfMonthModel();

            dynamic rawWeekDay = rawReport.Where(x => x.MealID == null).First();
            weekDays.NumberMeals = GetNumberMealsInMonth(rawWeekDay);

            return weekDays;
        }

        /// <summary>
        /// Calculate total meals in month.
        /// </summary>
        /// <param name="meals">The list meals in month.</param>
        /// <returns>The model contain total meals in month.</returns>
        private MealDayOfMonthModel CalculateTotalMeals(List<MealMonthModel> meals)
        {
            var total = new MealDayOfMonthModel();

            foreach (MealMonthModel meal in meals)
            {
                foreach (MealItemMonthModel mealItem in meal.MealItems)
                {
                    List<int> numberMeals = mealItem.NumberMealsMonth.NumberMeals;

                    for (int i = 0; i < numberMeals.Count(); i++)
                    {
                        if (total.NumberMeals.Count < numberMeals.Count())
                        {
                            total.NumberMeals.Add(numberMeals[i]);
                        }
                        else
                        {
                            total.NumberMeals[i] += numberMeals[i];
                        }
                    }

                    total.TotalQuantity += mealItem.NumberMealsMonth.TotalQuantity;
                    total.Amount += mealItem.NumberMealsMonth.Amount;
                }
            }

            return total;
        }

        /// <summary>
        /// Get number meals of day in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of current meal item.</param>
        /// <returns>The list number meals of day in month.</returns>
        private List<int> GetNumberMealsInMonth(dynamic rawWeekDay)
        {
            var numberMeals = new List<int>();

            numberMeals = GetNumberMealsFirstWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsSecondWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsThridWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsFourthWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsFifthWeek(rawWeekDay, numberMeals);

            return numberMeals;
        }

        /// <summary>
        /// Get number meals of first week in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of meal item.</param>
        /// <param name="numberMeals">The list number meals of day in month.</param>
        /// <returns>The list number meals contain number meals of first week.</returns>
        private List<int> GetNumberMealsFirstWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D1);
            numberMeals.Add(rawWeekDay.D2);
            numberMeals.Add(rawWeekDay.D3);
            numberMeals.Add(rawWeekDay.D4);
            numberMeals.Add(rawWeekDay.D5);
            numberMeals.Add(rawWeekDay.D6);
            numberMeals.Add(rawWeekDay.D7);

            return numberMeals;
        }

        /// <summary>
        /// Get number meals of second week in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of meal item.</param>
        /// <param name="numberMeals">The list number meals of day in month.</param>
        /// <returns>The list number meals contain number meals of second week.</returns>
        private List<int> GetNumberMealsSecondWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D8);
            numberMeals.Add(rawWeekDay.D9);
            numberMeals.Add(rawWeekDay.D10);
            numberMeals.Add(rawWeekDay.D11);
            numberMeals.Add(rawWeekDay.D12);
            numberMeals.Add(rawWeekDay.D13);
            numberMeals.Add(rawWeekDay.D14);

            return numberMeals;
        }

        /// <summary>
        /// Get number meals of third week in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of meal item.</param>
        /// <param name="numberMeals">The list number meals of day in month.</param>
        /// <returns>The list number meals contain number meals of third week.</returns>
        private List<int> GetNumberMealsThridWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D15);
            numberMeals.Add(rawWeekDay.D16);
            numberMeals.Add(rawWeekDay.D17);
            numberMeals.Add(rawWeekDay.D18);
            numberMeals.Add(rawWeekDay.D19);
            numberMeals.Add(rawWeekDay.D20);
            numberMeals.Add(rawWeekDay.D21);

            return numberMeals;
        }

        /// <summary>
        /// Get number meals of fourth week in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of meal item.</param>
        /// <param name="numberMeals">The list number meals of day in month.</param>
        /// <returns>The list number meals contain number meals of fourth week.</returns>
        private List<int> GetNumberMealsFourthWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D22);
            numberMeals.Add(rawWeekDay.D23);
            numberMeals.Add(rawWeekDay.D24);
            numberMeals.Add(rawWeekDay.D25);
            numberMeals.Add(rawWeekDay.D26);
            numberMeals.Add(rawWeekDay.D27);
            numberMeals.Add(rawWeekDay.D28);

            return numberMeals;
        }

        /// <summary>
        /// Get number meals of fifth week in month.
        /// </summary>
        /// <param name="rawWeekDay">The data of meal item.</param>
        /// <param name="numberMeals">The list number meals of day in month.</param>
        /// <returns>The list number meals contain number meals of fifth week.</returns>
        private List<int> GetNumberMealsFifthWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            int? day29 = rawWeekDay.D29;

            if (day29.HasValue)
            {
                numberMeals.Add((int)day29);
            }
            else
            {
                numberMeals.Add(0);
            }

            int? day30 = rawWeekDay.D30;

            if (day30.HasValue)
            {
                numberMeals.Add((int)day30);
            }
            else
            {
                numberMeals.Add(0);
            }

            int? day31 = rawWeekDay.D31;

            if (day31.HasValue)
            {
                numberMeals.Add((int)day31);
            }
            else
            {
                numberMeals.Add(0);
            }

            return numberMeals;
        }
    }
}
