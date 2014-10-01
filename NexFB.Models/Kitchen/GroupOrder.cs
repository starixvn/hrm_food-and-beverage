// --------------------------------------------------------------------------
// <copyright file="GroupOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.30">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Group Order.
    /// </summary>
    public class GroupOrder
    {
        public int GroupOrderId { get; set; }

        public int DepartmentId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int LocationId { get; set; }

        public string LocationEngName { get; set; }

        public string LocationVnmName { get; set; }

        public string FloorName { get; set; }
    }
}
