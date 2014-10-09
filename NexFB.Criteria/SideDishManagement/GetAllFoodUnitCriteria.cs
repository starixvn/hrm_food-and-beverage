// ----------------------------------------------------------------------
// <copyright file="GetAllFoodUnitCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 1:41:06 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food unit criteria.
    /// </summary>
    public class GetAllFoodUnitCriteria : CriteriaBase
    {
        public int? FoodUnitId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodUnit_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
