namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get food of meal planning by on date.
    /// </summary>
    public class GetFoodMealPlanningByOnDateCriteria : CriteriaBase
    {
        public DateTime OnDate { get; set; }

        public int MenuGroupId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_GetFoodByOnDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
