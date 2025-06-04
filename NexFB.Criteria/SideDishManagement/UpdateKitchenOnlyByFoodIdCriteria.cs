namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Edit kitchen only by food Id criteria.
    /// </summary>
    public class UpdateKitchenOnlyByFoodIdCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool KitchenOnly { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditKitchenOnlyByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
