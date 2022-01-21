namespace NexFB.Services.Interface.Report
{
    using System;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.ViewMenuReport;

    public interface IViewMenuReportService
    {
        ViewMenuReportModel GetViewMenuReport(GetViewMenuReportCriteria criteria);

        void ExportViewMenuReport(ViewMenuReportModel report, string templatePath, DateTime fromDate, DateTime toDate);
    }
}