// --------------------------------------------------------------------------
// <copyright file="InsertOrderSideDishesByUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.23">Created</change>
// </history>

namespace NexFB.Criteria.SmartCardOrder
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    /// <summary>
    /// Insert Order Side Dishes By User Criteria.
    /// </summary>
    public class InsertOrderSideDishesByUserCriteria : CriteriaBase
    {
        [Output]
        public int OrderNo { get; set; }

        [Output]
        public int OrderId { get; set; }

        public string ListDetail { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public bool TakeAway { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ListDetail);
        }

        public override string GetSettingKey()
        {
            return "POS_Insert_OrderSideDishByUser";
        }
    }
}
