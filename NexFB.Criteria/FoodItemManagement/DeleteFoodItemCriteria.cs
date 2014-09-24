// ----------------------------------------------------------------------
// <copyright file="DeleteFoodItemCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/24/2014 4:47:58 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete food item criteria.
    /// </summary>
    public class DeleteFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_DeleteByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
