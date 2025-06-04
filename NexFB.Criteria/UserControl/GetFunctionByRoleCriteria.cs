namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all function by role criteria.
    /// </summary>
    public class GetFunctionByRoleCriteria : CriteriaBase
    {
        public int? RoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Function_GetByRole";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
