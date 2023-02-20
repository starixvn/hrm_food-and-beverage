namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    public class GetAllEmployeeRoleCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "query")]
        public string Keyword { get; set; }

        public string RoleId { get; set; }

        public override string GetSettingKey() => "UserCtrl_GetEmployees";

        public override bool IsValid() => true;
    }
}