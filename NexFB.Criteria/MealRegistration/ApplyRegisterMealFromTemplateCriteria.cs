// ----------------------------------------------------------------------
// <copyright file="ApplyRegisterMealFromTemplateCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/15/2014">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Apply register meal from template criteria.
    /// </summary>
    public class ApplyRegisterMealFromTemplateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_ApplyFromTemplate";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
