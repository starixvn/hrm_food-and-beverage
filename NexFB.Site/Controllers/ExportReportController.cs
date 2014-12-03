// ----------------------------------------------------------------------
// <copyright file="ExportReportController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/3/2014 2:23:17 PM">Created.</change>
// </history>
             
namespace NexFB.Site.Controllers
{
    using System;
    using System.Web.Mvc;
    using NexFB.Criteria.Report;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Export report controller.
    /// </summary>
    public class ExportReportController : Controller
    {
        private const string MonthlyMainDishReportLocalGroupByEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishReportLocal.xls";
        private const string MonthlyMainDishReportLocalTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishReportLocal.xls";

        private readonly IReportService _reportService;

        public ExportReportController()
            : this(null)
        {
        }

        public ExportReportController(IReportService reportService)
        {
            _reportService = reportService ?? new ReportService();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Export monthly main dish report in local.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Excel file.</returns>
        public ActionResult ExportMonthlyMainDishReport(GetMonthlyMainDishReportLocalCriteria criteria)
        {
            var report = _reportService.GetMonthlyMainDishReportLocal(criteria);
            string templateUrl = string.Empty;

            if (report != null)
            {
                if (report.IsGroupByEmployee)
                {
                    templateUrl = Server.MapPath(MonthlyMainDishReportLocalGroupByEmployeeTemplatePath);
                }
                else
                {
                    templateUrl = Server.MapPath(MonthlyMainDishReportLocalGroupByEmployeeTemplatePath);
                }

                _reportService.ExportMonthlyMainDishReportLocal(report, templateUrl);
            }

            return new EmptyResult();
        }
    }
}
