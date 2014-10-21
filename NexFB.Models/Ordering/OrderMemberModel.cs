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
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// OrderMember Model.
    /// </summary>
    public class OrderMemberModel
    {
        public OrderMemberModel()
        {
            Menus = new List<Menu>();
        }

        public int OrderId { get; set; }

        public Employee Member { get; set; }

        public IList<Menu> Menus { get; set; }

        public bool IsOrder { get; set; }

        public Status CurrentStatus { get; set; }
    }
}
