namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete food item criteria.
    /// </summary>
    public class DeleteFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_DeleteByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
