namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMemberSavedOrder Criteria.
    /// </summary>
    public class GetMemberSavedOrderCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_OrderSaveOrderBySysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
