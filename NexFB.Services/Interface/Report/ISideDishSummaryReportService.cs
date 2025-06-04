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
