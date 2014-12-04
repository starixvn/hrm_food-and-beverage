// ----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/5/2014 9:03:28 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Web.Http;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;

    /// <summary>
    /// The report controller.
    /// </summary>
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController()
            : this(null)
        {
        }

        public ReportController(IReportService reportService)
        {
            _reportService = reportService ?? new ReportService();
        }

        /// <summary>
        /// Get monthly main dish report local.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <param name="departmentId">The department Id.</param>
        /// <param name="employeeTypeId">The employee type Id.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="groupBy">The group by.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="isPagingCallback">The is paging callback.</param>
        /// <param name="isDebug">The is debug.</param>
        /// <param name="isLocal">The is local.</param>
        /// <returns>The report.</returns>
        [HttpGet]
        public MonthlyLocalReportModel GetMonthlyMainDishReportLocal(
            DateTime reportDate,
            string departmentId = "",
            string employeeTypeId = "",
            string teamId = "",
            string mealId = "",
            int groupBy = 0,
            int startIndex = 0,
            int itemsPerPage = 50,
            bool isPagingCallback = false,
            bool isDebug = false,
            int isLocal = 1)
        {
            var criteria = new GetMonthlyMainDishReportLocalCriteria
            {
                ReportDate = reportDate,
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
                MealId = mealId,
                GroupBy = groupBy,
                IsDebug = isDebug,
                IsLocal = isLocal,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
                IsPagingCallback = isPagingCallback
            };
            MonthlyLocalReportModel fullReport = null;

            if (criteria.IsPagingCallback)
            {
                 fullReport = SessionManager.Get<MonthlyLocalReportModel>("test");
            }

            if (fullReport == null)
            {
                fullReport = _reportService.GetMonthlyMainDishReportLocal(criteria);
                SessionManager.Set<MonthlyLocalReportModel>("test", fullReport);
            }

            MonthlyLocalReportModel pagingReport = _reportService.GetPaingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }
    }
}