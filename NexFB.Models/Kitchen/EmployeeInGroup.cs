// --------------------------------------------------------------------------
// <copyright file="EmployeeInGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.2">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    using System.Collections.Generic;

    /// <summary>
    /// Employee In Group.
    /// </summary>
    public class EmployeeInGroup
    {
        public GroupOrder GroupOrder { get; set; }

        public List<EmployeeInfo> Employees { get; set; }
    }
}
