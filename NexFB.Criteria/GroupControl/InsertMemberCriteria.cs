namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert member criteria.
    /// </summary>
    public class InsertMemberCriteria : CriteriaBase
    {
        public string MemberId { get; set; }

        public int GroupId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_InsertMember";
        }

        public override bool IsValid()
        {
            return GroupId > 0;
        }
    }
}
