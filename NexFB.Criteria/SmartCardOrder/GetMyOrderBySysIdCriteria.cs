// --------------------------------------------------------------------------
// <copyright file="GetMyOrderBySysIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.30">Created</change>
// </history>

namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get My Order By SysId Criteria.
    /// </summary>
    public class GetMyOrderBySysIdCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid()
        {
            return SysId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_MyOrderBySysID";
        }
    }
}
