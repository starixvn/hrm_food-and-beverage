namespace NexFB.Models.MealRegistration
{
    using System.Collections.Generic;

    /// <summary>
    /// The meal register model.
    /// </summary>
    public class MealRegisterModel
    {
        public int Week { get; set; }

        public IList<MealDateModel> WeekMeals { get; set; }
    }
}
