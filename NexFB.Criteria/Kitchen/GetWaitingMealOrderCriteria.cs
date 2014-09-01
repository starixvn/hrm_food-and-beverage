// --------------------------------------------------------------------------
// <copyright file="GetWaitingMealOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Waiting Meal Order Criteria.
    /// </summary>
    public class GetWaitingMealOrderCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_ListRequestWaitingByPOS";
        }
    }
}
