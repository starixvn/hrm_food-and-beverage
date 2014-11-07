// --------------------------------------------------------------------------
// <copyright file="EmployeeMappingData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.13">Created</change>
// </history>

namespace NexFB.Pos.Core.Models
{
    using System.Collections.Generic;
    using NexFB.Models.Kitchen;

    /// <summary>
    /// Employee Mapping Data.
    /// </summary>
    public class EmployeeMappingData
    {
        public int SysId { get; set; }

        public List<OrderMenu> ListOrderDetails { get; set; }
    }
}
