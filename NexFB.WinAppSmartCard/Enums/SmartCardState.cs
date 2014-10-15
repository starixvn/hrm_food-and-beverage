// --------------------------------------------------------------------------
// <copyright file="SmartCardState.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Smart card State.
    /// </summary>
    public enum SmartcardState
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// The Inserted item.
        /// </summary>
        [Description("Inserted")]
        Inserted = 1,

        /// <summary>
        /// The Ejected item.
        /// </summary>
        [Description("Ejected")]
        Ejected = 2
    }
}
