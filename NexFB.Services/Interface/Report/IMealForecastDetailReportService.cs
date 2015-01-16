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
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MealForecastReport;

    /// <summary>
    /// The interface of meal forecast detail report service.
    /// </summary>
    public interface IMealForecastDetailReportService
    {
        MealForecastDetailHistoryReport GetMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria);

        MealForecastDetailHistoryReport GetPagingReport(MealForecastDetailHistoryReport fullReport, int startIndex, int itemsPerPage);

        void ExportMealForecastDetailReport(MealForecastDetailHistoryReport report, string templatePath, bool? isVegetarian);
    }
}