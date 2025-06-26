namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get setting of register meal criteria.
    /// </summary>
    public class GetRegisterMealSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "RegisterMeal_Setting";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
