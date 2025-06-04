namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;

    /// <summary>
    /// The total price model.
    /// </summary>
    [Serializable]
    public class TotalPriceModel
    {
        public string Total { get; set; }

        public int Breakfast { get; set; }

        public int Lunch { get; set; }

        public int Dinner { get; set; }

        public int Supper { get; set; }
    }
}
