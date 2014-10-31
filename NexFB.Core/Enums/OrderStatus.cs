// --------------------------------------------------------------------------
// <copyright file="OrderStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.16">Created</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Order Status.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// None item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// New order.
        /// </summary>
        [Description("New Order")]
        NewOrder = 1,

        /// <summary>
        /// Received Order.
        /// </summary>
        [Description("ReceivedOrder")]
        ReceivedOrder = 2,

        /// <summary>
        /// Reduced order.
        /// </summary>
        [Description("ReducedOrder")]
        ReducedOrder = 3,

        /// <summary>
        /// Delivered order.
        /// </summary>
        [Description("DeliveredOrder")]
        DeliveredOrder = 4,

        /// <summary>
        /// Edited order.
        /// </summary>
        [Description("Edited Order")]
        EditedOrder = 5,
    }
}
