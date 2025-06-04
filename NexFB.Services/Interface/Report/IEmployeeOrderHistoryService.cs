namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.EmployeeOrderHistory;

    /// <summary>
    /// The interface of Employee order history service.
    /// </summary>
    public interface IEmployeeOrderHistoryService
    {
        EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(GetEmployeeOrderHistoryCriteria criteria);

        EmployeeOrderHistoryReport GetPagingReport(EmployeeOrderHistoryReport fullReport, int startIndex, int itemsPerPage);

        void ExportEmployeeOrderHistoryReport(EmployeeOrderHistoryReport report, string templatePath);
    }
}
