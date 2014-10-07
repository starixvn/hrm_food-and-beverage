// ----------------------------------------------------------------------
// <copyright file="DeleteMealPlanningCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/7/2014 3:12:31 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete meal planning criteria.
    /// </summary>
    public class DeleteMealPlanningCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public DateTime OnDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_Delete";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
