namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetEmployeeInGroupBySysIdCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid() => SysId > 0;

        public override string GetSettingKey() => "POS_Get_GroupUserBySysID";
    }
}