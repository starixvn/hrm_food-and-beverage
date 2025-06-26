namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System;
    using System.Collections.Generic;
    using Core.Enums;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;

    /// <summary>
    /// The Monthly Main Dish Per Day model.
    /// </summary>
    [Serializable]
    public class MonthlyMainDishPerDayModel
    {
        public MonthlyMainDishPerDayModel()
        {
            TypeDays = new List<MainDishPerDayModel>();
        }

        public DateTime Month { get; set; }

        public bool IsGroupByEmployee { get; set; }

        public GroupByReport GroupBy { get; set; }

        public List<MainDishPerDayModel> TypeDays { get; set; }

        public MealDayOfMonthModel Total { get; set; }

        public MealDayOfMonthModel WeekDays { get; set; }

        public int TotalItem { get; set; }

        public List<BreakRuleOrder> BreakRuleOrders { get; set; }

    }
}
