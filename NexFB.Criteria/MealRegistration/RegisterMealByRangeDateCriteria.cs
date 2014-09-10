// ----------------------------------------------------------------------
// <copyright file="RegisterMealByRangeDateCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/10/2014 10:52:33 AM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Register meal by range date criteria.
    /// </summary>
    public class RegisterMealByRangeDateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public string Meal { get; set; }

        public string OnDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_SetByRangeDate";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
