// --------------------------------------------------------------------------
// <copyright file="MenuType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.28">Created</change>
// </history>

namespace NexFB.Pos.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Invoice Type.
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Main Menu.
        /// </summary>
        [Description("MainMenu")]
        MainMenu = 1,

        /// <summary>
        /// Alternative Menu.
        /// </summary>
        [Description("AlternativeMenu")]
        AlternativeMenu = 2,
    }
}
