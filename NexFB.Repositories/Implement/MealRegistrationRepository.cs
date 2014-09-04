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
    using System.Collections.Generic;
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
        public IList<MealDateModel> GetRegisterMealByRangeDate(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    multi.Read();
                    IEnumerable<RawMealDateModel> rawMealDates = multi.Read<RawMealDateModel>();

                    IList<MealDateModel> mealDates = ConvertMealDate(rawMealDates);
                    return mealDates;
                }
            }
        }

        /// <summary>
        /// Convert meal date.
        /// </summary>
        /// <param name="rawMealDates">The enumerator raw meal dates.</param>
        /// <returns>The list meal date model.</returns>
        private IList<MealDateModel> ConvertMealDate(IEnumerable<RawMealDateModel> rawMealDates)
        {
            IList<MealDateModel> mealDates = new List<MealDateModel>();

            foreach (RawMealDateModel rawMealDate in rawMealDates)
            {
                var mealDate = new MealDateModel
                {
                    WorkDate = rawMealDate.WorkDate
                };
                mealDate.Meals[0].IsVegetarian = rawMealDate.Breakfast;
                mealDate.Meals[1].IsVegetarian = rawMealDate.Lunch;
                mealDate.Meals[2].IsVegetarian = rawMealDate.Dinner;
                mealDate.Meals[3].IsVegetarian = rawMealDate.Supper;

                mealDates.Add(mealDate);
            }

            return mealDates;
        }
    }
}
