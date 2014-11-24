// --------------------------------------------------------------------------
// <copyright file="EmployeeOrderStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.24">Created</change>
// </history>

namespace NexFB.Models.SmartCardOrder
{
    /// <summary>
    /// Order Status.
    /// </summary>
    public class EmployeeOrderStatus
    {
        public int MealId { get; set; }

        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }

        public bool IsAllowOrder { get; set; }
    }
}
