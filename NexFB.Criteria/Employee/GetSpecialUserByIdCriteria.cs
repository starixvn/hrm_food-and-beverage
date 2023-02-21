namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetSpecialUserByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey() => "Employee_Sel_SpecialEmp";

        public override bool IsValid() => UserId > 0;
    }
}