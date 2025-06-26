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
