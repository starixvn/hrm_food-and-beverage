namespace NexFB.Criteria.MealTimeManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update meal time criteria.
    /// </summary>
    public class UpdateMealTimeCriteria : CriteriaBase
    {
        public int MealId { get; set; }

        public string RegisterTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Meal_EditTime";
        }

        public override bool IsValid()
        {
            return MealId > 0;
        }
    }
}
