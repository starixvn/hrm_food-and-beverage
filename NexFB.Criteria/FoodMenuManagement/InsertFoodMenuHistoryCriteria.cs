namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    public class InsertFoodMenuHistoryCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public DateTime EffectedDateEnd { get; set; }

        public double CurrentPricePerMeal { get; set; }

        public double CurrentPricePerDay { get; set; }

        public double NewPricePerMeal { get; set; }

        public double NewPricePerDay { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey() => "FoodMenu_InsertFoodMenuHistory";

        public override bool IsValid() => MenuId > 0 && ModifiedBy > 0 && IsValidCurrentPrice() && IsValidNewPrice();

        private bool IsValidCurrentPrice() => CurrentPricePerMeal > 0 || CurrentPricePerDay > 0;

        private bool IsValidNewPrice() => NewPricePerMeal > 0 || NewPricePerDay > 0;
    }
}