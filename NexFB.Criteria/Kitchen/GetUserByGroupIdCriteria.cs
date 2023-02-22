namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetUserByGroupIdCriteria : CriteriaBase
    {
        public int GroupOrderId { get; set; }

        public override bool IsValid() => GroupOrderId > 0;

        public override string GetSettingKey() => "POS_Get_GroupUserByGroupID";
    }
}