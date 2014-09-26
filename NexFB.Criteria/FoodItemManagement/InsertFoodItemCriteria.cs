// ----------------------------------------------------------------------
// <copyright file="InsertFoodItemCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 2:31:39 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert food item criteria.
    /// </summary>
    public class InsertFoodItemCriteria : CriteriaBase
    {
        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodPhoto { get; set; }

        public string MenuGroupId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Food_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FoodEngName) && !string.IsNullOrEmpty(FoodVnmName);
        }
    }
}
