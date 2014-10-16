// ----------------------------------------------------------------------
// <copyright file="GetAlternativeMenuByUserIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/16/2014 1:01:49 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get alternative menu by user Id criteria.
    /// </summary>
    public class GetAlternativeMenuByUserIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_FoodAlternative";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
