namespace NexFB.Models.MealRegistration
{
    /// <summary>
    /// The default meal model.
    /// </summary>
    public class DefaultMealModel
    {
        public int UserId { get; set; }

        public int MealId { get; set; }

        public bool? MonMeal { get; set; }

        public bool? TueMeal { get; set; }

        public bool? WedMeal { get; set; }

        public bool? ThuMeal { get; set; }

        public bool? FriMeal { get; set; }

        public bool? SatMeal { get; set; }

        public bool? SunMeal { get; set; }
    }
}
