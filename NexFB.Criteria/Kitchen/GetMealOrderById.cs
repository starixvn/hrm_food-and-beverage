// --------------------------------------------------------------------------
// <copyright file="GetMealOrderById.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.15">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Meal OrderBy Id.
    /// </summary>
    public class GetMealOrderById : CriteriaBase
    {
        public int OrderId { get; set; }

        public override bool IsValid()
        {
            return OrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_OrderRequestByOrderID";
        }
    }
}
