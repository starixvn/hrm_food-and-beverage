// --------------------------------------------------------------------------
// <copyright file="AddMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.18">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// RemoveMember Criteria.
    /// </summary>
    public class AddMemberCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public int MemberId { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderUser_InsertMember";
        }

        public override bool IsValid()
        {
            return UserId > 0 && MemberId > 0;
        }
    }
}
