namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert group criteria.
    /// </summary>
    public class InsertGroupCriteria : CriteriaBase
    {
        public int? GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_InsertGroup";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(GroupName);
        }
    }
}
