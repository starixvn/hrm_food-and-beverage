// --------------------------------------------------------------------------
// <copyright file="GetUserByGroupIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.1">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get User By Group Id Criteria.
    /// </summary>
    public class GetUserByGroupIdCriteria : CriteriaBase
    {
        public int GroupOrderId { get; set; }

        public override bool IsValid()
        {
            return GroupOrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_GroupUserByGroupID";
        }
    }
}
