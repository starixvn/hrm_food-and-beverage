namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    public class DailyMainDishReportService : BaseService, IDailyMainDishReportService
    {
        private readonly IReportRepository _reportRepository;

        public DailyMainDishReportService()
            : this(null)
        {
        }

        public DailyMainDishReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        public DailyMainDishReportModel GetDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new DailyMainDishReportModel();

            if (criteria.IsValid())
            {
                report = _reportRepository.GetDailyMainDishReport(criteria);
            }

            return report;
        }

        public DailyMainDishReportModel GetPagingReport(
            DailyMainDishReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new DailyMainDishReportModel
            {
                Total = report.Total,
                GrandTotal = report.GrandTotal,
                TotalItem = CalculateTotalItems(report)
            };

            pagingReport.MainDishes = LoopAndGetPagingReport(report, startIndex, itemsPerPage);
            return pagingReport;
        }

        public void ExportDailyMainDishReport(DailyMainDishReportModel report, string templatePath)
        {
            string reportName = Lang.DailyMainDishReportName;
            string reportTitle = Lang.DailyMainDishReportTitle;
            var writer = new ExcelTemplateWriter(templatePath, reportName, Lang.DailyMainDishReportSheet);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report, ref writer);
            FillFooterRow(report, ref writer);
            writer.SyncColumnsWidth(9);

            writer.Render();
        }
        private int CalculateTotalItems(DailyMainDishReportModel report)
        {
            int totalItem = 0;

            foreach (DailyMainDishCompanyTeamModel mainDish in report.MainDishes)
            {
                totalItem += mainDish.EmployeeDishes.Count();
            }

            return totalItem;
        }

        private List<DailyMainDishCompanyTeamModel> LoopAndGetPagingReport(
            DailyMainDishReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var mainDishes = new List<DailyMainDishCompanyTeamModel>();
            int numberItemsInMainDish = 0;
            int numberMainDishes = report.MainDishes.Count;
            int index = 0;

            while (index < numberMainDishes)
            {
                DailyMainDishCompanyTeamModel mainDish = report.MainDishes[index];
                numberItemsInMainDish = mainDish.EmployeeDishes.Count;

                if (numberItemsInMainDish > startIndex)
                {
                    int numberItemsRemain = numberItemsInMainDish - startIndex;

                    if (numberItemsRemain <= itemsPerPage)
                    {
                        DailyMainDishCompanyTeamModel pagingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, numberItemsRemain);
                        mainDishes.Add(pagingMainDish);

                        if (numberItemsRemain == itemsPerPage)
                        {
                            break;
                        }
                        else
                        {
                            startIndex = 0;
                            itemsPerPage -= numberItemsRemain;
                        }
                    }
                    else
                    {
                        DailyMainDishCompanyTeamModel paingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, itemsPerPage);
                        mainDishes.Add(paingMainDish);
                        break;
                    }
                }
                else
                {
                    startIndex -= numberItemsInMainDish;
                }

                index++;
            }

            return mainDishes;
        }

        private DailyMainDishCompanyTeamModel AddEmployeeItemIntoPagingReport(int startIndex, DailyMainDishCompanyTeamModel mainDish, int numberItemsRemain)
        {
            var newMainDish = new DailyMainDishCompanyTeamModel
            {
                Company = mainDish.Company,
                Team = mainDish.Team,
                EmployeeDishes = mainDish.EmployeeDishes.Skip(startIndex).Take(numberItemsRemain).ToList()
            };

            return newMainDish;
        }

        private void FillContentRow(DailyMainDishReportModel report, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 1;

            foreach (DailyMainDishCompanyTeamModel mainDish in report.MainDishes)
            {
                FillContentCompanyTeam(mainDish, ref writer, ref rowIndex);
            }
        }

        private void FillFooterRow(DailyMainDishReportModel report, ref ExcelTemplateWriter writer)
        {
            FillTotalRow(report, ref writer);
            FillAmountRow(report.GrandTotal, ref writer);
        }

        private void FillContentCompanyTeam(
            DailyMainDishCompanyTeamModel mainDish,
            ref ExcelTemplateWriter writer,
            ref int rowIndex)
        {
            IEnumerable<object> records = CreateRecord(mainDish.EmployeeDishes, ref rowIndex);

            writer.WriteRow(4, new object[] { mainDish.Company.Name + " - " + mainDish.Team.Name });
            writer.WriteRows(5, records);
        }

        private IEnumerable<object> CreateRecord(List<DailyEmployeeMainDishModel> employeeItems, ref int rowIndex)
        {
            IList<object> records = new List<object>();

            for (int i = 0; i < employeeItems.Count; i++)
            {
                DailyEmployeeMainDishModel employeeItem = employeeItems[i];
                var record = new
                {
                    Count = rowIndex,
                    Date = employeeItem.WorkDate,
                    Shift = employeeItem.Shift != null ? employeeItem.Shift.ShiftCode : string.Empty,
                    FullName = employeeItem.Employee.FullName,
                    Nick = employeeItem.Employee.NickName,
                    Breakfast = employeeItem.Breakfast,
                    Lunch = employeeItem.Lunch,
                    Dinner = employeeItem.Dinner,
                    Supper = employeeItem.Supper
                };

                rowIndex++;
                records.Add(record);
            }

            return records;
        }

        private object CreateTotalRecord(TotalPriceModel total, string typeTotal)
        {
            var record = new
            {
                Count = typeTotal,
                Date = string.Empty,
                Shift = string.Empty,
                FullName = string.Empty,
                Nick = string.Empty,
                Breakfast = total.Breakfast,
                Lunch = total.Lunch,
                Dinner = total.Dinner,
                Supper = total.Supper
            };

            return record;
        }

        private void FillTotalRow(DailyMainDishReportModel report, ref ExcelTemplateWriter writer)
        {
            object subTotalRecord = CreateTotalRecord(report.Total, Lang.Total);
            writer.WriteRow(6, subTotalRecord);
        }

        private void FillAmountRow(int totalAmount, ref ExcelTemplateWriter writer)
        {
            var record = new
            {
                Count = Lang.GrandTotal,
                Date = string.Empty,
                Shift = string.Empty,
                FullName = string.Empty,
                Nick = string.Empty,
                Breakfast = totalAmount,
                Lunch = string.Empty,
                Dinner = string.Empty,
                Supper = string.Empty
            };

            writer.WriteRow(7, record);
        }
    }
}