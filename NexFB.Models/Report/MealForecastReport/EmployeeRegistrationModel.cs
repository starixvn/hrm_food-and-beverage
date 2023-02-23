// --------------------------------------------------------------------------
// <copyright file="EmployeeRegistrationModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.4">Created</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using NexFB.Models.Entities;

    [Serializable]
    public class EmployeeRegistrationModel
    {
        public DateTime WorkDate { get; set; }

        public bool? IsVegetarian { get; set; }

        public int ActualOrder { get; set; }

        public string CompanyName { get; set; }

        public string ShiftName { get; set; }

        public MealModel Meal { get; set; }

        public Employee Employee { get; set; }
    }
}
