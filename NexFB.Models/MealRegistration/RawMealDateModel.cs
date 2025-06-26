namespace NexFB.Models.MealRegistration
{
    using System;

    /// <summary>
    /// The raw meal date model contain data from database.
    /// </summary>
    public class RawMealDateModel
    {
        public DateTime WorkDate { get; set; }

        public int Week { get; set; }

        public int WeekDay { get; set; }

        public int? Breakfast { get; set; }

        public int? Lunch { get; set; }

        public int? Dinner { get; set; }

        public int? Supper { get; set; }

        public bool IsDisabled { get; set; }
    }
}
