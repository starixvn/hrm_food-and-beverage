// ----------------------------------------------------------------------
// <copyright file="UpdateMealPlanningCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/7/2014 1:17:06 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMealPlanning
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update meal planning criteria.
    /// </summary>
    public class UpdateMealPlanningCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public DateTime OnDate { get; set; }

        public string FoodId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodPlan_Edit";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
