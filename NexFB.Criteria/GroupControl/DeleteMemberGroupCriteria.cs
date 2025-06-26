namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Remove member of group criteria.
    /// </summary>
    public class DeleteMemberGroupCriteria : CriteriaBase
    {
        public string MemberId { get; set; }

        public int GroupId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_RemoveMember";
        }

        public override bool IsValid()
        {
            return GroupId > 0 && !string.IsNullOrEmpty(MemberId);
        }
    }
}
