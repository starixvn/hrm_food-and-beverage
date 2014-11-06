// ----------------------------------------------------------------------
// <copyright file="GetGroupCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 2:24:11 PM">Created.</change>
// </history>

namespace NexFB.Criteria.GroupControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee role criteria.
    /// </summary>
    public class GetGroupCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        public string GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_GetGroup";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
