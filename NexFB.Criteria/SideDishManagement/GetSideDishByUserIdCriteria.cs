// ----------------------------------------------------------------------
// <copyright file="GetSideDishByUserIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/21/2014 2:13:30 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Side Dish By User Id Criteria.
    /// </summary>
    public class GetSideDishByUserIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_FoodOrder";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
