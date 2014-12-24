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
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Services.Implement;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;

    /// <summary>
    /// Export report controller.
    /// </summary>
    public class ExportReportController : Controller
    {
        private const string MonthlyMainDishReportLocalEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeeReportLocal.xls";
        private const string MonthlyMainDishReportLocalDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentReportLocal.xls";
        private const string EmployeeOrderHistoryTemplatePath = @"~\Src\App\Report\ExcelTemplates\EmployeeOrderHistoryReport.xls";

        private readonly IReportService _reportService;
        private readonly IEmployeeOrderHistoryService _employeeOrderHistoryService;

        public ExportReportController()
            : this(null, null)
        {
        }

        public ExportReportController(IReportService reportService, IEmployeeOrderHistoryService employeeOrderHistoryService)
        {
            _reportService = reportService ?? new ReportService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
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

        /// <summary>
        /// Export Employee Order History Report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The excel file.</returns>
        public ActionResult ExportEmployeeOrderHistoryReport(GetEmployeeOrderHistoryCriteria criteria)
        {
            EmployeeOrderHistoryReport report = SessionManager.Get<EmployeeOrderHistoryReport>(
                ReportConstant.EmployeeOrderHistoryReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = string.Empty;

            if (report == null)
            {
                report = _employeeOrderHistoryService.GetEmployeeOrderHistoryReport(criteria);
            }

            if (report != null)
            {
                templatePath = Server.MapPath(EmployeeOrderHistoryTemplatePath);
                _employeeOrderHistoryService.ExportEmployeeOrderHistoryReport(report, templatePath);
            }

            return new EmptyResult();
        }
    }
}
