namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert meal planning criteria.
    /// </summary>
    public class InsertMealPlanningCriteria : CriteriaBase
    {
        public string MenuId { get; set; }

        public DateTime OnDate { get; set; }

        public string FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public int? SetMenuId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_Multi_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
