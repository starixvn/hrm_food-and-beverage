// --------------------------------------------------------------------------
// <copyright file="InsertOrderMealByUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.21">Created</change>
// </history>

namespace NexFB.Criteria.SmartCardOrder
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    /// <summary>
    /// Insert Order Meal By User Criteria.
    /// </summary>
    public class InsertOrderMealByUserCriteria : CriteriaBase
    {
        [Output]
        public int OrderNo { get; set; }

        [Output]
        public int OrderId { get; set; }

        public string ListDetail { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ListDetail);
        }

        public override string GetSettingKey()
        {
            return "POS_Insert_OrderMealByUser";
        }
    }
}
