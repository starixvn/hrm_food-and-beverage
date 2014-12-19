// --------------------------------------------------------------------------
// <copyright file="SpecialUser.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.17">Created</change>
// </history>

namespace NexFB.Models.Employee
{
    using NexFB.Models.Entities;

    /// <summary>
    /// The Special User.
    /// </summary>
    public class SpecialUser : Employee
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string EmployeeTypeName { get; set; }

        public bool IsActive { get; set; }
    }
}
