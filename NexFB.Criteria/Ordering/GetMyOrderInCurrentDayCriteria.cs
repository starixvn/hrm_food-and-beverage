// --------------------------------------------------------------------------
// <copyright file="GetMyOrderInCurrentDayCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetMyOrderInCurrentDay Criteria.
    /// </summary>
    public class GetMyOrderInCurrentDayCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_MyOrderSysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
