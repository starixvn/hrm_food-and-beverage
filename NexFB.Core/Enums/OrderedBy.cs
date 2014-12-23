// ----------------------------------------------------------------------
// <copyright file="OrderedBy.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/23/2014 1:59:04 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The Ordered By ENUM.
    /// </summary>
    public enum OrderedBy
    {
        /// <summary>
        /// The all item.
        /// </summary>
        [Description("All")]
        All = 0,

        /// <summary>
        /// The Me item.
        /// </summary>
        [Description("Me")]
        Me = 1,

        /// <summary>
        /// Admin item.
        /// </summary>
        [Description("Not Me")]
        NotMe = 2
    }
}
