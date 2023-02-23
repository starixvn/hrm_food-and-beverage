namespace NexFB.Site.Controllers.API
{
    using System;
    using NexFB.Models.Report.MonthlyMealPriceReport;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface.Report;

    public class LunchAndDinnerReportController : BaseController
    {
        private readonly IMonthlyMealTotalAmountService reportService;

        public LunchAndDinnerReportController() : this(null)
        {
        }

        public LunchAndDinnerReportController(IMonthlyMealTotalAmountService reportService)
        {
            this.reportService = reportService ?? new MonthlyMealTotalAmountService();
        }

        public LunchAndDinnerTotalAmountReportModel GetData(string companyId, DateTime reportDate)
        {
            return reportService.GetMonthlyLunchAndDinnerReport(reportDate, companyId);
        }
    }
}