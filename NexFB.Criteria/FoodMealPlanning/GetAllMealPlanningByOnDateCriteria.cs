namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all meal planning by on date criteria.
    /// </summary>
    public class GetAllMealPlanningByOnDateCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string MealId { get; set; }

        public string MenuId { get; set; }

        public string EmployeeTypeId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_GetMenuByOnDate";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}
