// --------------------------------------------------------------------------
// <copyright file="OrderMemberModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Ordering
{
    using NexFB.Models.Entities;

    /// <summary>
    /// OrderMember Model.
    /// </summary>
    public class OrderMemberModel
    {
        public Employee Member { get; set; }

        public Menu Menu { get; set; }

        public bool IsOrder { get; set; }
    }
}
