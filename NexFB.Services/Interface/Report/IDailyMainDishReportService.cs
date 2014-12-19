// ----------------------------------------------------------------------
// <copyright file="IDailyMainDishReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/18/2014 5:22:50 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;

    /// <summary>
    /// The interface of Daily Main Dish Report service.
    /// </summary>
    public interface IDailyMainDishReportService
    {
        DailyMainDishReportModel GetDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria);

        DailyMainDishReportModel GetPagingReport(DailyMainDishReportModel report, int startIndex, int itemsPerPage);
    }
}
