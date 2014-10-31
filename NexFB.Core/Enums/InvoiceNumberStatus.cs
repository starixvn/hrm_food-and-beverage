// --------------------------------------------------------------------------
// <copyright file="InvoiceNumberStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.30">Created</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Invoice Number Status.
    /// </summary>
    public enum InvoiceNumberStatus
    {
        /// <summary>
        /// None item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// New order.
        /// </summary>
        [Description("Receive Order Food Invoice")]
        ReceiveOrderFoodInvoice = 1,

        /// <summary>
        /// Received Order.
        /// </summary>
        [Description("Deliver Order Food Invoice")]
        DeliverOrderFoodInvoice = 2,
    }
}
