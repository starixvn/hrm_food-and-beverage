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
    using System.Collections.Generic;

    /// <summary>
    /// MealOrdering Model.
    /// </summary>
    public class MealOrderingModel
    {
        public IList<OrderMemberModel> Members { get; set; }

        public int CreatorOrder { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public short LocationId { get; set; }
    }
}
