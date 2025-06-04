namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete side dish criteria.
    /// </summary>
    public class DeleteSideDishCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_DeleteByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
