namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberNewOrder Criteria.
    /// </summary>
    public class GetMemberNewOrderCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MyStatusBySysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
