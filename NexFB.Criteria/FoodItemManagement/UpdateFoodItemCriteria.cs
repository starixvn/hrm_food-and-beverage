// ----------------------------------------------------------------------
// <copyright file="UpdateFoodItemCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/24/2014 1:50:59 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update food item criteria.
    /// </summary>
    public class UpdateFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public string SetMenuId { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodEngName { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
