namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    public class MonthlyMainDishPerDayReportService : BaseService, IMonthlyMainDishPerDayReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMonthlyMainDishReportService _monthlyMainDishPerMealReportService;

        public MonthlyMainDishPerDayReportService()
            : this(null, null)
        {
        }

        public MonthlyMainDishPerDayReportService(
            IReportRepository reportRepository,
            IMonthlyMainDishReportService reportService)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
            _monthlyMainDishPerMealReportService = reportService ?? new MonthlyMainDishReportService();
        }
        public MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            MonthlyMainDishPerDayModel report = null;

            if (criteria.IsValid())
            {
                report = _reportRepository.GetMonthlyMainDishPerDayReport(criteria);
                report.TotalItem = CalculateTotelItem(report.TypeDays);
                report.Month = criteria.ReportDate;
                report.IsGroupByEmployee = criteria.GroupBy == (int)GroupByReport.Employee;
            }

            return report;
        }
        public MonthlyMainDishPerDayModel GetPagingReport(
            MonthlyMainDishPerDayModel fullReport,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new MonthlyMainDishPerDayModel
            {
                IsGroupByEmployee = fullReport.IsGroupByEmployee,
                Month = fullReport.Month,
                Total = fullReport.Total,
                TotalItem = fullReport.TotalItem,
                WeekDays = fullReport.WeekDays
            };

            pagingReport.TypeDays = LoopAndGetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }
        public void ExportMonthlyMainDishPerDayReport(
            MonthlyMainDishPerDayModel report,
            string templatePath)
        {
            string reportName = Lang.MonthlyMainDishPerDayReportName;
            string reportTitle = Lang.MonthlyMainDishPerDayReportTitle;
            var writer = new ExcelTemplateWriter(templatePath, reportName, Lang.MonthlyMainDishPerDaySheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);
            writer.CopyRows(4, 1);
            writer.WriteCellRange(3, 1, 4, 31, report.Month);
            WriteDayHeaderInMonth(report.WeekDays.NumberMeals, ref writer);

            FillContentRow(report, ref writer);
            writer = _monthlyMainDishPerMealReportService.FillTotalRow(report.Total, report.GroupBy, writer);
            writer.SyncColumnsWidth(37);

            writer.Render();
        }

        private void WriteDayHeaderInMonth(List<int> weekdays, ref ExcelTemplateWriter writer)
        {
            int startCol = 4;

            for (int i = 0; i < 31; i++)
            {
                int currentDate = weekdays[i];

                if (currentDate == 7 || currentDate == 1)
                {
                    writer.WriteCellRange(4, 1, startCol + i, 1, -(i + 1));
                }
            }
        }

        private void FillContentRow(MonthlyMainDishPerDayModel report, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 1;

            foreach (MainDishPerDayModel typeDay in report.TypeDays)
            {
                FillContentMeal(typeDay, ref writer, report.GroupBy, ref rowIndex);
            }
        }

        private void FillContentMeal(
            MainDishPerDayModel typeDay,
            ref ExcelTemplateWriter writer,
            GroupByReport groupBy,
            ref int rowIndex)
        {
            RecordModel mealRecord = _monthlyMainDishPerMealReportService.CreateMealRecord(typeDay.MealItems, groupBy, rowIndex);
            rowIndex = mealRecord.RowIndex;

            writer.WriteRow(5, new object[] { typeDay.TypeDay });
            writer.WriteRows(6, mealRecord.Records);
        }

        private int CalculateTotelItem(List<MainDishPerDayModel> typeDays)
        {
            int totalItem = 0;

            foreach (MainDishPerDayModel typeDay in typeDays)
            {
                totalItem += typeDay.MealItems.Count;
            }

            return totalItem;
        }
        private List<MainDishPerDayModel> LoopAndGetPagingReport(
            MonthlyMainDishPerDayModel report,
            int startIndex,
            int itemsPerPage)
        {
            var typeDays = new List<MainDishPerDayModel>();
            int numberItemsInMeal = 0;
            int numberMeals = report.TypeDays.Count;
            int index = 0;

            while (index < numberMeals)
            {
                MainDishPerDayModel typeDay = report.TypeDays[index];
                numberItemsInMeal = typeDay.MealItems.Count;

                if (numberItemsInMeal > startIndex)
                {
                    int numberItemsRemain = numberItemsInMeal - startIndex;

                    if (numberItemsRemain <= itemsPerPage)
                    {
                        MainDishPerDayModel pagingTypeDay = AddMealItemIntoPagingReport(startIndex, typeDay, numberItemsRemain);
                        typeDays.Add(pagingTypeDay);

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
                        MainDishPerDayModel pagingTypeDay = AddMealItemIntoPagingReport(startIndex, typeDay, itemsPerPage);
                        typeDays.Add(pagingTypeDay);
                        break;
                    }
                }
                else
                {
                    startIndex -= numberItemsInMeal;
                }

                index++;
            }

            return typeDays;
        }

        private MainDishPerDayModel AddMealItemIntoPagingReport(
            int startIndex,
            MainDishPerDayModel typeDay,
            int items)
        {
            var pagingTypeDay = new MainDishPerDayModel
            {
                IsOffDay = typeDay.IsOffDay,
                MealItems = typeDay.MealItems.Skip(startIndex).Take(items).ToList()
            };

            return pagingTypeDay;
        }
    }
}