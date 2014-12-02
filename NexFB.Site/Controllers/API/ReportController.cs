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
    using System.Web;
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
        private const string MonthlyMainDishReportLocalGroupByEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishReportLocal.xls";
        private const string MonthlyMainDishReportLocalTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishReportLocal.xls";

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
                IsLocal = isLocal
            };

            var report = _reportService.GetMonthlyMainDishReportLocal(criteria);
            return report;
        }

        /// <summary>
        /// Export monthly main dish report in local.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <param name="departmentId">The department Id.</param>
        /// <param name="employeeTypeId">The employee type Id.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="groupBy">The group by.</param>
        [HttpGet]
        public void ExportMonthlyMainDishReport(
            DateTime reportDate,
            string departmentId = "",
            string employeeTypeId = "",
            string teamId = "",
            string mealId = "",
            int groupBy = 0)
        {
            var criteria = new GetMonthlyMainDishReportLocalCriteria
            {
                ReportDate = reportDate,
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
                MealId = mealId,
                GroupBy = groupBy
            };

            var report = _reportService.GetMonthlyMainDishReportLocal(criteria);
            string templateUrl = string.Empty;

            if (report != null)
            {
                if (report.IsGroupByEmployee)
                {
                    templateUrl = HttpContext.Current.Server.MapPath(MonthlyMainDishReportLocalGroupByEmployeeTemplatePath);
                }
                else
                {
                    templateUrl = HttpContext.Current.Server.MapPath(MonthlyMainDishReportLocalGroupByEmployeeTemplatePath);
                }

                _reportService.ExportMonthlyMainDishReportLocal(report, templateUrl);
            }
        }
    }
}