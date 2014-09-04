// ----------------------------------------------------------------------
// <copyright file="MealDateModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:18:29 PM">Created.</change>
// </history>

namespace NexFB.Models.MealRegistration
{
    using System.Collections.Generic;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;

    /// <summary>
    /// The meal date model.
    /// </summary>
    public class MealDateModel
    {
        public MealDateModel()
        {
            Meals = new List<MealModel>();

            var breakfast = new MealModel
            {
                MealId = 1,
                Name = Enumerations.GetDescripton(Meal.Breakfast)
            };
            var lunch = new MealModel
            {
                MealId = 2,
                Name = Enumerations.GetDescripton(Meal.Lunch)
            };
            var dinner = new MealModel
            {
                MealId = 3,
                Name = Enumerations.GetDescripton(Meal.Dinner)
            };
            var supper = new MealModel
            {
                MealId = 4,
                Name = Enumerations.GetDescripton(Meal.Supper)
            };

            Meals.Add(breakfast);
            Meals.Add(lunch);
            Meals.Add(dinner);
            Meals.Add(supper);
        }

        public string WorkDate { get; set; }

        public IList<MealModel> Meals { get; set; }
    }
}
