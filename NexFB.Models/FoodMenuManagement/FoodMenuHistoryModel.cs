using System;

namespace NexFB.Models.FoodMenuManagement
{
    public class FoodMenuHistoryModel
    {
        public int MenuId { get; set; }

        public DateTime EffectedDateBegin { get; set; }

        public DateTime? EffectedDateEnd { get; set; }

        public double PricePerMeal { get; set; }

        public double PricePerDay { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool IsAllowedEdit
        {
            get
            {
                return EffectedDateEnd == null;
            }
        }
    }
}