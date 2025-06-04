namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee function criteria.
    /// </summary>
    public class GetAllEmployeeFunctionCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetAllFunctionBySysID";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
