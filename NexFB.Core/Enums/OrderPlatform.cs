// ----------------------------------------------------------------------
// <copyright file="OrderPlatform.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/23/2014 2:10:08 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The Order Platform ENUM.
    /// </summary>
    public enum OrderPlatform
    {
        /// <summary>
        /// The all item.
        /// </summary>
        [Description("All")]
        All = 0,

        /// <summary>
        /// The Web item.
        /// </summary>
        [Description("Web")]
        Web = 1,

        /// <summary>
        /// The POS-Kitchen item.
        /// </summary>
        [Description("POS - Kitchen")]
        PosKitchen = 2,

        /// <summary>
        /// The POS-Employee item.
        /// </summary>
        [Description("POS - Employee")]
        PosEmployee = 3
    }
}
