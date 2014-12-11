// --------------------------------------------------------------------------
// <copyright file="DepartmentCode.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.8">Created</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Department Code.
    /// </summary>
    public enum DepartmentCode
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
        NX = 1,

        /// <summary>
        /// SMS company.
        /// </summary>
        [Description("SMS")]
        SMS = 2,

        /// <summary>
        /// Hong MA Company.
        /// </summary>
        [Description("HM")]
        HM = 3,
    }
}
