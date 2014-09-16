// --------------------------------------------------------------------------
// <copyright file="GetMyOrderByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.15">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMyOrderById Criteria.
    /// </summary>
    public class GetMyOrderByIdCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MyOrderByOrderID";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
