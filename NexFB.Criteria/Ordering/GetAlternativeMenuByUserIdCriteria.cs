namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get alternative menu by user Id criteria.
    /// </summary>
    public class GetAlternativeMenuByUserIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_FoodAlternative";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
