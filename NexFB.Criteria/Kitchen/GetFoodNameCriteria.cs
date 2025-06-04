namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Food Name Criteria.
    /// </summary>
    public class GetFoodNameCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_FoodOrder";
        }
    }
}
