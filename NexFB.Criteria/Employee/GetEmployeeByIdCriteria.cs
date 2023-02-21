namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetEmployeeByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey() => "Employee_GetEmployeeBySysID";

        public override bool IsValid() => UserId > 0;
    }
}