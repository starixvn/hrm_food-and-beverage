// ----------------------------------------------------------------------
// <copyright file="GetBasicFoodItemCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/3/2014 2:23:49 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get basic information of food item criteria.
    /// </summary>
    public class GetBasicFoodItemCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        public string MenuGroupId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Food_GetName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
