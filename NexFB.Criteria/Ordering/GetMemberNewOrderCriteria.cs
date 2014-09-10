// --------------------------------------------------------------------------
// <copyright file="GetMemberNewOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMenuByUserId Criteria.
    /// </summary>
    public class GetMemberNewOrderCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MyStatusBySysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
