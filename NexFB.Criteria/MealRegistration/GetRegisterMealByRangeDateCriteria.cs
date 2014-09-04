// ----------------------------------------------------------------------
// <copyright file="GetRegisterMealByRangeDateCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 10:42:18 AM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get register meal by range date criteria.
    /// </summary>
    public class GetRegisterMealByRangeDateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public short? MealId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_GetByRangeDate";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
