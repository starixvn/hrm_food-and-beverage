// ----------------------------------------------------------------------
// <copyright file="RoleDepartmentModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 3:05:29 PM">Created.</change>
// </history>

namespace NexFB.Models.UserControl
{
    using NexFB.Models.Entities;

    /// <summary>
    /// The Role Department model.
    /// </summary>
    public class RoleDepartmentModel
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public Department Department { get; set; }
    }
}
