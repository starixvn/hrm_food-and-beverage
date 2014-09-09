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
    using System;
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
                Name = Enumerations.GetDescription(Meal.Breakfast),
                ShortName = "B"
            };
            var lunch = new MealModel
            {
                MealId = 2,
                Name = Enumerations.GetDescription(Meal.Lunch),
                ShortName = "L"
            };
            var dinner = new MealModel
            {
                MealId = 3,
                Name = Enumerations.GetDescription(Meal.Dinner),
                ShortName = "D"
            };
            var supper = new MealModel
            {
                MealId = 4,
                Name = Enumerations.GetDescription(Meal.Supper),
                ShortName = "S"
            };

            Meals.Add(breakfast);
            Meals.Add(lunch);
            Meals.Add(dinner);
            Meals.Add(supper);
        }

        public DateTime WorkDate { get; set; }

        public int WeekDay { get; set; }

        public bool IsDisabled { get; set; }

        public IList<MealModel> Meals { get; set; }
    }
}
