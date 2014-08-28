// ----------------------------------------------------------------------
// <copyright file="Role.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 1:45:06 PM">Created.</change>
// </history>

namespace NexFB.Models.Entities
{
    /// <summary>
    /// The Role entity.
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public Department Department { get; set; }
    }
}
