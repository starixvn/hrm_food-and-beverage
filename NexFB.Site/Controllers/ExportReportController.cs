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
    using System.Web.Mvc;
    using NexFB.Core.Constants;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web;

    /// <summary>
    /// Export report controller.
    /// </summary>
    public class ExportReportController : Controller
    {
        private const string MonthlyMainDishReportLocalEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeeReportLocal.xls";
        private const string MonthlyMainDishReportLocalDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentReportLocal.xls";

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
            MonthlyLocalReportModel report = SessionManager.Get<MonthlyLocalReportModel>(
                ReportConstant.MonthlyMainDishReportLocalSessionKey + SessionManager.CurrentUser.UserId);
            string templateUrl = string.Empty;

            if (report == null)
            {
                report = _reportService.GetMonthlyMainDishReportLocal(criteria);
            }

            if (report != null)
            {
                if (report.IsGroupByEmployee)
                {
                    templateUrl = Server.MapPath(MonthlyMainDishReportLocalEmployeeTemplatePath);
                }
                else
                {
                    templateUrl = Server.MapPath(MonthlyMainDishReportLocalDepartmentTemplatePath);
                }

                _reportService.ExportMonthlyMainDishReportLocal(report, templateUrl);
            }

            return new EmptyResult();
        }
    }
}
