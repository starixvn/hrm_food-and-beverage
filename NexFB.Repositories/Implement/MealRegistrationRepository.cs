// ----------------------------------------------------------------------
// <copyright file="MealRegistrationRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:58:02 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The meal registration repository.
    /// </summary>
    public class MealRegistrationRepository : IMealRegistrationRepository
    {
        /// <summary>
        /// Get register meal by range.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list meal date model.</returns>
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    multi.Read();
                    IEnumerable<RawMealDateModel> rawMealDates = multi.Read<RawMealDateModel>();

                    IList<MealRegisterModel> mealDates = ConvertMealDate(rawMealDates);
                    return mealDates;
                }
            }
        }

        /// <summary>
        /// Convert meal date.
        /// </summary>
        /// <param name="rawMealDates">The enumerator raw meal dates.</param>
        /// <returns>The list meal date in week model.</returns>
        private IList<MealRegisterModel> ConvertMealDate(IEnumerable<RawMealDateModel> rawMealDates)
        {
            var mealRegister = new List<MealRegisterModel>();
            IEnumerable<int> numberWeeks = rawMealDates.Select(x => x.Week).Distinct();

            foreach (int week in numberWeeks)
            {
                var weekMeal = new MealRegisterModel
                {
                    Week = week
                };

                List<RawMealDateModel> rawMealInWeek = rawMealDates.Where<RawMealDateModel>(x => x.Week == week).ToList();
                weekMeal.WeekMeals = GetMealDateInWeek(rawMealInWeek);

                mealRegister.Add(weekMeal);
            }

            return mealRegister;
        }

        /// <summary>
        /// Get meal date in week.
        /// </summary>
        /// <param name="rawMealInWeek">The list meal date in week.</param>
        /// <returns>The list meal date model.</returns>
        private IList<MealDateModel> GetMealDateInWeek(List<RawMealDateModel> rawMealInWeek)
        {
            IList<MealDateModel> mealDates = new List<MealDateModel>();
            int numberDay = rawMealInWeek.Count();

            if (numberDay != 7)
            {
                rawMealInWeek = InsertMissingDateInWeek(rawMealInWeek);
            }

            foreach (RawMealDateModel rawMealDate in rawMealInWeek)
            {
                var mealDate = new MealDateModel
                {
                    WorkDate = rawMealDate.WorkDate,
                    WeekDay = rawMealDate.WeekDay,
                    IsDisabled = rawMealDate.IsDisabled
                };

                mealDate.Meals[0].IsVegetarian = rawMealDate.Breakfast;
                mealDate.Meals[1].IsVegetarian = rawMealDate.Lunch;
                mealDate.Meals[2].IsVegetarian = rawMealDate.Dinner;
                mealDate.Meals[3].IsVegetarian = rawMealDate.Supper;

                if (rawMealDate.WorkDate.Date < DateTime.Now.Date)
                {
                    mealDate.IsDisabled = true;
                }

                mealDates.Add(mealDate);
            }

            return mealDates;
        }

        /// <summary>
        /// Insert missing date in week.
        /// </summary>
        /// <param name="rawMealInWeek">The meal date in week.</param>
        /// <returns>The list with new missing date in week.</returns>
        private List<RawMealDateModel> InsertMissingDateInWeek(List<RawMealDateModel> rawMealInWeek)
        {
            var newRawMealInWeek = new List<RawMealDateModel>();
            int week = rawMealInWeek.First().Week;
            DateTime fitstWorkDate = rawMealInWeek.First().WorkDate;
            int firstWeekDay = rawMealInWeek.First().WeekDay;

            for (int i = 0; i < 7; i++)
            {
                int weekDay = i + 1;
                int dateIndex = rawMealInWeek.FindIndex(x => x.WeekDay == weekDay);
                DateTime itemWorkDate;

                if (i > 0)
                {
                    itemWorkDate = newRawMealInWeek[0].WorkDate.AddDays(i);
                }
                else
                {
                    itemWorkDate = fitstWorkDate.AddDays(weekDay - firstWeekDay);
                }

                var newDate = new RawMealDateModel
                {
                    WeekDay = weekDay,
                    Week = week,
                    WorkDate = itemWorkDate,
                    IsDisabled = dateIndex >= 0 ? false : true,
                    Breakfast = dateIndex >= 0 ? rawMealInWeek[dateIndex].Breakfast : null,
                    Lunch = dateIndex >= 0 ? rawMealInWeek[dateIndex].Lunch : null,
                    Dinner = dateIndex >= 0 ? rawMealInWeek[dateIndex].Dinner : null,
                    Supper = dateIndex >= 0 ? rawMealInWeek[dateIndex].Supper : null
                };
                newRawMealInWeek.Add(newDate);
            }

            return newRawMealInWeek;
        }
    }
}
