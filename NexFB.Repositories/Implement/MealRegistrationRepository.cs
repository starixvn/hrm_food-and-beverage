namespace NexFB.Repositories.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.MealRegistration;
    using NexFB.Repositories.Interface;

    public class MealRegistrationRepository : IMealRegistrationRepository
    {
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    multi.Read();
                    var rawMealDates = multi.Read<RawMealDateModel>();

                    IList<MealRegisterModel> mealDates = ConvertMealDate(rawMealDates);
                    return mealDates;
                }
            }
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "<Pending>")]
        private IList<MealDateModel> GetMealDateInWeek(List<RawMealDateModel> rawMealInWeek)
        {
            IList<MealDateModel> mealDates = new List<MealDateModel>();
            int numberDay = rawMealInWeek.Count;
            const byte TotalDayOfWeek = 7;

            if (numberDay != TotalDayOfWeek)
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

                mealDates.Add(mealDate);
            }

            return mealDates;
        }

        private List<RawMealDateModel> InsertMissingDateInWeek(List<RawMealDateModel> rawMealInWeek)
        {
            var newRawMealInWeek = new List<RawMealDateModel>();
            int week = rawMealInWeek.First().Week;
            DateTime fitstWorkDate = rawMealInWeek.First().WorkDate;
            int firstWeekDay = rawMealInWeek.First().WeekDay;
            const byte TotalDayOfWeek = 7;

            for (int i = 0; i < TotalDayOfWeek; i++)
            {
                int weekDay = i + 1;
                int dateIndex = rawMealInWeek.FindIndex(x => x.WeekDay == weekDay);
                DateTime itemWorkDate = i > 0 ? newRawMealInWeek[0].WorkDate.AddDays(i) : fitstWorkDate.AddDays(weekDay - firstWeekDay);

                var newDate = new RawMealDateModel
                {
                    WeekDay = weekDay,
                    Week = week,
                    WorkDate = itemWorkDate,
                    IsDisabled = dateIndex < 0,
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
