namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get meal timesheet criteria.
    /// </summary>
    public class GetMealTimesheetCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Meal_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
