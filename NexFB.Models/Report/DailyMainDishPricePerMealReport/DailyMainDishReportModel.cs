namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Daily main dish price per meal report model.
    /// </summary>
    [Serializable]
    public class DailyMainDishReportModel
    {
        public DailyMainDishReportModel()
        {
            MainDishes = new List<DailyMainDishCompanyTeamModel>();
        }

        public List<DailyMainDishCompanyTeamModel> MainDishes { get; set; }

        public TotalPriceModel Total { get; set; }

        public int GrandTotal { get; set; }

        public int TotalItem { get; set; }
    }
}
