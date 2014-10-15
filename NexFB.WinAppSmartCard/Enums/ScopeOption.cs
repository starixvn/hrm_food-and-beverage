// --------------------------------------------------------------------------
// <copyright file="ScopeOption.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Scope Option.
    /// </summary>
    public enum ScopeOption
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// The Terminal item.
        /// </summary>
        [Description("Terminal")]
        Terminal = 1,

        /// <summary>
        /// The System item.
        /// </summary>
        [Description("System")]
        System = 2
    }
}
