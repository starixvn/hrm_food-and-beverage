// --------------------------------------------------------------------------
// <copyright file="GetSideFoodBySysIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Side Food By Sys Id Criteria.
    /// </summary>
    public class GetSideFoodBySysIdCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid()
        {
            return SysId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_FoodOrder";
        }
    }
}
