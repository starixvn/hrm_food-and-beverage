using System.Collections.Generic;

namespace NexFB.Models.Report.MonthlyMealPriceReport
{
    public class LunchAndDinnerTotalAmountReportModel
    {
        public IEnumerable<LunchAndDinnerModel> MealPriceList { get; set; }

        public int TotalLunchMeals { get; set; }

        public double MonthlyTotalLunchAmount { get; set; }

        public int TotalDinnerMeals { get; set; }

        public double MonthlyTotalDinnerAmount { get; set; }

        public double TotalAmount
        {
            get
            {
                return MonthlyTotalLunchAmount + MonthlyTotalDinnerAmount;
            }
        }
    }
}