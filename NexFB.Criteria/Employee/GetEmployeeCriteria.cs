namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetEmployeeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int? UserId { get; set; }

        public string CompanyId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "query")]
        public string Keyword { get; set; }

        public string RoleId { get; set; }

        public bool IsShowDisable { get; set; }

        public override string GetSettingKey() => "Employee_GetEmployees";

        public override bool IsValid() => true;
    }
}