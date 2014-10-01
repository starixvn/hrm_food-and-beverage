// ----------------------------------------------------------------------
// <copyright file="UpdateFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/29/2014 12:33:49 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update food menu criteria.
    /// </summary>
    public class UpdateFoodMenuCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public string MenuVnmName { get; set; }

        public string MenuEngName { get; set; }

        public bool IsVegetarian { get; set; }

        public string Descriptions { get; set; }

        public double PricePerMeal { get; set; }

        public double PricePerDay { get; set; }

        public bool IsActive { get; set; }

        public string MealId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string FoodId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_EditByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0 && !string.IsNullOrEmpty(MenuVnmName) && !string.IsNullOrEmpty(MenuEngName) 
                        && !string.IsNullOrEmpty(MealId) && !string.IsNullOrEmpty(EmployeeTypeId);
        }
    }
}
