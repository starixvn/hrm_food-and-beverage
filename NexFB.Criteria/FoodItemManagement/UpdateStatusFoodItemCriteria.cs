// --------------------------------------------------------------------------
// <copyright file="UpdateStatusFoodItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.27">Created</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update status of food item criteria.
    /// </summary>
    public class UpdateStatusFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditActiveByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
