using System;

namespace NexFB.Models.Report.MonthlyMealPriceReport
{
    public class LunchAndDinnerModel
    {
        public DateTime OnDate { get; set; }

        public int LunchMealQuantity { get; set; }

        public double PricePerLunch { get; set; }

        public double LunchAmount { get; set; }

        public int DinnerMealQuantity { get; set; }

        public double PricePerDinner { get; set; }

        public double DinnerAmount { get; set; }

        public double TotalAmount
        {
            get
            {
                return LunchAmount + DinnerAmount;
            }
        }
    }
}