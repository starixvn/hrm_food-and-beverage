namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate or deactivate group criteria.
    /// </summary>
    public class ActivateGroupCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_EditActiveGroup";
        }

        public override bool IsValid()
        {
            return GroupId > 0;
        }
    }
}
