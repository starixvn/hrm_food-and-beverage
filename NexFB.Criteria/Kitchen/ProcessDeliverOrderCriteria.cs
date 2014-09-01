// --------------------------------------------------------------------------
// <copyright file="ProcessDeliverOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Process Deliver Order Criteria.
    /// </summary>
    public class ProcessDeliverOrderCriteria : CriteriaBase
    {
        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_DeliverOrder";
        }
    }
}
