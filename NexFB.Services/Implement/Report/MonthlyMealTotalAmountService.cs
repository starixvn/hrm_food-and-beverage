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
    using NexFB.Criteria.Company;
    using NexFB.Models;
    using Resources;

    public class MonthlyMealTotalAmountService : BaseService, IMonthlyMealTotalAmountService
    {
        public LunchAndDinnerTotalAmountReportModel GetMonthlyLunchAndDinnerReport(DateTime reportDate, string departmentId)
        {
            var report = new LunchAndDinnerTotalAmountReportModel();
            var mealsPrice = GetListLunchAndDinnerAmountOnMonth(reportDate, departmentId);

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

        private IEnumerable<LunchAndDinnerModel> GetListLunchAndDinnerAmountOnMonth(DateTime reportDate, string departmentId)
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
            string reportTitle = GetReportTitle(reportDate, departmentId, Lang.LunchAndDinner);
            // Fill report title.
            writer.WriteRow(0, new { reportTitle });
            writer.CopyRows(1, 2);

            FillLunchAndDinnerReportContentRow(report.MealPriceList, ref writer);
            FillTotalLunchAndDinerReportRow(report, writer);
            writer.SyncColumnsWidth(6);

            writer.Render();
        }

        private string ConvertToNumberFormat(double number)
        {
            return string.Format("{0:#,##0}", number);
        }

        private void FillTotalLunchAndDinerReportRow(LunchAndDinnerTotalAmountReportModel report, ExcelTemplateWriter writer)
        {
            writer.WriteRow(2, new object[]
            {
                Lang.Total,
                report.TotalLunchMeals,
                report.MonthlyTotalLunchAmount,
                report.TotalDinnerMeals,
                report.MonthlyTotalDinnerAmount,
                report.TotalAmount
            });
        }

        private void FillLunchAndDinnerReportContentRow(IEnumerable<LunchAndDinnerModel> listMealsPrice, ref ExcelTemplateWriter writer)
        {
            foreach (LunchAndDinnerModel item in listMealsPrice)
            {
                var record = new
                {
                    Date = item.OnDate.ToString("dd-MMM-yyyy"),
                    LunchMeals = item.LunchMealQuantity,
                    LunchAmount = item.LunchAmount,
                    DinnerMeals = item.DinnerMealQuantity,
                    DinnerAmount = item.DinnerAmount,
                    TotalPrice = item.TotalAmount
                };

                writer.WriteRow(1, record);
            }
        }

        public BreakfastAndSupperTotalAmountReportModel GetMonthlyBreakfastAndSupperReport(DateTime reportDate, string companyId)
        {
            var report = new BreakfastAndSupperTotalAmountReportModel();
            var mealsPrice = GetListBreakfastAndSupperAmountOnMonth(reportDate, companyId);

            if (mealsPrice != null)
            {
                report.MealAmountList = mealsPrice;
                report.TotalBreakfastMeals = mealsPrice.Sum(m => m.BreakfastMealQuantity);
                report.MonthlyTotalBreakfastAmount = mealsPrice.Sum(m => m.BreakfastAmount);
                report.TotalSupperMeals = mealsPrice.Sum(m => m.SupperMealQuantity);
                report.MonthlyTotalSupperAmount = mealsPrice.Sum(m => m.SupperAmount);
            }

            return report;
        }

        private IEnumerable<BreakfastAndSupperModel> GetListBreakfastAndSupperAmountOnMonth(DateTime reportDate, string companyId)
        {
            var criteria = new GetMonthlyBreakfastAndSupperReportCriteria
            {
                ReportDate = reportDate,
                CompanyId = companyId
            };
            return GetData<BreakfastAndSupperModel>(criteria);
        }

        public void ExportMonthlyBreakfastAndSupperReport(string templateLink, DateTime reportDate, string companyId)
        {
            var reportName = Lang.MonthlyBreakfastAndSupperTotalAmountReport;
            var writer = new ExcelTemplateWriter(templateLink, reportName);
            var report = GetMonthlyBreakfastAndSupperReport(reportDate, companyId);
            string reportTitle = GetReportTitle(reportDate, companyId, Lang.BreakfastAndSupper);
            // Fill report title.
            writer.WriteRow(0, new { reportTitle });
            writer.CopyRows(1, 2);

            FillBreakfastAndSupperReportContentRow(report.MealAmountList, ref writer);
            FillTotalBreakfastAndSupperReportRow(report, writer);
            writer.SyncColumnsWidth(6);

            writer.Render();
        }

        private void FillTotalBreakfastAndSupperReportRow(BreakfastAndSupperTotalAmountReportModel report, ExcelTemplateWriter writer)
        {
            writer.WriteRow(2, new object[]
            {
                Lang.Total,
                report.TotalBreakfastMeals,
                report.MonthlyTotalBreakfastAmount,
                report.TotalSupperMeals,
                report.MonthlyTotalSupperAmount,
                report.TotalAmount
            });
        }

        private void FillBreakfastAndSupperReportContentRow(IEnumerable<BreakfastAndSupperModel> mealAmountList, ref ExcelTemplateWriter writer)
        {
            foreach (BreakfastAndSupperModel item in mealAmountList)
            {
                var record = new
                {
                    Date = item.OnDate.ToString("dd-MMM-yyyy"),
                    BreakfastMeals = item.BreakfastMealQuantity,
                    BreakfastAmount = item.BreakfastAmount,
                    SupperMeals = item.SupperMealQuantity,
                    SupperAmount = item.SupperAmount,
                    TotalPrice = item.TotalAmount
                };

                writer.WriteRow(1, record);
            }
        }

        private string GetReportTitle(DateTime reportDate, string companyId, string suffixReportType)
        {
            var companyName = "All Company";
            var companies = !string.IsNullOrWhiteSpace(companyId) ? companyId.Split(',') : new string[] { };
            if (companies.Length == 1)
            {
                var criteria = new GetCompanyByIdCriteria { CompanyId = short.Parse(companies[0]) };
                var company = GetDataEntity<Company>(criteria);
                companyName = company.Name;
            }
            return string.Format("{0}{1}{2}{3}{4}{5}",
                Lang.MealRecord,
                reportDate.ToString("MMM yyyy"),
                " - ",
                companyName,
                Environment.NewLine,
                suffixReportType).ToUpperInvariant();
        }
    }
}