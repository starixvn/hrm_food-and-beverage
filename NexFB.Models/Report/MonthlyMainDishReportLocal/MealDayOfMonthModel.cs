namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The meal days of month model.
    /// </summary>
    [Serializable]
    public class MealDayOfMonthModel
    {
        public MealDayOfMonthModel()
        {
            NumberMeals = new List<int>();
        }

        public List<int> NumberMeals { get; set; }

        public List<string> Shifts { get; set; }

        public int TotalQuantity { get; set; }

        public decimal Amount { get; set; }
    }
}
