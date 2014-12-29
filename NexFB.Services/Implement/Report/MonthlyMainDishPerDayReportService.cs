// ----------------------------------------------------------------------
// <copyright file="MonthlyMainDishPerDayReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/26/2014 4:14:43 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of Monthly Main Dish Per Day report service.
    /// </summary>
    public class MonthlyMainDishPerDayReportService : BaseService, IMonthlyMainDishPerDayReportService
    {
        private readonly IReportRepository _reportRepository;

        public MonthlyMainDishPerDayReportService()
            : this(null)
        {
        }

        public MonthlyMainDishPerDayReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Get monthly main dish per day report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The report.</returns>
        public MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            MonthlyMainDishPerDayModel report = null;

            if (criteria.IsValid())
            {
                report = _reportRepository.GetMonthlyMainDishPerDayReport(criteria);
            }

            return report;
        }
    }
}
