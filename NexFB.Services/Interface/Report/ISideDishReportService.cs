// --------------------------------------------------------------------------
// <copyright file="ISideDishReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.14">Created</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.SideDishReport;

    /// <summary>
    /// ISideDishReportService interface.
    /// </summary>
    public interface ISideDishReportService
    {
        SideDishReportModel GetSideDishReport(GetSideDishReportCriteria criteria);

        void ExportSideDishReport(SideDishReportModel report, string templatePath);
    }
}
