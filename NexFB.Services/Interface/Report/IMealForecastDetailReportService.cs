// -----------------------------------------------------------------------
// <copyright file="IMealForecastDetailReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using System;
    using System.Collections.Generic;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MealForecastReport;

    /// <summary>
    /// The interface of meal forecast detail report service.
    /// </summary>
    public interface IMealForecastDetailReportService
    {
        MealForecastDetailReport GetMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria);

        EmployeeRegistrationReportModel GetEmployeeRegistration(GetEmployeeRegistrationCriteria criteria);
        EmployeeRegistrationReportModel GetEmployeeNonRegistration(GetEmployeeNonRegistrationCriteria criteria);

        MealForecastDetailReport GetPagingReport(MealForecastDetailReport fullReport, int startIndex, int itemsPerPage);

        EmployeeRegistrationReportModel GetPagingEmployeeRegistration(
           EmployeeRegistrationReportModel fullReport,
           int startIndex,
           int itemsPerPage);
        EmployeeRegistrationReportModel GetPagingEmployeeNonRegistration(
          EmployeeRegistrationReportModel fullReport,
          int startIndex,
          int itemsPerPage);

        void ExportMealForecastDetailReport(MealForecastDetailReport report, string templatePath, bool? isVegetarian, DateTime dateSelected);

        void ExportEmployeeRegistrationReport(
            EmployeeRegistrationReportModel report,
            string templatePath,
            bool? isVegetarian,
            DateTime dateSelected);
        void ExportEmployeeNonRegistrationReport(
            EmployeeRegistrationReportModel report,
            string templatePath,
            bool? isVegetarian,
            DateTime dateSelected);
    }
}