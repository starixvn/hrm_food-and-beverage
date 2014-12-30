// ----------------------------------------------------------------------
// <copyright file="IReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/5/2014 8:56:49 AM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;

    /// <summary>
    /// The interface of report service.
    /// </summary>
    public interface IReportService
    {
        MonthlyLocalReportModel GetMonthlyMainDishReportLocal(GetMonthlyMainDishReportLocalCriteria criteria);

        void ExportMonthlyMainDishReportLocal(MonthlyLocalReportModel report, string templateLink);

        MonthlyLocalReportModel GetPaingReport(MonthlyLocalReportModel report, int startIndex, int itemsPerPage);

        DailyMainDishReportModel GetDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria);

        RecordModel CreateMealRecord(List<MealItemMonthModel> mealItems, bool isGroupByEmployee, int rowIndex);

        ExcelTemplateWriter FillTotalRow(MealDayOfMonthModel total, bool isGroupByEmployee, ExcelTemplateWriter writer);
    }
}
