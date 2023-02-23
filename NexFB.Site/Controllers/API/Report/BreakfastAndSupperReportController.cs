namespace NexFB.Site.Controllers.API
{
    using System;
    using NexFB.Models.Report.MonthlyMealPriceReport;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface.Report;

    public class BreakfastAndSupperReportController : BaseController
    {
        private readonly IMonthlyMealTotalAmountService reportService;

        public BreakfastAndSupperReportController() : this(null)
        {
        }

        public BreakfastAndSupperReportController(IMonthlyMealTotalAmountService reportService)
        {
            this.reportService = reportService ?? new MonthlyMealTotalAmountService();
        }

        public BreakfastAndSupperTotalAmountReportModel GetData(string companyId, DateTime reportDate)
        {
            return reportService.GetMonthlyBreakfastAndSupperReport(reportDate, companyId);
        }
    }
}