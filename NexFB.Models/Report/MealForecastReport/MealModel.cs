namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    [Serializable]
    public class MealModel
    {
        public int MenuId { get; set; }

        public int MealId { get; set; }

        public string MealName { get; set; }
    }
}