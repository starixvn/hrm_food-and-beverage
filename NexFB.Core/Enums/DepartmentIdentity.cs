// --------------------------------------------------------------------------
// <copyright file="DepartmentIdentity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.1">Created</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Department ENUM.
    /// </summary>
    public enum DepartmentIdentity
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// NEXCEL Solutions company.
        /// </summary>
        [Description("Nexcel")]
        Nexcel = 1,

        /// <summary>
        /// SMS company.
        /// </summary>
        [Description("Sms")]
        Sms = 2,

        /// <summary>
        /// Hong MA Company.
        /// </summary>
        [Description("Hong Ma")]
        HongMA = 3,

        [Description("Luminet")]
        Luminet = 4,
    }
}
