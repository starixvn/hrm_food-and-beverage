// --------------------------------------------------------------------------
// <copyright file="FoodOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.30">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Food Order.
    /// </summary>
    public class FoodOrder
    {
        public int FoodId { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodCode { get; set; }

        public string FoodEngName { get; set; }

        public string FoodPhoto { get; set; }

        public bool IsActive { get; set; }
    }
}
