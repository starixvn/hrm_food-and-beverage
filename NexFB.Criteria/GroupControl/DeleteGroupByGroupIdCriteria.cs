// --------------------------------------------------------------------------
// <copyright file="DeleteGroupByGroupIdCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Delete food menu by menu Id criteria.
    /// </summary>
    public class DeleteGroupByGroupIdCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_DeleteGroup";
        }

        public override bool IsValid()
        {
            return GroupId > 0;
        }
    }
}
