// ----------------------------------------------------------------------
// <copyright file="UpdateFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/29/2014 12:33:49 PM">Created.</change>
// </history>

using System;
using Fanex.Data.Repository;

namespace NexFB.Criteria.GroupControl
{
    /// <summary>
    /// Update food menu criteria.
    /// </summary>
    public class UpdateGroupCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_EditGroup";
        }

        public override bool IsValid()
        {
            return GroupId > 0 && !string.IsNullOrEmpty(GroupName);
        }
    }
}
