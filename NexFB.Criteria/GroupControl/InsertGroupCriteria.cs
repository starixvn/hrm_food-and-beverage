// ----------------------------------------------------------------------
// <copyright file="InsertFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/26/2014 2:36:27 PM">Created.</change>
// </history>

namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert food menu criteria.
    /// </summary>
    public class InsertGroupCriteria : CriteriaBase
    {
        public int? GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupOrderAdmin_InsertGroup";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(GroupName);
        }
    }
}
