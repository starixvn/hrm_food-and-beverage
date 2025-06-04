namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update status of food item criteria.
    /// </summary>
    public class UpdateStatusFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditActiveByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
