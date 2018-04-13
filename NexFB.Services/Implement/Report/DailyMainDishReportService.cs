// ----------------------------------------------------------------------
// <copyright file="DailyMainDishReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/18/2014 5:26:19 PM">Created.</change>
// </history>

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

    /// <summary>
    /// The implement of Daily Main Dish Report service.
    /// </summary>
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

        /// <summary>
        /// Get daily main dish report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The daily main dish report.</returns>
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

        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The Daily Main Dish report.</returns>
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

        /// <summary>
        /// Export daily main dish report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template path.</param>
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

        /// <summary>
        /// Calculate total items of report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>The total items.</returns>
        private int CalculateTotalItems(DailyMainDishReportModel report)
        {
            int totalItem = 0;

            foreach (DailyMainDishDepartmentTeamModel mainDish in report.MainDishes)
            {
                totalItem += mainDish.EmployeeDishes.Count();
            }

            return totalItem;
        }

        /// <summary>
        /// Loop and get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list new main dished of department and team.</returns>
        private List<DailyMainDishDepartmentTeamModel> LoopAndGetPagingReport(
            DailyMainDishReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var mainDishes = new List<DailyMainDishDepartmentTeamModel>();
            int numberItemsInMainDish = 0;
            int numberMainDishes = report.MainDishes.Count;
            int index = 0;

            while (index < numberMainDishes)
            {
                DailyMainDishDepartmentTeamModel mainDish = report.MainDishes[index];
                numberItemsInMainDish = mainDish.EmployeeDishes.Count;

                if (numberItemsInMainDish > startIndex)
                {
                    int numberItemsRemain = numberItemsInMainDish - startIndex;

                    if (numberItemsRemain <= itemsPerPage)
                    {
                        DailyMainDishDepartmentTeamModel pagingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, numberItemsRemain);
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
                        DailyMainDishDepartmentTeamModel paingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, itemsPerPage);
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

        private DailyMainDishDepartmentTeamModel AddEmployeeItemIntoPagingReport(int startIndex, DailyMainDishDepartmentTeamModel mainDish, int numberItemsRemain)
        {
            var newMainDish = new DailyMainDishDepartmentTeamModel
            {
                Department = mainDish.Department,
                Team = mainDish.Team,
                EmployeeDishes = mainDish.EmployeeDishes.Skip(startIndex).Take(numberItemsRemain).ToList()
            };

            return newMainDish;
        }

        private void FillContentRow(DailyMainDishReportModel report, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 1;

            foreach (DailyMainDishDepartmentTeamModel mainDish in report.MainDishes)
            {
                FillContentDepartmentTeam(mainDish, ref writer, ref rowIndex);
            }
        }

        private void FillFooterRow(DailyMainDishReportModel report, ref ExcelTemplateWriter writer)
        {
            FillTotalRow(report, ref writer);
            FillAmountRow(report.GrandTotal, ref writer);
        }

        private void FillContentDepartmentTeam(
            DailyMainDishDepartmentTeamModel mainDish,
            ref ExcelTemplateWriter writer,
            ref int rowIndex)
        {
            IEnumerable<object> records = CreateRecord(mainDish.EmployeeDishes, ref rowIndex);

            writer.WriteRow(4, new object[] { mainDish.Department.Name + " - " + mainDish.Team.Name });
            writer.WriteRows(5, records);
        }

        /// <summary>
        /// Create record.
        /// </summary>
        /// <param name="employeeItems">The employee items.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The list object record.</returns>
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

        /// <summary>
        /// Create total record.
        /// </summary>
        /// <param name="total">The total data.</param>
        /// <param name="typeTotal">The type of total.</param>
        /// <returns>The object data total.</returns>
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

        /// <summary>
        /// Fill total amount row.
        /// </summary>
        /// <param name="totalAmount">The total amount.</param>
        /// <param name="writer">The writer.</param>
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