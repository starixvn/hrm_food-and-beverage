namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ChangePassword Criteria.
    /// </summary>
    public class ChangePasswordCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public string Password { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_ChangePassword";
        }

        public override bool IsValid()
        {
            return UserId > 0 && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
