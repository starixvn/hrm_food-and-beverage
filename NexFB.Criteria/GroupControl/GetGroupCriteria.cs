namespace NexFB.Criteria.GroupControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee role criteria.
    /// </summary>
    public class GetGroupCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        public string GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_GetGroup";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
