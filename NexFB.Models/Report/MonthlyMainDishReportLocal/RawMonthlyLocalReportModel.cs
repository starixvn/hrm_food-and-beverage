// ----------------------------------------------------------------------
// <copyright file="RawMonthlyLocalReportModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 9:49:33 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The raw monthly local report model.
    /// </summary>
    public class RawMonthlyLocalReportModel
    {
        public MealModel Meal { get; set; }

        public Employee Employee { get; set; }

        public EmployeeTypeModel EmployeeType { get; set; }

        public Department Department { get; set; }

        public Team Team { get; set; }
    }
}
