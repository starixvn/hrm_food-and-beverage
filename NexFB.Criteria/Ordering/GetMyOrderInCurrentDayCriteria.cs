namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMyOrderInCurrentDay Criteria.
    /// </summary>
    public class GetMyOrderInCurrentDayCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MyOrderSysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
