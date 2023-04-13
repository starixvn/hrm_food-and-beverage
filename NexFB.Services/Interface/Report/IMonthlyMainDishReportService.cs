// ----------------------------------------------------------------------
// <copyright file="IMonthlyMainDishReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="1/15/2015 12:33:27 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using System.Collections.Generic;
    using Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;

    /// <summary>
    /// The interface of Monthly Main Dish Per Meal report.
    /// </summary>
    public interface IMonthlyMainDishReportService
    {
        MonthlyLocalReportModel GetMonthlyMainDishReportLocal(GetMonthlyMainDishReportLocalCriteria criteria);

        MonthlyLocalReportModel GetPaingReport(MonthlyLocalReportModel report, int startIndex, int itemsPerPage);

        void ExportMonthlyMainDishReportLocal(MonthlyLocalReportModel report, string templateLink);

        RecordModel CreateMealRecord(List<MealItemMonthModel> mealItems, GroupByReport groupBy, int rowIndex);
        RecordModel CreateMealRecord(List<MealItemMonthModel> mealItems, GroupByReport groupBy, int rowIndex, List<BreakRuleOrder> breakRuleOrders, bool IsGroupByEmployee);

        ExcelTemplateWriter FillTotalRow(MealDayOfMonthModel total, GroupByReport groupBy, ExcelTemplateWriter writer, bool IsGroupByEmployee);
    }
}