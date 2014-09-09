// --------------------------------------------------------------------------
// <copyright file="MenuQuantityModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.9">Created</change>
// </history>

namespace NexFB.Models.Ordering
{
    using NexFB.Models.Entities;

    /// <summary>
    /// MenuQuantity Model.
    /// </summary>
    public class MenuQuantityModel
    {
        public int Quantity { get; set; }

        public Menu Menu { get; set; }
    }
}
