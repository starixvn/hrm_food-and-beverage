namespace NexFB.Repositories.Interface
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Models.Report.SideDishSummaryReport;
    using NexFB.Models.Report.ViewMenuReport;

    /// <summary>
    /// The interface of Report repository.
    /// </summary>
    public interface IReportRepository
    {
        MonthlyLocalReportModel GetMonthlyMainDishReportLocal(ICriteria criteria);

        DailyMainDishReportModel GetDailyMainDishReport(ICriteria criteria);

        MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(ICriteria criteria);

        SideDishReportModel GetSideDishReport(ICriteria criteria, int? pageIndex = null, int? itemsPerPage = null);

        ViewMenuReportModel GetViewMenuReport(ICriteria criteria);

        SideDishSummaryReportModel GetSideDishSummaryReport(ICriteria criteria, int? pageIndex = null, int? itemsPerPage = null);
    }
}
