namespace NexFB.Services.Interface.Report
{
    using System;
    using Models.Report.MonthlyMealPriceReport;

    public interface IMonthlyMealTotalAmountService
    {
        LunchAndDinnerTotalAmountReportModel GetMonthlyLunchAndDinnerReport(DateTime reportDate, string companyId);

        void ExportMonthlyLunchAndDinnerReport(string templateLink, DateTime reportDate, string companyId);

        BreakfastAndSupperTotalAmountReportModel GetMonthlyBreakfastAndSupperReport(DateTime reportDate, string companyId);

        void ExportMonthlyBreakfastAndSupperReport(string templateLink, DateTime reportDate, string companyId);
    }
}