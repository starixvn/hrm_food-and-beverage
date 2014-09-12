// ----------------------------------------------------------------------
// <copyright file="SetRegisterMealByDefaultCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/12/2014 5:05:12 PM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Set register meal by default criteria.
    /// </summary>
    public class SetRegisterMealByDefaultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public string DayOfWeek { get; set; }

        public string Meal { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_SetDefault";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
