namespace NexFB.Models.MealTimeManagement
{
    /// <summary>
    /// The meal time management model.
    /// </summary>
    public class MealTimeModel
    {
        public int MealId { get; set; }

        public string RegisterTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}
