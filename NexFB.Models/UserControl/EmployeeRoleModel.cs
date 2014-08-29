// ----------------------------------------------------------------------
// <copyright file="EmployeeRoleModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 3:03:42 PM">Created.</change>
// </history>

namespace NexFB.Models.UserControl
{
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// The Employee Role model.
    /// </summary>
    public class EmployeeRoleModel
    {
        public int SystemId { get; set; }

        public Role Role { get; set; }
    }
}
