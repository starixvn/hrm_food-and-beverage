// ----------------------------------------------------------------------
// <copyright file="RolesSystem.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 12:35:09 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    public enum RolesSystem : int
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// System Admin item.
        /// </summary>
        [Description("SA")]
        SA = 1,

        /// <summary>
        /// Finance item.
        /// </summary>
        Finance = 2,

        /// <summary>
        /// Kitchen item.
        /// </summary>
        [Description("Kitchen")]
        Kitchen = 3,

        /// <summary>
        /// Employee item.
        /// </summary>
        [Description("Employee")]
        Employee = 4,

        /// <summary>
        /// Admin item.
        /// </summary>
        [Description("Admin")]
        Admin = 5
    }
}
