// --------------------------------------------------------------------------
// <copyright file="GetEmployeeRegistrationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.4">Created</change>
// </history>

namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeRegistrationCriteria : CriteriaBase
    {
        public DateTime WorkDate { get; set; }

        public int? MealId { get; set; }

        public int? MenuId { get; set; }

        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_RegistrationMealDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
