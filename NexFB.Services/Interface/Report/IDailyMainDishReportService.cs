namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;

    /// <summary>
    /// The interface of Daily Main Dish Report service.
    /// </summary>
    public interface IDailyMainDishReportService
    {
        DailyMainDishReportModel GetDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria);

        DailyMainDishReportModel GetPagingReport(DailyMainDishReportModel report, int startIndex, int itemsPerPage);

        void ExportDailyMainDishReport(DailyMainDishReportModel report, string templatePath);
    }
}
