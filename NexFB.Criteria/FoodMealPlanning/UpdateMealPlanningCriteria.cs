namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update meal planning criteria.
    /// </summary>
    public class UpdateMealPlanningCriteria : CriteriaBase
    {
        public string MenuId { get; set; }

        public DateTime OnDate { get; set; }

        public string FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public int? SetMenuId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_Multi_Edit";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
