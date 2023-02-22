namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetGroupNameCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "POS_Get_GroupName";
    }
}