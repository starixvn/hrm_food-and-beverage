// ----------------------------------------------------------------------
// <copyright file="MealItemMonthModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 8:49:13 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;

    /// <summary>
    /// The meal item in month model.
    /// </summary>
    [Serializable]
    public class MealItemMonthModel
    {
        public Employee Employee { get; set; }

        public EmployeeTypeModel EmployeeType { get; set; }

        public Company Company { get; set; }

        public Team Team { get; set; }

        public string CostCenter { get; set; }

        public MealDayOfMonthModel NumberMealsMonth { get; set; }
    }
}