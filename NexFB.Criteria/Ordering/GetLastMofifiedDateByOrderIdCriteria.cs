// ----------------------------------------------------------------------
// <copyright file="GetLastMofifiedDateByOrderIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/6/2014 10:10:10 AM">Created.</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get last modified date by order Id criteria.
    /// </summary>
    public class GetLastMofifiedDateByOrderIdCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_ModifiedByOrderID";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
