namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete Special Employee criteria.
    /// </summary>
    public class DeleteSpecialUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        [SpParam(Ignored = true)]
        public int ModifiedBy { get; set; }

        [SpParam(Ignored = true)]
        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_Del_SpecialEmp";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
