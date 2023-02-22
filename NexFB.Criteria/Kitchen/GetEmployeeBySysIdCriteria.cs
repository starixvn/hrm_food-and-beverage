namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetEmployeeBySysIdCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid() => SysId > 0;

        public override string GetSettingKey() => "Employee_GetEmployeeBySysID";
    }
}