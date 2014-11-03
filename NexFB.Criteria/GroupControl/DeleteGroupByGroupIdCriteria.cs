// ----------------------------------------------------------------------
// <copyright file="DeleteFoodMenuByMenuIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/29/2014 11:08:42 AM">Created.</change>
// </history>

using System;
using Fanex.Data.Repository;

namespace NexFB.Criteria.GroupControl
{
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
