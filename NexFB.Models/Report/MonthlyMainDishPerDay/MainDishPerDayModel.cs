namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Resources;

    /// <summary>
    /// The main dish per day model.
    /// </summary>
    [Serializable]
    public class MainDishPerDayModel
    {
        public MainDishPerDayModel()
        {
            MealItems = new List<MealItemMonthModel>();
        }

        public bool IsOffDay { get; set; }

        public string TypeDay
        {
            get
            {
                return IsOffDay ? Lang.OffDay : Lang.WorkDay;
            }
        }

        public List<MealItemMonthModel> MealItems { get; set; }
    }
}
