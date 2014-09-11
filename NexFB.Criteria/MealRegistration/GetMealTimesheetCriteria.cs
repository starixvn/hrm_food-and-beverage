// ----------------------------------------------------------------------
// <copyright file="GetMealTimesheetCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/11/2014 9:59:25 AM">Created.</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get meal timesheet criteria.
    /// </summary>
    public class GetMealTimesheetCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Meal_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
