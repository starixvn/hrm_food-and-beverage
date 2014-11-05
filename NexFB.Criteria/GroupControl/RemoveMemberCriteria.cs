namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update food menu criteria.
    /// </summary>
    public class RemoveMemberCriteria : CriteriaBase
    {
        public int MemberId { get; set; }

        public int GroupId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_RemoveMember";
        }

        public override bool IsValid()
        {
            return GroupId > 0 && MemberId > 0;
        }
    }
}
