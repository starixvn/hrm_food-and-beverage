namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get employee menu by system Id criteria.
    /// </summary>
    public class GetEmployeeMenuBySystemIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "UserCtrl_GetMenuFunctionBySysID";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
