namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMenuByUserId Criteria.
    /// </summary>
    public class GetMenuByUserIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MenuBySysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
