namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate or deactivate user criteria.
    /// </summary>
    public class ActivateSpecialUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public int ModifiedBy { get; set; }

        [SpParam(Ignored = true)]
        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_Upd_Actived_SpecialEmp";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
