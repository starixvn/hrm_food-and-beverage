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
    using NexFB.Core.Constants;
    using NexFB.Models.Entities;

    /// <summary>
    /// MealOrdering Model.
    /// </summary>
    public class MealOrderingModel
    {
        public MealOrderingModel()
        {
            Members = new List<OrderMemberModel>();
            Location = new Location();
            CurrentMenus = new List<Menu>();
            IsShowMember = false;
        }

        public int OrderId { get; set; }

        public int OrderNo { get; set; }

        public IList<OrderMemberModel> Members { get; set; }

        public int MemberCount { get; set; }

        public Employee CreatorOrder { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public string OrderFrom { get; set; }

        public Location Location { get; set; }

        public Status Status { get; set; }

        public DateTime ModifiedDate { get; set; }

        public IList<Menu> CurrentMenus { get; set; }

        public IList<MenuQuantityModel> MenuQuantity { get; set; }

        public bool IsSavedOrder { get; set; }

        public Employee CurrentUser { get; set; }

        public string ModifiedTime
        {
            get
            {
                return ModifiedDate.ToString(Constants.TimeFormat);
            }
        }

        public bool IsShowMember { get; set; }

        public bool IsSideDishOrder { get; set; }
    }
}
