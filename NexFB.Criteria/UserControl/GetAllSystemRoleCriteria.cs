namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all system role criteria.
    /// </summary>
    public class GetAllSystemRoleCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "SystemRole_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
