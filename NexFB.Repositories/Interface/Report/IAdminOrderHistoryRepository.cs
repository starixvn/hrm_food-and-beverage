namespace NexFB.Repositories.Interface.Report
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.AdminOrderHistoryReport;

    /// <summary>
    /// The interface of Admin Order History repository.
    /// </summary>
    public interface IAdminOrderHistoryRepository
    {
        AdminOrderHistoryReport GetAdminOrderHistoryReport(ICriteria criteria);
    }
}
