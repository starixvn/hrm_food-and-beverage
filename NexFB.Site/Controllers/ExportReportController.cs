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
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;

    /// <summary>
    /// Export report controller.
    /// </summary>
    public class ExportReportController : Controller
    {
        private const string _MonthlyMainDishReportLocalEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeeReportLocal.xls";
        private const string _MonthlyMainDishReportLocalDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentReportLocal.xls";
        private const string _EmployeeOrderHistoryTemplatePath = @"~\Src\App\Report\ExcelTemplates\EmployeeOrderHistoryReport.xls";
        private const string _SideDishReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\SideDishReport.xls";
        private const string _MonthlyMainDishPerDayEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeePerDayReport.xls";
        private const string _MonthlyMainDishPerDayDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentPerDayReport.xls";
        private const string _MealForecastAllTypeReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastAllTypeReport.xls";
        private const string _MealForecastReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastReport.xls";
        private const string _DailyMainDishReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\DailyMainDishReport.xls";
        private const string _MealForecastDetailReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastDetailReport.xls";

        private readonly IMonthlyMainDishReportService _monthlyMainDishReportService;
        private readonly IEmployeeOrderHistoryService _employeeOrderHistoryService;
        private readonly IMonthlyMainDishPerDayReportService _monthlyMainDishPerDayService;
        private readonly IMealForecastReportService _mealForecastReportService;
        private readonly IDailyMainDishReportService _dailyMainDishReportService;
        private readonly ISideDishReportService _sideDishReportService;
        private readonly IMealForecastDetailReportService _mealForecastDetailReportService;

        public ExportReportController()
            : this(null, null, null, null, null, null, null)
        {
        }

        public ExportReportController(
            IEmployeeOrderHistoryService employeeOrderHistoryService,
            IMonthlyMainDishReportService monthlyMainDishReportService,
            IDailyMainDishReportService dailyMainDishReportService,
            IMealForecastReportService mealForecastReportService,
            IMealForecastDetailReportService mealForecastDetailReportService,
            IMonthlyMainDishPerDayReportService monthlyMainDishPerDayService,
            ISideDishReportService sideDishReportService)
        {
            _monthlyMainDishReportService = monthlyMainDishReportService ?? new MonthlyMainDishReportService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
            _monthlyMainDishPerDayService = monthlyMainDishPerDayService ?? new MonthlyMainDishPerDayReportService();
            _mealForecastReportService = mealForecastReportService ?? new MealForecastReportService();
            _dailyMainDishReportService = dailyMainDishReportService ?? new DailyMainDishReportService();
            _sideDishReportService = sideDishReportService ?? new SideDishReportService();
            _mealForecastDetailReportService = mealForecastDetailReportService ?? new MealForecastDetailReportService();
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
                report = _monthlyMainDishReportService.GetMonthlyMainDishReportLocal(criteria);
            }

            if (report != null)
            {
                if (report.IsGroupByEmployee)
                {
                    templateUrl = Server.MapPath(_MonthlyMainDishReportLocalEmployeeTemplatePath);
                }
                else
                {
                    templateUrl = Server.MapPath(_MonthlyMainDishReportLocalDepartmentTemplatePath);
                }

                _monthlyMainDishReportService.ExportMonthlyMainDishReportLocal(report, templateUrl);
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
                templatePath = Server.MapPath(_EmployeeOrderHistoryTemplatePath);
                _employeeOrderHistoryService.ExportEmployeeOrderHistoryReport(report, templatePath);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Exports the side dish report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The EmptyResult.</returns>
        public ActionResult ExportSideDishReport(GetSideDishReportCriteria criteria)
        {
            string templateUrl = Server.MapPath(_SideDishReportTemplatePath);
            SideDishReportModel report = _sideDishReportService.GetSideDishReport(criteria);

            if (report != null)
            {
                _sideDishReportService.ExportSideDishReport(report, templateUrl);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Export monthly main dish per day report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The excel file.</returns>
        public ActionResult ExportMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria)
        {
            MonthlyMainDishPerDayModel report = SessionManager.Get<MonthlyMainDishPerDayModel>(
                ReportConstant.MonthlyMainDishPerDayReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = string.Empty;

            if (report == null)
            {
                report = _monthlyMainDishPerDayService.GetMonthlyMainDishPerDayReport(criteria);
            }

            if (report != null)
            {
                if (report.IsGroupByEmployee)
                {
                    templatePath = Server.MapPath(_MonthlyMainDishPerDayEmployeeTemplatePath);
                }
                else
                {
                    templatePath = Server.MapPath(_MonthlyMainDishPerDayDepartmentTemplatePath);
                }

                _monthlyMainDishPerDayService.ExportMonthlyMainDishPerDayReport(report, templatePath);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Export daily main dish report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The excel file.</returns>
        public ActionResult ExportDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria)
        {
            DailyMainDishReportModel report = SessionManager.Get<DailyMainDishReportModel>(
                ReportConstant.DailyMainDishReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = Server.MapPath(_DailyMainDishReportTemplatePath);

            if (report == null)
            {
                report = _dailyMainDishReportService.GetDailyMainDishReport(criteria);
            }

            _dailyMainDishReportService.ExportDailyMainDishReport(report, templatePath);
            return new EmptyResult();
        }

        /// <summary>
        /// Export Meal Forecast Report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The excel file.</returns>
        public ActionResult ExportMealForecastReport(GetMealForecastReportCriteria criteria)
        {
            MealForecastReportModel report = SessionManager.Get<MealForecastReportModel>(ReportConstant.MealForecastReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = string.Empty;

            if (report == null)
            {
                report = _mealForecastReportService.GetMealForecastReport(criteria);
            }

            if (report != null)
            {
                if (criteria.IsVegetarian == null)
                {
                    templatePath = Server.MapPath(_MealForecastAllTypeReportTemplatePath);
                }
                else
                {
                    templatePath = Server.MapPath(_MealForecastReportTemplatePath);
                }

                _mealForecastReportService.ExportMealForecastReport(report, templatePath, criteria.IsVegetarian);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Export Meal Forecast Detail Report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The excel file.</returns>
        public ActionResult ExportMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria)
        {
            MealForecastDetailReport report = SessionManager.Get<MealForecastDetailReport>(
                ReportConstant.MealForecastDetailReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = string.Empty;

            if (report == null)
            {
                report = _mealForecastDetailReportService.GetMealForecastDetailReport(criteria);
            }

            if (report != null)
            {
                templatePath = Server.MapPath(_MealForecastDetailReportTemplatePath);
                _mealForecastDetailReportService.ExportMealForecastDetailReport(report, templatePath, criteria.IsVegetarian, criteria.OnDate);
            }

            return new EmptyResult();
        }
    }
}