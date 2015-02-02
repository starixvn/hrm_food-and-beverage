// -----------------------------------------------------------------------
// <copyright file="ISideDishSummaryReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/30">Created</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.SideDishSummaryReport;

    /// <summary>
    /// The interface of Side dish summary report service.
    /// </summary>
    public interface ISideDishSummaryReportService
    {
        SideDishSummaryReportModel GetSideDishSummaryReport(GetSideDishSummaryReportCriteria criteria);

        void ExportSideDishSummaryReport(SideDishSummaryReportModel report, string templatePath);
    }
}