namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using System.Collections.Generic;
    using Core.Enums;

    /// <summary>
    /// The monthly main dish report local model.
    /// </summary>
    [Serializable]
    public class MonthlyLocalReportModel
    {
        public MonthlyLocalReportModel()
        {
            Meals = new List<MealMonthModel>();
        }

        public DateTime Month { get; set; }

        public bool IsGroupByEmployee { get; set; }

        public GroupByReport GroupBy { get; set; }

        public List<MealMonthModel> Meals { get; set; }

        public MealDayOfMonthModel Total { get; set; }

        public MealDayOfMonthModel WeekDays { get; set; }

        public int TotalItem { get; set; }
    }
}
