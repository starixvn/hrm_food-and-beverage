// --------------------------------------------------------------------------
// <copyright file="CardState.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.SmartCardHelper
{
    using System.ComponentModel;

    /// <summary>
    /// Card State.
    /// </summary>
    internal enum CardState
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// The Ignore item.
        /// </summary>
        [Description("Ignore")]
        Ignore = 1,

        /// <summary>
        /// The Changed item.
        /// </summary>
        [Description("Changed")]
        Changed = 2,

        /// <summary>
        /// The Unknown item.
        /// </summary>
        [Description("Unknown")]
        Unknown = 4,

        /// <summary>
        /// The Unavailable item.
        /// </summary>
        [Description("Unavailable")]
        Unavailable = 8,

        /// <summary>
        /// The Empty item.
        /// </summary>
        [Description("Empty")]
        Empty = 16,

        /// <summary>
        /// The Present item.
        /// </summary>
        [Description("Present")]
        Present = 32,

        /// <summary>
        /// The AttributeMatch item.
        /// </summary>
        [Description("AttributeMatch")]
        AttributeMatch = 64,

        /// <summary>
        /// The Exclusive item.
        /// </summary>
        [Description("Exclusive")]
        Exclusive = 128,

        /// <summary>
        /// The InUse item.
        /// </summary>
        [Description("InUse")]
        InUse = 256,

        /// <summary>
        /// The Mute item.
        /// </summary>
        [Description("Mute")]
        Mute = 512,

        /// <summary>
        /// The Unpowered item.
        /// </summary>
        [Description("Unpowered")]
        Unpowered = 1024
    }
}
