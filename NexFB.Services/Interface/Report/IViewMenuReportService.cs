// --------------------------------------------------------------------------
// <copyright file="IViewMenuReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.19">Created</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using System;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.ViewMenuReport;

    /// <summary>
    /// IViewMenuReportService interface.
    /// </summary>
    public interface IViewMenuReportService
    {
        ViewMenuReportModel GetViewMenuReport(GetViewMenuReportCriteria criteria);

        void ExportViewMenuReport(ViewMenuReportModel report, string templatePath, DateTime startDate, DateTime endDate);
    }
}