// ----------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/5/2014 8:58:36 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using Fanex.Common.Helper;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The implement of report service.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService()
            : this(null)
        {
        }

        public ReportService(IReportRepository reportRepository)
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
            }

            return report;
        }
    }
}
