namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Alternative Food Criteria.
    /// </summary>
    public class GetAlternativeFoodCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_FoodAlternative";
        }
    }
}
