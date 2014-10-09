// ----------------------------------------------------------------------
// <copyright file="InsertSideDishCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 1:19:39 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert side dish criteria.
    /// </summary>
    public class InsertSideDishCriteria : CriteriaBase
    {
        public string FoodCode { get; set; }

        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public int UnitId { get; set; }

        public double Price { get; set; }

        public string EmployeeTypeId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FoodCode) && !string.IsNullOrEmpty(FoodVnmName) && 
                UnitId > 0 && !string.IsNullOrEmpty(EmployeeTypeId);
        }
    }
}
