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
                MealEngName = Enumerations.GetDescription(Meal.Breakfast),
                ShortName = "B"
            };
            var lunch = new MealModel
            {
                MealId = 2,
                MealEngName = Enumerations.GetDescription(Meal.Lunch),
                ShortName = "L"
            };
            var dinner = new MealModel
            {
                MealId = 3,
                MealEngName = Enumerations.GetDescription(Meal.Dinner),
                ShortName = "D"
            };
            var supper = new MealModel
            {
                MealId = 4,
                MealEngName = Enumerations.GetDescription(Meal.Supper),
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
