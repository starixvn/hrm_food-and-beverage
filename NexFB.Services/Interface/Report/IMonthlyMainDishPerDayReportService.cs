namespace NexFB.Services.Interface.Report
{
    using System.Collections.Generic;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;

    /// <summary>
    /// The interface of Monthly Main Dish Per .
    /// </summary>
    public interface IMonthlyMainDishPerDayReportService
    {
        MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria);

        MonthlyMainDishPerDayModel GetPagingReport(MonthlyMainDishPerDayModel fullReport, int startIndex, int itemsPerPage);

        IList<BreakRuleOrder> GetBreakRuleOrder(GetBreakRuleOrderCriteria criteria);

        void ExportMonthlyMainDishPerDayReport(MonthlyMainDishPerDayModel report, string templatePath);
    }
}
