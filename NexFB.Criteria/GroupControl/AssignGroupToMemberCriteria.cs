// ----------------------------------------------------------------------
// <copyright file="AssignGroupToMemberCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="1/22/2015 9:40:36 AM">Created.</change>
// </history>

namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Assign group to member criteria.
    /// </summary>
    public class AssignGroupToMemberCriteria : CriteriaBase
    {
        public AssignGroupToMemberCriteria()
        {
            GroupIds = string.Empty;

            SysIds = string.Empty;
        }

        public string MemberId { get; set; }

        [SpParam(Ignored = true)]
        public string SysIds { get; set; }

        [SpParam(Ignored = true)]
        public string GroupIds { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_AssignGroupToMember";
        }
        
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MemberId);
        }
    }
}
