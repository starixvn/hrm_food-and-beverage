namespace NexFB.Services.Interface.Report
{
    using System;
    using Models.Report.MonthlyMealPriceReport;

    public interface IMonthlyMealTotalAmountService
    {
        LunchAndDinnerTotalAmountReportModel GetMonthlyLunchAndDinnerReport(DateTime reportDate, string departmentId);

        void ExportMonthlyLunchAndDinnerReport(string templateLink, DateTime reportDate, string departmentId);

        BreakfastAndSupperTotalAmountReportModel GetMonthlyBreakfastAndSupperReport(DateTime reportDate, string departmentId);

        void ExportMonthlyBreakfastAndSupperReport(string templateLink, DateTime reportDate, string departmentId);
    }
}