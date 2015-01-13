// --------------------------------------------------------------------------
// <copyright file="GetOrderByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.12">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetOrderById Criteria.
    /// </summary>
    public class GetOrderByIdCriteria : CriteriaBase
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
