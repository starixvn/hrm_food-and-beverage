// --------------------------------------------------------------------------
// <copyright file="GetOrderDetailsByOrderNoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.3">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Order Details By Order No Criteria.
    /// </summary>
    public class GetOrderDetailsByOrderNoCriteria : CriteriaBase
    {
        public int OrderNo { get; set; }

        public override bool IsValid()
        {
            return OrderNo > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_OrderRequestByOrderNo";
        }
    }
}
