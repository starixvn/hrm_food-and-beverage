// ----------------------------------------------------------------------
// <copyright file="GetRegisterMealSettingCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/21/2014 4:33:07 PM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get setting of register meal criteria.
    /// </summary>
    public class GetRegisterMealSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "RegisterMeal_Setting";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
