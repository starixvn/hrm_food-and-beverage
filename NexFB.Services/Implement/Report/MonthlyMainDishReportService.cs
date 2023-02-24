// ----------------------------------------------------------------------
// <copyright file="MonthlyMainDishReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="1/15/2015 12:42:36 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of Monthly Main Dish Report Per Meal service.
    /// </summary>
    public class MonthlyMainDishReportService : IMonthlyMainDishReportService
    {
        private readonly IReportRepository _reportRepository;

        public MonthlyMainDishReportService()
            : this(null)
        {
        }

        public MonthlyMainDishReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Get monthly main dish report by local.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The monthly local report.</returns>
        public MonthlyLocalReportModel GetMonthlyMainDishReportLocal(GetMonthlyMainDishReportLocalCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new MonthlyLocalReportModel();

            if (criteria.IsValid())
            {
                report = _reportRepository.GetMonthlyMainDishReportLocal(criteria);
                report.Month = criteria.ReportDate;
                report.IsGroupByEmployee = criteria.GroupBy == (int)GroupByReport.Employee;
                report.GroupBy = (GroupByReport)criteria.GroupBy;
            }

            return report;
        }

        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The paging report.</returns>
        public MonthlyLocalReportModel GetPaingReport(
            MonthlyLocalReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new MonthlyLocalReportModel
            {
                IsGroupByEmployee = report.IsGroupByEmployee,
                GroupBy = report.GroupBy,
                Month = report.Month,
                Total = report.Total,
                WeekDays = report.WeekDays,
                TotalItem = CalculateTotalItem(report)
            };

            List<MealMonthModel> meals = LoopAndGetPagingReport(report, startIndex, itemsPerPage);
            pagingReport.Meals = meals;

            return pagingReport;
        }

        /// <summary>
        /// Export monthly main dish report in local.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templateLink">The url of template.</param>
        public void ExportMonthlyMainDishReportLocal(MonthlyLocalReportModel report, string templateLink)
        {
            string reportName = Lang.MonthlyMainDishLocalReportName;
            string reportTitle = Lang.MonthlyMainDishLocalReportTitle;
            var writer = new ExcelTemplateWriter(templateLink, reportName, Lang.MonthlyMainDishLocalSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);
            writer.CopyRows(4, 1);
            writer.WriteCellRange(3, 1, report.GroupBy == GroupByReport.Employee ? 4 : 2, 31, report.Month.ToString("MMM-yyyy"));
            WriteDayHeaderInMonth(report.WeekDays.NumberMeals, ref writer);

            FillContentRow(report, ref writer);
            writer = FillTotalRow(report.Total, report.GroupBy, writer);
            if (report.GroupBy == GroupByReport.CostCenter)
            {
                WriteSummaryTableGroupByCostCenter(report, ref writer);
            }
            writer.SyncColumnsWidth(37);

            writer.Render();
        }

        /// <summary>
        /// Create meal records.
        /// </summary>
        /// <param name="mealItems">The list meal items.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The Record model.</returns>
        public RecordModel CreateMealRecord(
            List<MealItemMonthModel> mealItems,
            GroupByReport groupBy,
            int rowIndex)
        {
            IEnumerable<object> records = null;
            bool isGroupByCostCenter = groupBy == GroupByReport.CostCenter;

            if (groupBy == GroupByReport.Employee)
            {
                records = CreateMealGroupByEmployee(mealItems, ref rowIndex);
            }
            else
            {
                records = CreateMealGroupByCompanyOrCostCenter(mealItems, ref rowIndex, isGroupByCostCenter);
            }

            var mealRecords = new RecordModel
            {
                Records = records,
                RowIndex = rowIndex
            };

            return mealRecords;
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="total">The total meal.</param>
        /// <param name="writer">The writer.</param>
        /// <returns>The writer after fill total row.</returns>
        public ExcelTemplateWriter FillTotalRow(
            MealDayOfMonthModel total,
            GroupByReport groupBy,
            ExcelTemplateWriter writer)
        {
            object record = null;

            if (groupBy == GroupByReport.Employee)
            {
                record = CreateTotalMealEmployeeRecord(total);
            }
            else
            {
                record = CreateTotalMealCompanyRecord(total);
            }

            var rowId = groupBy == GroupByReport.CostCenter ? 8 : 7;
            writer.WriteRow(rowId, record);

            return writer;
        }

        private void WriteSummaryTableGroupByCostCenter(MonthlyLocalReportModel report, ref ExcelTemplateWriter writer)
        {
            var listCostCenter = GetListCostCenter(report);
            WriteCostCenter(listCostCenter, ref writer, GetTotalRow(report));
            WriteCostCenterContent(report, ref writer);
            WriteCostCenterTotal(report, ref writer);
        }

        private int GetTotalRow(MonthlyLocalReportModel report)
        {
            return report.Meals.Sum(x => x.MealItems.Count) + report.Meals.Count * 2 + 7;
        }

        private void WriteCostCenterContent(MonthlyLocalReportModel report, ref ExcelTemplateWriter writer)
        {
            var rowIndexConst = GetTotalRow(report);
            var rowIndex = rowIndexConst;
            var colIndex = 2;
            foreach (var meal in report.Meals)
            {
                writer.WriteCellRange(rowIndex, 1, colIndex, 1, meal.Meal.MealEngName);
                foreach (var item in meal.MealItems)
                {
                    rowIndex++;
                    writer.WriteCellRange(rowIndex, 1, colIndex, 1, item.NumberMealsMonth.Amount);
                }
                colIndex++;
                rowIndex = rowIndexConst;
            }
        }

        private void WriteCostCenterTotal(MonthlyLocalReportModel report, ref ExcelTemplateWriter writer)
        {
            var listAmount = CalculateTotalAmountCostCenter(report);
            var rowIndex = GetTotalRow(report);
            var colIndex = 2 + report.Meals.Count;
            decimal total = 0;
            writer.WriteCellRange(rowIndex, 1, colIndex, 1, "Total");
            foreach (var item in listAmount)
            {
                rowIndex++;
                total += item;
                writer.WriteCellRange(rowIndex, 1, colIndex, 1, item);
            }
            rowIndex++;
            writer.WriteCellRange(rowIndex, 1, colIndex - 1, 1, "Total (VND)");
            writer.WriteCellRange(rowIndex, 1, colIndex, 1, total);
        }

        private List<decimal> CalculateTotalAmountCostCenter(MonthlyLocalReportModel report)
        {
            var rawSummary = new List<MealCostCenterModel>();

            foreach (var meal in report.Meals)
            {
                foreach (var item in meal.MealItems)
                {
                    rawSummary.Add(new MealCostCenterModel { CostCenter = item.CostCenter, Amount = item.NumberMealsMonth.Amount });
                }
            }
            var listAmount = new List<decimal>();
            var groupCostCenter = rawSummary.GroupBy(x => x.CostCenter).Select(g => new { CostCenter = g.Key, Amounts = g });
            foreach (var item in groupCostCenter)
            {
                listAmount.Add(item.Amounts.Sum(x => x.Amount));
            }

            return listAmount;
        }

        private void WriteCostCenter(List<string> listCostCenter, ref ExcelTemplateWriter writer, int rowIndexConst)
        {
            var colIndex = 1;
            var rowIndex = rowIndexConst;
            writer.CopyRows(1, 1);
            writer.CopyRows(10, 1);
            writer.WriteCellRange(rowIndex, 1, colIndex, 1, string.Empty);

            foreach (var costCenter in listCostCenter)
            {
                rowIndex++;
                writer.CopyRows(11, 1);
                writer.WriteCellRange(rowIndex, 1, colIndex, 1, costCenter);
            }

            writer.CopyRows(11, 1);
        }

        private List<string> GetListCostCenter(MonthlyLocalReportModel report)
        {
            var listCostCenter = new List<string>();
            foreach (var meal in report.Meals)
            {
                foreach (var mealItem in meal.MealItems)
                {
                    listCostCenter.Add(mealItem.CostCenter);
                }
            }
            return listCostCenter.Distinct().ToList();
        }

        private int CalculateTotalItem(MonthlyLocalReportModel report)
        {
            int totalItem = 0;

            foreach (MealMonthModel meal in report.Meals)
            {
                totalItem += meal.MealItems.Count;
            }

            return totalItem;
        }

        /// <summary>
        /// Loop and get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list new meals.</returns>
        private List<MealMonthModel> LoopAndGetPagingReport(
            MonthlyLocalReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var meals = new List<MealMonthModel>();
            int numberItemsInMeal = 0;
            int numberMeals = report.Meals.Count;
            int index = 0;

            while (index < numberMeals)
            {
                MealMonthModel meal = report.Meals[index];
                numberItemsInMeal = meal.MealItems.Count;

                if (numberItemsInMeal > startIndex)
                {
                    int numberItemsRemain = numberItemsInMeal - startIndex;

                    if (numberItemsRemain <= itemsPerPage)
                    {
                        MealMonthModel pagingMeal = AddMealItemIntoPagingReport(startIndex, meal, numberItemsRemain);
                        meals.Add(pagingMeal);

                        if (numberItemsRemain == itemsPerPage)
                        {
                            break;
                        }

                        startIndex = 0;
                        itemsPerPage -= numberItemsRemain;
                    }
                    else
                    {
                        MealMonthModel paingMeal = AddMealItemIntoPagingReport(startIndex, meal, itemsPerPage);
                        meals.Add(paingMeal);
                        break;
                    }
                }
                else
                {
                    startIndex -= numberItemsInMeal;
                }

                index++;
            }

            return meals;
        }

        /// <summary>
        /// Create new meal of paging report.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="meal">The current meal.</param>
        /// <param name="items">The number items per page.</param>
        /// <returns>The new meal.</returns>
        private MealMonthModel AddMealItemIntoPagingReport(int startIndex, MealMonthModel meal, int items)
        {
            var newMeal = new MealMonthModel
            {
                Meal = meal.Meal,
                MealItems = meal.MealItems.Skip(startIndex).Take(items).ToList(),
                TotalAmountOfMeal = meal.TotalAmountOfMeal,
                TotalQuantityOfMeal = meal.TotalQuantityOfMeal,
                TotalQuantityPerDays = meal.TotalQuantityPerDays
            };

            return newMeal;
        }

        /// <summary>
        /// Convert weekend number from positive number to negative number.
        /// </summary>
        /// <param name="weekdays">The weekdays in month.</param>
        /// <param name="writer">The excel writer.</param>
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

        private void FillContentRow(MonthlyLocalReportModel report, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 1;

            foreach (MealMonthModel meal in report.Meals)
            {
                FillContentMeal(meal, ref writer, report.GroupBy, ref rowIndex);
            }
        }

        private void FillSubTotalRow(MealMonthModel meal, ref ExcelTemplateWriter writer)
        {
            var totalMeals = meal.TotalQuantityPerDays;
            var record = new
            {
                Total = string.Format("{0}{1}", meal.Meal.MealEngName, " Total"),
                MergeCell = string.Empty,
                Day1 = totalMeals[0],
                Day2 = totalMeals[1],
                Day3 = totalMeals[2],
                Day4 = totalMeals[3],
                Day5 = totalMeals[4],
                Day6 = totalMeals[5],
                Day7 = totalMeals[6],
                Day8 = totalMeals[7],
                Day9 = totalMeals[8],
                Day10 = totalMeals[9],
                Day11 = totalMeals[10],
                Day12 = totalMeals[11],
                Day13 = totalMeals[12],
                Day14 = totalMeals[13],
                Day15 = totalMeals[14],
                Day16 = totalMeals[15],
                Day17 = totalMeals[16],
                Day18 = totalMeals[17],
                Day19 = totalMeals[18],
                Day20 = totalMeals[19],
                Day21 = totalMeals[20],
                Day22 = totalMeals[21],
                Day23 = totalMeals[22],
                Day24 = totalMeals[23],
                Day25 = totalMeals[24],
                Day26 = totalMeals[25],
                Day27 = totalMeals[26],
                Day28 = totalMeals[27],
                Day29 = totalMeals[28],
                Day30 = totalMeals[29],
                Day31 = totalMeals[30],
                Quantity = meal.TotalQuantityOfMeal,
                Amount = meal.TotalAmountOfMeal
            };
            writer.WriteRow(7, record);
        }

        private void FillContentMeal(
            MealMonthModel meal,
            ref ExcelTemplateWriter writer,
            GroupByReport groupBy,
            ref int rowIndex)
        {
            RecordModel mealRecord = CreateMealRecord(meal.MealItems, groupBy, rowIndex);
            rowIndex = mealRecord.RowIndex;

            writer.WriteRow(5, new object[] { meal.Meal.MealEngName });
            writer.WriteRows(6, mealRecord.Records);
            if (groupBy == GroupByReport.CostCenter)
            {
                FillSubTotalRow(meal, ref writer);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IEnumerable<object> CreateMealGroupByEmployee(List<MealItemMonthModel> mealItems, ref int rowIndex)
        {
            IList<object> records = new List<object>();

            for (int i = 0; i < mealItems.Count; i++)
            {
                MealItemMonthModel mealItem = mealItems[i];
                List<int> numberMeals = mealItem.NumberMealsMonth.NumberMeals;

                var record = new
                {
                    Count = rowIndex,
                    Nick = mealItem.Employee.NickName,
                    EmployeeType = mealItem.EmployeeType.EmployeeTypeName,
                    DepartmentSection = mealItem.Company.Name + "> " + mealItem.Team.Name,
                    Day1 = numberMeals[0],
                    Day2 = numberMeals[1],
                    Day3 = numberMeals[2],
                    Day4 = numberMeals[3],
                    Day5 = numberMeals[4],
                    Day6 = numberMeals[5],
                    Day7 = numberMeals[6],
                    Day8 = numberMeals[7],
                    Day9 = numberMeals[8],
                    Day10 = numberMeals[9],
                    Day11 = numberMeals[10],
                    Day12 = numberMeals[11],
                    Day13 = numberMeals[12],
                    Day14 = numberMeals[13],
                    Day15 = numberMeals[14],
                    Day16 = numberMeals[15],
                    Day17 = numberMeals[16],
                    Day18 = numberMeals[17],
                    Day19 = numberMeals[18],
                    Day20 = numberMeals[19],
                    Day21 = numberMeals[20],
                    Day22 = numberMeals[21],
                    Day23 = numberMeals[22],
                    Day24 = numberMeals[23],
                    Day25 = numberMeals[24],
                    Day26 = numberMeals[25],
                    Day27 = numberMeals[26],
                    Day28 = numberMeals[27],
                    Day29 = numberMeals[28],
                    Day30 = numberMeals[29],
                    Day31 = numberMeals[30],
                    Quantity = mealItem.NumberMealsMonth.TotalQuantity,
                    Amount = mealItem.NumberMealsMonth.Amount
                };

                records.Add(record);
                rowIndex++;
            }

            return records;
        }

        /// <summary>
        ///  Create meal records of report group by department.
        /// </summary>
        /// <param name="mealItems">The list meal items.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>The enumerator object.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IEnumerable<object> CreateMealGroupByCompanyOrCostCenter(List<MealItemMonthModel> mealItems, ref int rowIndex, bool isGroupByCostCenter)
        {
            IList<object> records = new List<object>();

            for (int i = 0; i < mealItems.Count; i++)
            {
                MealItemMonthModel mealItem = mealItems[i];
                List<int> numberMeals = mealItem.NumberMealsMonth.NumberMeals;

                var record = new
                {
                    Count = rowIndex,
                    DepartmentSection = isGroupByCostCenter ? mealItem.CostCenter : mealItem.Company.Name + "> " + mealItem.Team.Name,
                    Day1 = numberMeals[0],
                    Day2 = numberMeals[1],
                    Day3 = numberMeals[2],
                    Day4 = numberMeals[3],
                    Day5 = numberMeals[4],
                    Day6 = numberMeals[5],
                    Day7 = numberMeals[6],
                    Day8 = numberMeals[7],
                    Day9 = numberMeals[8],
                    Day10 = numberMeals[9],
                    Day11 = numberMeals[10],
                    Day12 = numberMeals[11],
                    Day13 = numberMeals[12],
                    Day14 = numberMeals[13],
                    Day15 = numberMeals[14],
                    Day16 = numberMeals[15],
                    Day17 = numberMeals[16],
                    Day18 = numberMeals[17],
                    Day19 = numberMeals[18],
                    Day20 = numberMeals[19],
                    Day21 = numberMeals[20],
                    Day22 = numberMeals[21],
                    Day23 = numberMeals[22],
                    Day24 = numberMeals[23],
                    Day25 = numberMeals[24],
                    Day26 = numberMeals[25],
                    Day27 = numberMeals[26],
                    Day28 = numberMeals[27],
                    Day29 = numberMeals[28],
                    Day30 = numberMeals[29],
                    Day31 = numberMeals[30],
                    Quantity = mealItem.NumberMealsMonth.TotalQuantity,
                    Amount = mealItem.NumberMealsMonth.Amount
                };

                records.Add(record);
                rowIndex++;
            }

            return records;
        }

        /// <summary>
        /// Create total meal row.
        /// </summary>
        /// <param name="totalMeal">The total meal.</param>
        /// <returns>The total row.</returns>
        private object CreateTotalMealEmployeeRecord(MealDayOfMonthModel totalMeal)
        {
            List<int> numberMeals = totalMeal.NumberMeals;

            var record = new
            {
                Count = Lang.Total,
                Nick = string.Empty,
                EmployeeType = string.Empty,
                DepartmentSection = string.Empty,
                Day1 = numberMeals[0],
                Day2 = numberMeals[1],
                Day3 = numberMeals[2],
                Day4 = numberMeals[3],
                Day5 = numberMeals[4],
                Day6 = numberMeals[5],
                Day7 = numberMeals[6],
                Day8 = numberMeals[7],
                Day9 = numberMeals[8],
                Day10 = numberMeals[9],
                Day11 = numberMeals[10],
                Day12 = numberMeals[11],
                Day13 = numberMeals[12],
                Day14 = numberMeals[13],
                Day15 = numberMeals[14],
                Day16 = numberMeals[15],
                Day17 = numberMeals[16],
                Day18 = numberMeals[17],
                Day19 = numberMeals[18],
                Day20 = numberMeals[19],
                Day21 = numberMeals[20],
                Day22 = numberMeals[21],
                Day23 = numberMeals[22],
                Day24 = numberMeals[23],
                Day25 = numberMeals[24],
                Day26 = numberMeals[25],
                Day27 = numberMeals[26],
                Day28 = numberMeals[27],
                Day29 = numberMeals[28],
                Day30 = numberMeals[29],
                Day31 = numberMeals[30],
                Quantity = totalMeal.TotalQuantity,
                Amount = totalMeal.Amount
            };

            return record;
        }

        /// <summary>
        /// Create total meal row.
        /// </summary>
        /// <param name="totalMeal">The total meal.</param>
        /// <returns>The total row.</returns>
        private object CreateTotalMealCompanyRecord(MealDayOfMonthModel totalMeal)
        {
            List<int> numberMeals = totalMeal.NumberMeals;

            var record = new
            {
                Count = Lang.Total,
                Nick = string.Empty,
                Day1 = numberMeals[0],
                Day2 = numberMeals[1],
                Day3 = numberMeals[2],
                Day4 = numberMeals[3],
                Day5 = numberMeals[4],
                Day6 = numberMeals[5],
                Day7 = numberMeals[6],
                Day8 = numberMeals[7],
                Day9 = numberMeals[8],
                Day10 = numberMeals[9],
                Day11 = numberMeals[10],
                Day12 = numberMeals[11],
                Day13 = numberMeals[12],
                Day14 = numberMeals[13],
                Day15 = numberMeals[14],
                Day16 = numberMeals[15],
                Day17 = numberMeals[16],
                Day18 = numberMeals[17],
                Day19 = numberMeals[18],
                Day20 = numberMeals[19],
                Day21 = numberMeals[20],
                Day22 = numberMeals[21],
                Day23 = numberMeals[22],
                Day24 = numberMeals[23],
                Day25 = numberMeals[24],
                Day26 = numberMeals[25],
                Day27 = numberMeals[26],
                Day28 = numberMeals[27],
                Day29 = numberMeals[28],
                Day30 = numberMeals[29],
                Day31 = numberMeals[30],
                Quantity = totalMeal.TotalQuantity,
                Amount = totalMeal.Amount
            };

            return record;
        }
    }
}