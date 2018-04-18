using System.Collections.Generic;

namespace NexFB.Models.Report.MonthlyMealPriceReport
{
    public class BreakfastAndSupperTotalAmountReportModel
    {
        public IEnumerable<BreakfastAndSupperModel> MealAmountList { get; set; }

        public int TotalBreakfastMeals { get; set; }

        public double MonthlyTotalBreakfastAmount { get; set; }

        public int TotalSupperMeals { get; set; }

        public double MonthlyTotalSupperAmount { get; set; }

        public double TotalAmount
        {
            get
            {
                return MonthlyTotalBreakfastAmount + MonthlyTotalSupperAmount;
            }
        }
    }
}