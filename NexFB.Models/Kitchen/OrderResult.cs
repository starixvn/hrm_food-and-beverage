// --------------------------------------------------------------------------
// <copyright file="OrderResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.13">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    using NexFB.Core.Models;

    /// <summary>
    /// Order Result.
    /// </summary>
    public class OrderResult
    {
        public CommandResult CommandResult { get; set; }

        public int OrderId { get; set; }

        public string OrderNo { get; set; }
    }
}
