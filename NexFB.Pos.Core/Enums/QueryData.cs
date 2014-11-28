// --------------------------------------------------------------------------
// <copyright file="QueryData.cs" company="Nexcel Solutions Vietnam">
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
    /// Query Data.
    /// </summary>
    public enum QueryData
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Get New Order.
        /// </summary>
        [Description("GetNewOrder")]
        GetNewOrder = 1,

        /// <summary>
        /// Get All Data.
        /// </summary>
        [Description("GetAllData")]
        GetAllData = 2,
    }
}
