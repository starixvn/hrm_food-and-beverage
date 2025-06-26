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
