namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate side dish criteria.
    /// </summary>
    public class ActivateSideDishCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_EditActiveByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
