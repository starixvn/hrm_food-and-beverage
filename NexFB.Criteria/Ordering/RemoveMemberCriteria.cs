namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// RemoveMember Criteria.
    /// </summary>
    public class RemoveMemberCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderUser_RemoveMember";
        }

        public override bool IsValid()
        {
            return UserId > 0 && MemberId > 0;
        }
    }
}
