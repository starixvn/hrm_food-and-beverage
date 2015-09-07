// --------------------------------------------------------------------------
// <copyright file="EmployeeRegistrationReportModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.4">Created</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class EmployeeRegistrationReportModel
    {
        public EmployeeRegistrationReportModel()
        {
            EmployeeRegistrations = new List<EmployeeRegistrationModel>();
            TotalMeal = new TotalMealForecastModel();
        }

        public IList<EmployeeRegistrationModel> EmployeeRegistrations { get; set; }

        public TotalMealForecastModel TotalMeal { get; set; }

        public int TotalItem { get; set; }
    }
}
