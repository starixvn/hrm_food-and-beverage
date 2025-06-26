namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete meal planning criteria.
    /// </summary>
    public class DeleteMealPlanningCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public DateTime OnDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_Delete";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
