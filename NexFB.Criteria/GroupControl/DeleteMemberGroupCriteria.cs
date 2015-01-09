// ----------------------------------------------------------------------
// <copyright file="DeleteMemberGroupCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="1/9/2015 9:58:55 AM">Created.</change>
// </history>

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
