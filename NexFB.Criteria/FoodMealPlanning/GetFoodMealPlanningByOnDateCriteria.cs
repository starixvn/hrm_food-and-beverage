// ----------------------------------------------------------------------
// <copyright file="GetFoodMealPlanningByOnDateCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/8/2014 9:47:43 AM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get food of meal planning by on date.
    /// </summary>
    public class GetFoodMealPlanningByOnDateCriteria : CriteriaBase
    {
        public DateTime OnDate { get; set; }

        public int MenuGroupId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_GetFoodByOnDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
