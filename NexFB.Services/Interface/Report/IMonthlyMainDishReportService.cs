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
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;

    /// <summary>
    /// The interface of Monthly Main Dish Per Meal report.
    /// </summary>
    public interface IMonthlyMainDishReportService
    {
        MonthlyLocalReportModel GetMonthlyMainDishReportLocal(GetMonthlyMainDishReportLocalCriteria criteria);

        MonthlyLocalReportModel GetPaingReport(MonthlyLocalReportModel report, int startIndex, int itemsPerPage);

        void ExportMonthlyMainDishReportLocal(MonthlyLocalReportModel report, string templateLink);

        RecordModel CreateMealRecord(List<MealItemMonthModel> mealItems, bool isGroupByEmployee, int rowIndex);
        
        SideDishReportModel GetSideDishReport(GetSideDishReportCriteria criteria);

        ExcelTemplateWriter FillTotalRow(MealDayOfMonthModel total, bool isGroupByEmployee, ExcelTemplateWriter writer);
        
        void ExportSideDishReport(SideDishReportModel report, string templateUrl);
    }
}
