namespace NexFB.Criteria.SetMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update status of set menu criteria.
    /// </summary>
    public class UpdateStatusSetMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FoodSetID")]
        public int SetMenuId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_EditActiveByFoodSetID";
        }

        public override bool IsValid()
        {
            return SetMenuId > 0;
        }
    }
}
