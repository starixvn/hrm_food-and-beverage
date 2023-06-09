// ----------------------------------------------------------------------
// <copyright file="IMonthlyMainDishPerDayReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/26/2014 4:12:05 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using System.Collections.Generic;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;

    /// <summary>
    /// The interface of Monthly Main Dish Per .
    /// </summary>
    public interface IMonthlyMainDishPerDayReportService
    {
        MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria);

        MonthlyMainDishPerDayModel GetPagingReport(MonthlyMainDishPerDayModel fullReport, int startIndex, int itemsPerPage);

        IList<BreakRuleOrder> GetBreakRuleOrder(GetBreakRuleOrderCriteria criteria);

        void ExportMonthlyMainDishPerDayReport(MonthlyMainDishPerDayModel report, string templatePath);
    }
}
