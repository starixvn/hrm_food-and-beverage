namespace NexFB.Site.Controllers.API.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Report;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.AdminOrderHistoryReport;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Models.Report.SideDishSummaryReport;
    using NexFB.Models.Report.ViewMenuReport;
    using NexFB.Models.SideDishManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;
    using NexFB.Web.Attributes;

    public class ReportController : BaseController
    {
        private readonly IMonthlyMainDishReportService _monthlyMainDishReportService;
        private readonly IDailyMainDishReportService _dailyMainDishReportService;
        private readonly ICommonService _commonService;
        private readonly IUserControlService _userControlService;
        private readonly IEmployeeOrderHistoryService _employeeOrderHistoryService;
        private readonly IMonthlyMainDishPerDayReportService _monthlyPerDayService;
        private readonly ISideDishReportService _sideDishReportService;
        private readonly IMealForecastReportService _mealForecastReportService;
        private readonly IViewMenuReportService _viewMenuReportService;
        private readonly ISideDishService _sideDishService;
        private readonly IMealForecastDetailReportService _mealForecastDetailReportService;
        private readonly ISideDishSummaryReportService _sideDishSummaryReportService;
        private readonly IEmployeeService _employeeService;
        private readonly IAdminOrderHistoryService adminOrderHistoryService;

        public ReportController()
            : this(null, null, null, null, null, null, null, null, null, null, null, null, null, null)
        {
        }

        public ReportController(
            IMonthlyMainDishReportService monthlyMainDishReportService,
            IDailyMainDishReportService dailyMainDishReportService,
            ICommonService commonService,
            IUserControlService userControlService,
            IEmployeeOrderHistoryService employeeOrderHistoryService,
            IMonthlyMainDishPerDayReportService monthlyPerDayService,
            ISideDishReportService sideDishReportService,
            IMealForecastReportService mealForecastReportService,
            IViewMenuReportService viewMenuReportService,
            IMealForecastDetailReportService mealForecastDetailReportService,
            ISideDishService sideDishService,
            ISideDishSummaryReportService sideDishSummaryReportService,
            IEmployeeService employeeService,
            IAdminOrderHistoryService adminOrderHistoryService)
        {
            _monthlyMainDishReportService = monthlyMainDishReportService ?? new MonthlyMainDishReportService();
            _dailyMainDishReportService = dailyMainDishReportService ?? new DailyMainDishReportService();
            _commonService = commonService ?? new CommonService();
            _userControlService = userControlService ?? new UserControlService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
            _monthlyPerDayService = monthlyPerDayService ?? new MonthlyMainDishPerDayReportService();
            _sideDishReportService = sideDishReportService ?? new SideDishReportService();
            _mealForecastReportService = mealForecastReportService ?? new MealForecastReportService();
            _viewMenuReportService = viewMenuReportService ?? new ViewMenuReportService();
            _mealForecastDetailReportService = mealForecastDetailReportService ?? new MealForecastDetailReportService();
            _sideDishService = sideDishService ?? new SideDishService();
            _sideDishSummaryReportService = sideDishSummaryReportService ?? new SideDishSummaryReportService();
            _employeeService = employeeService ?? new EmployeeService();
            this.adminOrderHistoryService = adminOrderHistoryService ?? new AdminOrderHistoryService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MonthlyMainDishReportPerMeal, Policy = Policy.Read)]
        public MonthlyLocalReportModel GetMonthlyMainDishReportLocal([FromUri] GetMonthlyMainDishReportLocalCriteria criteria)
        {
            MonthlyLocalReportModel fullReport = null;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId);
            }

            if (fullReport == null)
            {
                fullReport = _monthlyMainDishReportService.GetMonthlyMainDishReportLocal(criteria);
                if (fullReport.Meals.Count > 0 && criteria.GroupBy == 0)
                {
                    var criteriaShift = new GetEmployeeShiftInMonthCriteria
                    {
                        ReportDate = criteria.ReportDate
                    };
                    var shifts = _employeeService.GetEmployeeShiftInMonth(criteriaShift);
                    AddShiftToReport(fullReport, shifts);
                }
                SessionManager.Set<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId, fullReport);
            }

            MonthlyLocalReportModel pagingReport = _monthlyMainDishReportService.GetPaingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        private static void AddShiftToReport(MonthlyLocalReportModel fullReport, IList<EmployeeShift> shifts)
        {
            for (int i = 0; i < fullReport.Meals.Count; i++)
            {
                AddShiftToMeal(fullReport.Meals[i], shifts);
            }
        }

        private static void AddShiftToMeal(MealMonthModel mealMonthModel, IList<EmployeeShift> shifts)
        {
            foreach (var employee in mealMonthModel.MealItems)
            {
                var id = employee.Employee.UserId;
                var result = shifts.Where(x => x.SysID == id).OrderBy(x => x.WorkDate);
                employee.NumberMealsMonth.Shifts = result.Select(x => x.Shift).ToList();
            }
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.DailyMainDishReport, Policy = Policy.Read)]
        public DailyMainDishReportModel GetDailyMainDishReport([FromUri] GetDailyMainDishPricePerMealReportCriteria criteria)
        {
            DailyMainDishReportModel fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<DailyMainDishReportModel>(ReportConstant.DailyMainDishReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _dailyMainDishReportService.GetDailyMainDishReport(criteria);
                SessionManager.Set<DailyMainDishReportModel>(ReportConstant.DailyMainDishReportSessionKey + CurrentUser.UserId, fullReport);
            }

            return _dailyMainDishReportService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishReport, Policy = Policy.Read)]
        public SideDishReportModel GetSideDishReport([FromUri] GetSideDishReportCriteria criteria)
        {
            SideDishReportModel reportData = _sideDishReportService.GetSideDishReport(criteria);
            return reportData;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishSummaryReport, Policy = Policy.Read)]
        public SideDishSummaryReportModel GetSideDishSummaryReport([FromUri] GetSideDishSummaryReportCriteria criteria)
        {
            SideDishSummaryReportModel report = _sideDishSummaryReportService.GetSideDishSummaryReport(criteria);
            return report;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishSummaryReport, Policy = Policy.Read)]
        [HttpGet]
        public Employee GetEmployeeById(int userId)
        {
            var criteria = new GetEmployeeByIdCriteria
            {
                UserId = userId
            };
            Employee user = _employeeService.GetEmployeeById(criteria);
            return user;
        }

        public IEnumerable<SideDishModel> GetAllSideDish(
            int? foodId = null,
            bool? isActive = null,
            string employeeTypeId = null)
        {
            var criteria = new GetAllSideDishCriteria
            {
                IsActive = isActive,
                FoodId = foodId,
                EmployeeTypeId = employeeTypeId
            };

            return _sideDishService.GetAllSideDish(criteria);
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.ViewMenu, Policy = Policy.Read)]
        public ViewMenuReportModel GetViewMenuReport(
            DateTime fromDate,
            DateTime toDate,
            string mealId,
            bool? isVegetarian)
        {
            var criteria = new GetViewMenuReportCriteria
            {
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate,
                IsVegetarian = isVegetarian,
                SysId = CurrentUser.UserId
            };

            ViewMenuReportModel reportData = _viewMenuReportService.GetViewMenuReport(criteria);
            return reportData;
        }

        [HttpGet]
        public IEnumerable<Company> GetCompanyByUserRole()
        {
            IEnumerable<Company> companies = null;
            const byte financeRole = (byte)RolesSystem.Finance;
            const byte employeeRole = (byte)RolesSystem.Employee;
            const byte adminRole = (byte)RolesSystem.Admin;
            const byte kitchenRole = (byte)RolesSystem.Kitchen;
            const byte systemAdminRole = (byte)RolesSystem.SA;

            string roles = CurrentUser.Roles;

            if (!string.IsNullOrEmpty(roles))
            {
                bool isContainSystemAdminRole = roles.Contains(systemAdminRole.ToString());
                bool isContainAdminRole = roles.Contains(adminRole.ToString());
                bool isContainEmployeeRole = roles.Contains(employeeRole.ToString());
                bool isContainFinanceRole = roles.Contains(financeRole.ToString());
                bool isContainKitchenRole = roles.Contains(kitchenRole.ToString());

                if (isContainSystemAdminRole || isContainAdminRole ||
                    (!isContainFinanceRole && (isContainEmployeeRole || isContainKitchenRole)))
                {
                    companies = _commonService.GetAllCompany();
                }
                else if (isContainFinanceRole)
                {
                    var criteria = new GetEmployeeRoleByIdCriteria
                    {
                        SystemId = CurrentUser.UserId,
                        RoleId = financeRole.ToString()
                    };
                    companies = _userControlService.GetCompanyUserHasPermission(criteria);
                }
            }

            return companies;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.EmployeeOrderHistoricalReport, Policy = Policy.Read)]
        public EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport([FromUri] GetEmployeeOrderHistoryCriteria criteria)
        {
            EmployeeOrderHistoryReport fullReport;
            GetOrderedByEnum();
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeOrderHistoryReport>(ReportConstant.EmployeeOrderHistoryReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _employeeOrderHistoryService.GetEmployeeOrderHistoryReport(criteria);
                SessionManager.Set<EmployeeOrderHistoryReport>(ReportConstant.EmployeeOrderHistoryReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeOrderHistoryReport pagingReport = _employeeOrderHistoryService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MonthlyMainDishReportPerDay, Policy = Policy.Read)]
        public MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport([FromUri] GetMonthlyMainDishPerDayReportCriteria criteria)
        {
            MonthlyMainDishPerDayModel fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<MonthlyMainDishPerDayModel>(
                        ReportConstant.MonthlyMainDishPerDayReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _monthlyPerDayService.GetMonthlyMainDishPerDayReport(criteria);
                SessionManager.Set<MonthlyMainDishPerDayModel>(
                        ReportConstant.MonthlyMainDishPerDayReportSessionKey + CurrentUser.UserId, fullReport);
            }

            MonthlyMainDishPerDayModel pagingReport = _monthlyPerDayService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public MealForecastReportModel GetMealForecastReport([FromUri] GetMealForecastReportCriteria criteria)
        {
            MealForecastReportModel fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<MealForecastReportModel>(ReportConstant.MealForecastReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _mealForecastReportService.GetMealForecastReport(criteria);
                SessionManager.Set<MealForecastReportModel>(ReportConstant.MealForecastReportSessionKey + CurrentUser.UserId, fullReport);
            }

            MealForecastReportModel pagingReport = _mealForecastReportService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public MealForecastDetailReport GetMealForecastDetailReport([FromUri] GetMealForecastDetailReportCriteria criteria)
        {
            MealForecastDetailReport fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<MealForecastDetailReport>(ReportConstant.MealForecastDetailReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _mealForecastDetailReportService.GetMealForecastDetailReport(criteria);
                SessionManager.Set<MealForecastDetailReport>(ReportConstant.MealForecastDetailReportSessionKey + CurrentUser.UserId, fullReport);
            }

            MealForecastDetailReport pagingReport = _mealForecastDetailReportService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public EmployeeRegistrationReportModel GetEmployeeRegistration([FromUri] GetEmployeeRegistrationCriteria criteria)
        {
            EmployeeRegistrationReportModel fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeRegistrationReportModel>(ReportConstant.EmployeeRegistrationReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _mealForecastDetailReportService.GetEmployeeRegistration(criteria);
                SessionManager.Set<EmployeeRegistrationReportModel>(ReportConstant.EmployeeRegistrationReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeRegistrationReportModel pagingReport = _mealForecastDetailReportService.GetPagingEmployeeRegistration(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public EmployeeRegistrationReportModel GetEmployeeNonRegistration([FromUri] GetEmployeeNonRegistrationCriteria criteria)
        {
            EmployeeRegistrationReportModel fullReport;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeRegistrationReportModel>(ReportConstant.EmployeeNonRegistrationReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _mealForecastDetailReportService.GetEmployeeNonRegistration(criteria);
                SessionManager.Set<EmployeeRegistrationReportModel>(ReportConstant.EmployeeNonRegistrationReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeRegistrationReportModel pagingReport = _mealForecastDetailReportService.GetPagingEmployeeNonRegistration(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public AdminOrderHistoryReport GetAdminOrderHistoryReport([FromUri] GetAdminOrderHistoryCriteria criteria)
        {
            AdminOrderHistoryReport fullReport = null;
            if (criteria.IsPagingCallback)
            {
                fullReport = SessionManager.Get<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId);
            }

            if (fullReport == null)
            {
                fullReport = adminOrderHistoryService.GetAdminOrderHistoryReport(criteria);
                SessionManager.Set<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId, fullReport);
            }

            var pagingReport = adminOrderHistoryService.GetPagingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);

            return pagingReport;
        }

        [HttpGet]
        public List<object> GetDayType()
        {
            var dayTypeList = new List<object>();

            foreach (DayType dayType in Enum.GetValues(typeof(DayType)).Cast<DayType>())
            {
                var dayTypeItem = new
                {
                    Value = (int)dayType,
                    Name = Enumerations.GetDescription(dayType)
                };
                dayTypeList.Add(dayTypeItem);
            }

            return dayTypeList;
        }

        [HttpGet]
        public List<object> GetOrderedByEnum()
        {
            var orderedByList = new List<object>();

            foreach (OrderedBy orderedByEnum in (OrderedBy[])Enum.GetValues(typeof(OrderedBy)))
            {
                var orderedBy = new
                {
                    Value = (int)orderedByEnum,
                    Name = Enumerations.GetDescription(orderedByEnum)
                };

                orderedByList.Add(orderedBy);
            }

            return orderedByList;
        }

        [HttpGet]
        public List<object> GetOrderPlatformEnum()
        {
            var orderPlatformList = new List<object>();

            foreach (OrderPlatform orderPlatformEnum in (OrderPlatform[])Enum.GetValues(typeof(OrderPlatform)))
            {
                var orderPlatform = new
                {
                    Value = (int)orderPlatformEnum,
                    Name = Enumerations.GetDescription(orderPlatformEnum)
                };

                orderPlatformList.Add(orderPlatform);
            }

            return orderPlatformList;
        }

        [HttpGet]
        public List<object> GetOrderStatusEnum()
        {
            var orderStatues = new List<object>();

            foreach (OrderStatus orderStatusEnum in (OrderStatus[])Enum.GetValues(typeof(OrderStatus)))
            {
                if (orderStatusEnum != OrderStatus.None && orderStatusEnum != OrderStatus.DeletedOrder)
                {
                    var orderStatus = new
                    {
                        Value = (int)orderStatusEnum,
                        Name = Enumerations.GetDescription(orderStatusEnum)
                    };

                    orderStatues.Add(orderStatus);
                }
            }

            return orderStatues;
        }

        [HttpGet]
        public List<object> GetCompanyEnum()
        {
            var companies = new List<object>();

            foreach (CompanyIdentity companyEnum in (CompanyIdentity[])Enum.GetValues(typeof(CompanyIdentity)))
            {
                if (companyEnum != CompanyIdentity.None)
                {
                    var company = new
                    {
                        Value = (int)companyEnum,
                        Name = Enumerations.GetDescription(companyEnum)
                    };

                    companies.Add(company);
                }
            }

            return companies;
        }

        [HttpGet]
        public List<object> GetMenuTypeEnum()
        {
            var menuTypes = new List<object>();

            foreach (FoodMenuType menuTypeEnum in (FoodMenuType[])Enum.GetValues(typeof(FoodMenuType)))
            {
                var menuType = new
                {
                    Value = (int)menuTypeEnum,
                    Name = Enumerations.GetDescription(menuTypeEnum)
                };

                menuTypes.Add(menuType);
            }

            return menuTypes;
        }
    }
}