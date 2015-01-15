// -----------------------------------------------------------------------
// <copyright file="IMealForecastReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/7">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MealForecastReport;

    /// <summary>
    /// The interface of meal forecast report service.
    /// </summary>
    public interface IMealForecastReportService
    {
        MealForecastReportModel GetMealForecastReport(GetMealForecastReportCriteria criteria);

        MealForecastReportModel GetPagingReport(MealForecastReportModel fullReport, int startIndex, int itemsPerPage);

        void ExportMealForecastReport(MealForecastReportModel report, string templatePath, bool? isVegetarian);
    }
}