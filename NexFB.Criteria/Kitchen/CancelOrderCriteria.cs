// --------------------------------------------------------------------------
// <copyright file="CancelOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.10">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Cancel Order Criteria.
    /// </summary>
    public class CancelOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public override bool IsValid()
        {
            return OrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Cancel_OrderMeal";
        }
    }
}
