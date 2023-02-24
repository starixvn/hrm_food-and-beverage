namespace NexFB.Site.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Report;
    using NexFB.Criteria.ShiftMealMapping;
    using NexFB.Models.Authority;
    using NexFB.Models.Report.AdminOrderHistoryReport;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Models.Report.SideDishSummaryReport;
    using NexFB.Models.Report.ViewMenuReport;
    using NexFB.Models.ShiftMealMapping;
    using NexFB.Services.Implement;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;

    public class ExportReportController : Controller
    {
        private const string _MonthlyMainDishReportLocalEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeeReportLocal.xls";
        private const string _MonthlyMainDishReportLocalDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentReportLocal.xls";
        private const string _MonthlyMainDishReportLocalCostCenterTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishCostCenterReportLocal.xls";
        private const string _EmployeeOrderHistoryTemplatePath = @"~\Src\App\Report\ExcelTemplates\EmployeeOrderHistoryReport.xls";
        private const string _AdminOrderHistoryTemplatePath = @"~\Src\App\Report\ExcelTemplates\AdminOrderHistoryReport.xls";
        private const string _SideDishReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\SideDishReport.xls";
        private const string _ShiftMealMappingReport = @"~\Src\App\Report\ExcelTemplates\ShiftMealMappingReport.xls";
        private const string _MonthlyMainDishPerDayEmployeeTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishEmployeePerDayReport.xls";
        private const string _MonthlyMainDishPerDayDepartmentTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyMainDishDepartmentPerDayReport.xls";
        private const string _MealForecastAllTypeReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastAllTypeReport.xls";
        private const string _MealForecastReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastReport.xls";
        private const string _ViewMenuReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\ViewMenuReport.xls";
        private const string _SideDishSummaryReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\SideDishSummaryReport.xls";
        private const string _DailyMainDishReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\DailyMainDishReport.xls";
        private const string _MealForecastDetailReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MealForecastDetailReport.xls";
        private const string _EmployeeRegistrationReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\EmployeeRegistrationDetailReport.xls";
        private const string _EmployeeNonRegistrationReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\EmployeeNonRegistrationDetailReport.xls";
        private const string _FoodItemReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\FoodItemReport.xls";
        private const string _MonthlyLunchAndDinnerAmountReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyLunchAndDinnerAmoutReport.xls";
        private const string _MonthlyBreakfastAndSupperAmountReportTemplatePath = @"~\Src\App\Report\ExcelTemplates\MonthlyBreakfastAndSupperAmoutReport.xls";

        private readonly IMonthlyMainDishReportService _monthlyMainDishReportService;
        private readonly IEmployeeOrderHistoryService _employeeOrderHistoryService;
        private readonly IAdminOrderHistoryService _adminOrderHistoryService;
        private readonly IMonthlyMainDishPerDayReportService _monthlyMainDishPerDayService;
        private readonly IMealForecastReportService _mealForecastReportService;
        private readonly IDailyMainDishReportService _dailyMainDishReportService;
        private readonly IViewMenuReportService _viewMenuReportService;
        private readonly ISideDishReportService _sideDishReportService;
        private readonly IMealForecastDetailReportService _mealForecastDetailReportService;
        private readonly ISideDishSummaryReportService _sideDishSummaryReportService;
        private readonly IFoodItemService _foodItemService;
        private readonly IFoodItemReportService _foodItemReportService;
        private readonly IMonthlyMealTotalAmountService _monthlyMealTotalAmountService;
        private readonly IShiftMealMappingService _shiftMealMappingService;

        public ExportReportController()
            : this(null, null, null, null, null, null, null, null, null, null, null, null, null, null)
        {
        }

        [SuppressMessage("Critical Code Smell", "S1541:Methods and properties should not be too complex", Justification = "<Pending>")]
        [SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "<Pending>")]
        public ExportReportController(
            IEmployeeOrderHistoryService employeeOrderHistoryService,
            IAdminOrderHistoryService adminOrderHistoryService,
            IMonthlyMainDishReportService monthlyMainDishReportService,
            IDailyMainDishReportService dailyMainDishReportService,
            IMealForecastReportService mealForecastReportService,
            IViewMenuReportService viewMenuReportService,
            IMealForecastDetailReportService mealForecastDetailReportService,
            IMonthlyMainDishPerDayReportService monthlyMainDishPerDayService,
            ISideDishReportService sideDishReportService,
            ISideDishSummaryReportService sideDishSummaryReportService,
            IFoodItemService foodItemService,
            IFoodItemReportService foodItemReportService,
            IMonthlyMealTotalAmountService monthlyMealTotalAmountService,
            IShiftMealMappingService shiftMealMappingService)
        {
            _monthlyMainDishReportService = monthlyMainDishReportService ?? new MonthlyMainDishReportService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
            _monthlyMainDishPerDayService = monthlyMainDishPerDayService ?? new MonthlyMainDishPerDayReportService();
            _mealForecastReportService = mealForecastReportService ?? new MealForecastReportService();
            _dailyMainDishReportService = dailyMainDishReportService ?? new DailyMainDishReportService();
            _viewMenuReportService = viewMenuReportService ?? new ViewMenuReportService();
            _sideDishReportService = sideDishReportService ?? new SideDishReportService();
            _mealForecastDetailReportService = mealForecastDetailReportService ?? new MealForecastDetailReportService();
            _sideDishSummaryReportService = sideDishSummaryReportService ?? new SideDishSummaryReportService();
            _adminOrderHistoryService = adminOrderHistoryService ?? new AdminOrderHistoryService();
            _foodItemService = foodItemService ?? new FoodItemService();
            _foodItemReportService = foodItemReportService ?? new FoodItemReportService();
            _monthlyMealTotalAmountService = monthlyMealTotalAmountService ?? new MonthlyMealTotalAmountService();
            _shiftMealMappingService = shiftMealMappingService ?? new ShiftMealMappingService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportMonthlyMainDishReport(GetMonthlyMainDishReportLocalCriteria criteria)
        {
            var report = SessionManager.Get<MonthlyLocalReportModel>(
                ReportConstant.MonthlyMainDishReportLocalSessionKey + SessionManager.CurrentUser.UserId);
            string templateUrl = string.Empty;

            report = report ?? _monthlyMainDishReportService.GetMonthlyMainDishReportLocal(criteria);

            if (report != null)
            {
                if (report.GroupBy == GroupByReport.Employee)
                {
                    templateUrl = Server.MapPath(_MonthlyMainDishReportLocalEmployeeTemplatePath);
                }

                if (report.GroupBy == GroupByReport.Company)
                {
                    templateUrl = Server.MapPath(_MonthlyMainDishReportLocalDepartmentTemplatePath);
                }

                if (report.GroupBy == GroupByReport.CostCenter)
                {
                    templateUrl = Server.MapPath(_MonthlyMainDishReportLocalCostCenterTemplatePath);
                }

                _monthlyMainDishReportService.ExportMonthlyMainDishReportLocal(report, templateUrl);
            }

            return new EmptyResult();
        }

        public ActionResult ExportEmployeeOrderHistoryReport(GetEmployeeOrderHistoryCriteria criteria)
        {
            var report = SessionManager.Get<EmployeeOrderHistoryReport>(
                ReportConstant.EmployeeOrderHistoryReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _employeeOrderHistoryService.GetEmployeeOrderHistoryReport(criteria);

            if (report != null)
            {
                string templatePath = Server.MapPath(_EmployeeOrderHistoryTemplatePath);
                _employeeOrderHistoryService.ExportEmployeeOrderHistoryReport(report, templatePath);
            }

            return new EmptyResult();
        }

        public ActionResult ExportAdminOrderHistoryReport(GetAdminOrderHistoryCriteria criteria)
        {
            var report = SessionManager.Get<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId);

            report = report ?? _adminOrderHistoryService.GetAdminOrderHistoryReport(criteria);

            if (report != null)
            {
                var templatePath = Server.MapPath(_AdminOrderHistoryTemplatePath);
                _adminOrderHistoryService.ExportAdminOrderHistoryReport(report, templatePath);
            }

            return new EmptyResult();
        }

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

        public ActionResult ExportShiftMealMapping(GetAllShiftMealMappingCriteria criteria)
        {
            string templateUrl = Server.MapPath(_ShiftMealMappingReport);
            IEnumerable<ShiftMealMappingModel> report = _shiftMealMappingService.GetAllShiftMeal(criteria);
            if (report != null)
            {
                _shiftMealMappingService.ExportShiftMealMapping(report, templateUrl);
            }

            return new EmptyResult();
        }

        public ActionResult ExportMonthlyMainDishPerDayReport(GetMonthlyMainDishPerDayReportCriteria criteria)
        {
            var report = SessionManager.Get<MonthlyMainDishPerDayModel>(
                ReportConstant.MonthlyMainDishPerDayReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _monthlyMainDishPerDayService.GetMonthlyMainDishPerDayReport(criteria);

            if (report != null)
            {
                var templatePath = report.IsGroupByEmployee
                    ? Server.MapPath(_MonthlyMainDishPerDayEmployeeTemplatePath)
                    : Server.MapPath(_MonthlyMainDishPerDayDepartmentTemplatePath);

                _monthlyMainDishPerDayService.ExportMonthlyMainDishPerDayReport(report, templatePath);
            }

            return new EmptyResult();
        }

        public ActionResult ExportViewMenuReport(GetViewMenuReportCriteria criteria)
        {
            UserSession user = SessionManager.CurrentUser;
            if (user != null)
            {
                criteria.SysId = user.UserId;
            }

            string templatePath = Server.MapPath(_ViewMenuReportTemplatePath);
            ViewMenuReportModel reportData = _viewMenuReportService.GetViewMenuReport(criteria);
            if (reportData != null)
            {
                _viewMenuReportService.ExportViewMenuReport(reportData, templatePath, criteria.FromDate, criteria.ToDate);
            }

            return new EmptyResult();
        }

        public ActionResult ExportDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria)
        {
            var report = SessionManager.Get<DailyMainDishReportModel>(
                ReportConstant.DailyMainDishReportSessionKey + SessionManager.CurrentUser.UserId);
            string templatePath = Server.MapPath(_DailyMainDishReportTemplatePath);

            report = report ?? _dailyMainDishReportService.GetDailyMainDishReport(criteria);

            _dailyMainDishReportService.ExportDailyMainDishReport(report, templatePath);
            return new EmptyResult();
        }

        public ActionResult ExportMealForecastReport(GetMealForecastReportCriteria criteria)
        {
            var report = SessionManager.Get<MealForecastReportModel>(
                ReportConstant.MealForecastReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _mealForecastReportService.GetMealForecastReport(criteria);

            if (report != null)
            {
                var templatePath = criteria.IsVegetarian == null
                    ? Server.MapPath(_MealForecastAllTypeReportTemplatePath)
                    : Server.MapPath(_MealForecastReportTemplatePath);

                _mealForecastReportService.ExportMealForecastReport(report, templatePath, criteria.IsVegetarian);
            }

            return new EmptyResult();
        }

        public ActionResult ExportMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria)
        {
            var report = SessionManager.Get<MealForecastDetailReport>(
                ReportConstant.MealForecastDetailReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _mealForecastDetailReportService.GetMealForecastDetailReport(criteria);

            if (report != null)
            {
                var templatePath = Server.MapPath(_MealForecastDetailReportTemplatePath);
                _mealForecastDetailReportService.ExportMealForecastDetailReport(report, templatePath, criteria.IsVegetarian, criteria.Date);
            }

            return new EmptyResult();
        }

        public ActionResult ExportEmployeeRegistrationDetailReport(GetEmployeeRegistrationCriteria criteria)
        {
            var report = SessionManager.Get<EmployeeRegistrationReportModel>(
                ReportConstant.EmployeeRegistrationReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _mealForecastDetailReportService.GetEmployeeRegistration(criteria);

            if (report != null)
            {
                var templatePath = Server.MapPath(_EmployeeRegistrationReportTemplatePath);
                _mealForecastDetailReportService.ExportEmployeeRegistrationReport(report, templatePath, criteria.IsVegetarian, criteria.Date);
            }

            return new EmptyResult();
        }

        public ActionResult ExportEmployeeNonRegistrationDetailReport(GetEmployeeNonRegistrationCriteria criteria)
        {
            var report = SessionManager.Get<EmployeeRegistrationReportModel>(
                ReportConstant.EmployeeNonRegistrationReportSessionKey + SessionManager.CurrentUser.UserId);

            report = report ?? _mealForecastDetailReportService.GetEmployeeNonRegistration(criteria);

            if (report != null)
            {
                var templatePath = Server.MapPath(_EmployeeNonRegistrationReportTemplatePath);
                _mealForecastDetailReportService.ExportEmployeeNonRegistrationReport(report, templatePath, criteria.IsVegetarian, criteria.Date);
            }

            return new EmptyResult();
        }

        public ActionResult ExportSideDishSummaryReport(GetSideDishSummaryReportCriteria criteria)
        {
            string templatePath = Server.MapPath(_SideDishSummaryReportTemplatePath);
            SideDishSummaryReportModel report = _sideDishSummaryReportService.GetSideDishSummaryReport(criteria);

            if (report != null)
            {
                _sideDishSummaryReportService.ExportSideDishSummaryReport(report, templatePath);
            }

            return new EmptyResult();
        }

        public ActionResult ExportFoodItemReport(string setMenuId = "", bool? isActive = null)
        {
            string templatePath = Server.MapPath(_FoodItemReportTemplatePath);
            var criteria = new GetAllFoodItemCriteria
            {
                SetMenuId = setMenuId,
                IsActive = isActive
            };

            var report = _foodItemService.GetAllFoodItem(criteria);
            if (report != null)
            {
                _foodItemReportService.ExportFoodItemReport(report, templatePath);
            }

            return new EmptyResult();
        }

        public ActionResult ExportMonthlyLunchAndDinnerAmountReport(string companyId, DateTime reportDate)
        {
            string templatePath = Server.MapPath(_MonthlyLunchAndDinnerAmountReportTemplatePath);

            _monthlyMealTotalAmountService.ExportMonthlyLunchAndDinnerReport(templatePath, reportDate, companyId);

            return new EmptyResult();
        }

        public ActionResult ExportMonthlyBreakfastAndSupperAmountReport(string companyId, DateTime reportDate)
        {
            string templatePath = Server.MapPath(_MonthlyBreakfastAndSupperAmountReportTemplatePath);

            _monthlyMealTotalAmountService.ExportMonthlyBreakfastAndSupperReport(templatePath, reportDate, companyId);

            return new EmptyResult();
        }
    }
}