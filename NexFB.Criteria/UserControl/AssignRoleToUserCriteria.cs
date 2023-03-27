namespace NexFB.Criteria.UserControl
{
    using System;
    using Fanex.Data.Repository;

    public class AssignRoleToUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public string RoleId { get; set; }

        public string RoleCompanies { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey() => "UserCtrl_SetEmployee";

        public override bool IsValid() => SystemId > 0;
    }
}