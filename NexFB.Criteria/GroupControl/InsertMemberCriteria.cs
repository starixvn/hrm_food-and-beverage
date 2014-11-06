// --------------------------------------------------------------------------
// <copyright file="InsertMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

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
