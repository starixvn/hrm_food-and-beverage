namespace NexFB.Repositories.Interface.Report
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.EmployeeOrderHistory;

    /// <summary>
    /// The interface of Employee Order History repository.
    /// </summary>
    public interface IEmployeeOrderHistoryRepository
    {
        EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(ICriteria criteria);
    }
}
