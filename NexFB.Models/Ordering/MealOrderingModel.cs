// --------------------------------------------------------------------------
// <copyright file="MealOrderingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Ordering
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// MealOrdering Model.
    /// </summary>
    public class MealOrderingModel
    {
        public MealOrderingModel()
        {
            Members = new List<OrderMemberModel>();
        }

        public int OrderId { get; set; }

        public IList<OrderMemberModel> Members { get; set; }

        public int MemberCount { get; set; }

        public Employee CreatorOrder { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public Location Location { get; set; }

        public Status Status { get; set; }

        public DateTime ModifiedDate { get; set; }

        public Menu MyMeal { get; set; }

        public IList<MenuQuantityModel> MenuQuantity { get; set; }

        public bool IsSavedOrder { get; set; }

        public Employee CurrentUser { get; set; }
    }
}
