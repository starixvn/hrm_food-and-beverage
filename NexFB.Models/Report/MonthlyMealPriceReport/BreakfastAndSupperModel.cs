using System;

namespace NexFB.Models.Report.MonthlyMealPriceReport
{
    public class BreakfastAndSupperModel
    {
        public DateTime OnDate { get; set; }

        public int BreakfastMealQuantity { get; set; }

        public double PricePerBreakfast { get; set; }

        public double BreakfastAmount { get; set; }

        public int SupperMealQuantity { get; set; }

        public double PricePerSupper { get; set; }

        public double SupperAmount { get; set; }

        public double TotalAmount
        {
            get
            {
                return BreakfastAmount + SupperAmount;
            }
        }
    }
}