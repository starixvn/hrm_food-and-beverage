namespace NexFB.Models.FoodMenuManagement
{
    using System;

    public class InsertFoodMenuHistoryModel
    {
        public int MenuId { get; set; }

        public DateTime CurrentEndDate { get; set; }

        public double CurrentPricePerMeal { get; set; }

        public double CurrentPricePerDay { get; set; }

        public double NewPricePerMeal { get; set; }

        public double NewPricePerDay { get; set; }
    }
}