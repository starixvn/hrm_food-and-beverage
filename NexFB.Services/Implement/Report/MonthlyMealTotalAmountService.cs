namespace NexFB.Services.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Core.Utilities;
    using Criteria.Report;
    using Interface.Report;
    using Models.Report.MonthlyMealPriceReport;
    using Resources;

    public class MonthlyMealTotalAmountService : BaseService, IMonthlyMealTotalAmountService
    {
        public LunchAndDinnerTotalAmountReportModel GetMonthlyLunchAndDinnerReport(DateTime reportDate, string departmentId)
        {
            var report = new LunchAndDinnerTotalAmountReportModel();
            var mealsPrice = GetListMealPriceOnMonth(reportDate, departmentId);

            if (mealsPrice != null)
            {
                report.MealPriceList = mealsPrice;
                report.TotalLunchMeals = mealsPrice.Sum(m => m.LunchMealQuantity);
                report.MonthlyTotalLunchAmount = mealsPrice.Sum(m => m.LunchAmount);
                report.TotalDinnerMeals = mealsPrice.Sum(m => m.DinnerMealQuantity);
                report.MonthlyTotalDinnerAmount = mealsPrice.Sum(m => m.DinnerAmount);
            }

            return report;
        }

        private IEnumerable<LunchAndDinnerModel> GetListMealPriceOnMonth(DateTime reportDate, string departmentId)
        {
            var criteria = new GetMonthlyLunchAndDinnerReportCriteria
            {
                ReportDate = reportDate,
                DepartmentId = departmentId
            };
            return GetData<LunchAndDinnerModel>(criteria);
        }

        public void ExportMonthlyLunchAndDinnerReport(string templateLink, DateTime reportDate, string departmentId)
        {
            var reportName = Lang.MonthlyLunchAndDinnerTotalAmountReport;
            var writer = new ExcelTemplateWriter(templateLink, reportName);
            var report = GetMonthlyLunchAndDinnerReport(reportDate, departmentId);
            string reportTitle = string.Format("{0}{1}{2}", "MONTHLY MEAL RECORD OF ", reportDate.ToString("MMM yyyy"), " LUNCH & DINNER");
            // Fill report title.
            writer.WriteRow(0, new { reportTitle });
            writer.CopyRows(1, 2);

            FillLunchAndDinnerReportContentRow(report.MealPriceList, ref writer);
            FillTotalLunchAndDinerReportRow(report, writer);
            writer.SyncColumnsWidth(6);

            writer.Render();
        }

        private string ConvertToMoneyFormat(double number)
        {
            return string.Format("{0:#,###}", number);
        }

        private void FillTotalLunchAndDinerReportRow(LunchAndDinnerTotalAmountReportModel report, ExcelTemplateWriter writer)
        {
            writer.WriteRow(2, new object[]
            {
                Lang.Total,
                ConvertToMoneyFormat(report.TotalLunchMeals),
                ConvertToMoneyFormat(report.MonthlyTotalLunchAmount),
                ConvertToMoneyFormat(report.TotalDinnerMeals),
                ConvertToMoneyFormat(report.MonthlyTotalDinnerAmount),
                ConvertToMoneyFormat(report.TotalAmount)
            });
        }

        private void FillLunchAndDinnerReportContentRow(IEnumerable<LunchAndDinnerModel> listMealsPrice, ref ExcelTemplateWriter writer)
        {
            foreach (LunchAndDinnerModel item in listMealsPrice)
            {
                var record = new
                {
                    Date = item.OnDate.ToString("d-MMM-yyyy"),
                    LunchMeals = ConvertToMoneyFormat(item.LunchMealQuantity),
                    LunchAmount = ConvertToMoneyFormat(item.LunchAmount),
                    DinnerMeals = ConvertToMoneyFormat(item.DinnerMealQuantity),
                    DinnerAmount = ConvertToMoneyFormat(item.DinnerAmount),
                    TotalPrice = ConvertToMoneyFormat(item.TotalAmount)
                };

                writer.WriteRow(1, record);
            }
        }
    }
}