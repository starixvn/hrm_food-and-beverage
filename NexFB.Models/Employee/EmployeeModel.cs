// ----------------------------------------------------------------------
// <copyright file="EmployeeModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 1:07:36 PM">Created.</change>
// </history>

namespace NexFB.Models.Employee
{
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// The employee model.
    /// </summary>
    public class EmployeeModel : Employee
    {
        public EmployeeModel()
        {
            RoleItems = new List<Role>();
        }

        public Department Department { get; set; }

        public Team Team { get; set; }

        public List<Role> RoleItems { get; set; }
    }
}
