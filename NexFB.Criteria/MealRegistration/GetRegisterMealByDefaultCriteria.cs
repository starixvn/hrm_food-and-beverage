// ----------------------------------------------------------------------
// <copyright file="GetRegisterMealByDefaultCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/12/2014 3:01:58 PM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get register meal by default criteria.
    /// </summary>
    public class GetRegisterMealByDefaultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_GetDefault";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
