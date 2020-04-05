namespace NexFB.Criteria.Ordering
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateMealOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public byte MealId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_EditMeal";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}