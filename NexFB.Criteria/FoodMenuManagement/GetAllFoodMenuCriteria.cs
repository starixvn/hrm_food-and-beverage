// ----------------------------------------------------------------------
// <copyright file="GetAllFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 11:01:00 AM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food menu criteria.
    /// </summary>
    public class GetAllFoodMenuCriteria : CriteriaBase
    {
        public int? MenuId { get; set; }

        public string MealId { get; set; }

        public string EmployeeTypeId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
