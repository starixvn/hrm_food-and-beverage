// ----------------------------------------------------------------------
// <copyright file="GetAllFoodItemCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 8:57:44 AM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food item management criteria.
    /// </summary>
    public class GetAllFoodItemCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public string SetMenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Food_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
