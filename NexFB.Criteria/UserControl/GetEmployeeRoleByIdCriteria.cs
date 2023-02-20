namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    public class GetEmployeeRoleByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public string RoleId { get; set; }

        public override string GetSettingKey() => "UserCtrl_GetEmployeeBySysId";

        public override bool IsValid() => SystemId > 0;
    }
}