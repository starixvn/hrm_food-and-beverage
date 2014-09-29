// ----------------------------------------------------------------------
// <copyright file="InsertFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/26/2014 2:36:27 PM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert food menu criteria.
    /// </summary>
    public class InsertFoodMenuCriteria : CriteriaBase
    {
        public int? MenuId { get; set; }

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

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MenuVnmName) && !string.IsNullOrEmpty(MenuEngName);
        }
    }
}
