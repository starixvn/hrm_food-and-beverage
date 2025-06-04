namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.AdminOrderHistoryReport;

    /// <summary>
    /// The interface of Admin order history service.
    /// </summary>
    public interface IAdminOrderHistoryService
    {
        AdminOrderHistoryReport GetAdminOrderHistoryReport(GetAdminOrderHistoryCriteria criteria);

        AdminOrderHistoryReport GetPagingReport(AdminOrderHistoryReport fullReport, int startIndex, int itemsPerPage);

        void ExportAdminOrderHistoryReport(AdminOrderHistoryReport report, string templatePath);
    }
}
