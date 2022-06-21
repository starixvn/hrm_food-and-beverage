namespace NexFB.Site.Controllers.API.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Models.Report.MonthlyMealPriceReport;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Report;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Criteria.UserControl;
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
        private readonly IAdminOrderHistoryService _adminOrderHistoryService;
        private readonly IMonthlyMainDishPerDayReportService _monthlyPerDayService;
        private readonly ISideDishReportService _sideDishReportService;
        private readonly IMealForecastReportService _mealForecastReportService;
        private readonly IViewMenuReportService _viewMenuReportService;
        private readonly ISideDishService _sideDishService;
        private readonly IMealForecastDetailReportService _mealForecastDetailReportService;
        private readonly ISideDishSummaryReportService _sideDishSummaryReportService;
        private readonly IEmployeeService _employeeService;
        private readonly IMonthlyMealTotalAmountService _monthlyMealsTotalAmountService;

        public ReportController()
            : this(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)
        {
        }

        public ReportController(
            IMonthlyMainDishReportService monthlyMainDishReportService,
            IDailyMainDishReportService dailyMainDishReportService,
            ICommonService commonService,
            IUserControlService userControlService,
            IEmployeeOrderHistoryService employeeOrderHistoryService,
            IAdminOrderHistoryService adminOrderHistoryService,
            IMonthlyMainDishPerDayReportService monthlyPerDayService,
            ISideDishReportService sideDishReportService,
            IMealForecastReportService mealForecastReportService,
            IViewMenuReportService viewMenuReportService,
            IMealForecastDetailReportService mealForecastDetailReportService,
            ISideDishService sideDishService,
            ISideDishSummaryReportService sideDishSummaryReportService,
            IEmployeeService employeeService,
            IMonthlyMealTotalAmountService monthlyMealsTotalAmountService)
        {
            _monthlyMainDishReportService = monthlyMainDishReportService ?? new MonthlyMainDishReportService();
            _dailyMainDishReportService = dailyMainDishReportService ?? new DailyMainDishReportService();
            _commonService = commonService ?? new CommonService();
            _userControlService = userControlService ?? new UserControlService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
            _adminOrderHistoryService = adminOrderHistoryService ?? new AdminOrderHistoryService();
            _monthlyPerDayService = monthlyPerDayService ?? new MonthlyMainDishPerDayReportService();
            _sideDishReportService = sideDishReportService ?? new SideDishReportService();
            _mealForecastReportService = mealForecastReportService ?? new MealForecastReportService();
            _viewMenuReportService = viewMenuReportService ?? new ViewMenuReportService();
            _mealForecastDetailReportService = mealForecastDetailReportService ?? new MealForecastDetailReportService();
            _sideDishService = sideDishService ?? new SideDishService();
            _sideDishSummaryReportService = sideDishSummaryReportService ?? new SideDishSummaryReportService();
            _employeeService = employeeService ?? new EmployeeService();
            _monthlyMealsTotalAmountService = monthlyMealsTotalAmountService ?? new MonthlyMealTotalAmountService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MonthlyMainDishReportPerMeal, Policy = Policy.Read)]
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
                IsLocal = isLocal
            };
            MonthlyLocalReportModel fullReport = null;

            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId);
            }

            if (fullReport == null)
            {
                fullReport = _monthlyMainDishReportService.GetMonthlyMainDishReportLocal(criteria);
                SessionManager.Set<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId, fullReport);
            }

            MonthlyLocalReportModel pagingReport = _monthlyMainDishReportService.GetPaingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.DailyMainDishReport, Policy = Policy.Read)]
        public DailyMainDishReportModel GetDailyMainDishReport(
            DateTime fromDate,
            DateTime toDate,
            int orderCount,
            string departmentId = "",
            string employeeTypeId = "",
            string teamId = "",
            string mealId = "",
            int? userId = null,
            string query = "",
            bool isDebug = false,
            int? isLocal = null,
            int startIndex = 0,
            int itemsPerPage = 50,
            bool isPagingCallback = false)
        {
            var criteria = new GetDailyMainDishPricePerMealReportCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                OrderCount = orderCount,
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
                MealId = mealId,
                IsDebug = isDebug,
                IsLocal = isLocal,
                UserId = userId,
                Query = query
            };
            DailyMainDishReportModel fullReport = null;
            DailyMainDishReportModel pagingReport = null;

            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<DailyMainDishReportModel>(ReportConstant.DailyMainDishReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _dailyMainDishReportService.GetDailyMainDishReport(criteria);
                SessionManager.Set<DailyMainDishReportModel>(ReportConstant.DailyMainDishReportSessionKey + CurrentUser.UserId, fullReport);
            }

            pagingReport = _dailyMainDishReportService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishReport, Policy = Policy.Read)]
        public SideDishReportModel GetSideDishReport(
            DateTime fromDate,
            DateTime toDate,
            string departmentId = "",
            string teamIds = "",
            string employeeTypeIds = "",
            int? userId = null,
            string sideDishItemIds = "",
            int? pageIndex = null,
            int? itemsPerPage = null)
        {
            var criteria = new GetSideDishReportCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                DepartmentId = departmentId,
                TeamId = teamIds,
                EmployeeTypeId = employeeTypeIds,
                UserId = userId,
                SideDishItemId = sideDishItemIds,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            SideDishReportModel reportData = _sideDishReportService.GetSideDishReport(criteria);

            return reportData;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishSummaryReport, Policy = Policy.Read)]
        public SideDishSummaryReportModel GetSideDishSummaryReport(
            DateTime fromDate,
            DateTime toDate,
            string departmentId = "",
            string teamId = "",
            string employeeTypeId = "",
            int? userId = null,
            int? pageIndex = null,
            int? itemsPerPage = null)
        {
            var criteria = new GetSideDishSummaryReportCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
                UserId = userId,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            SideDishSummaryReportModel report = _sideDishSummaryReportService.GetSideDishSummaryReport(criteria);

            return report;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishSummaryReport, Policy = Policy.Read)]
        [HttpGet]
        public Employee GetEmployeeById(int userId)
        {
            var criteria = new GetEmployeeByIdCriteria()
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
        public IEnumerable<Department> GetDepartmentByUserRole()
        {
            IEnumerable<Department> departments = null;
            int financeRole = (int)RolesSystem.Finance;
            int employeeRole = (int)RolesSystem.Employee;
            int adminRole = (int)RolesSystem.Admin;
            int kitchenRole = (int)RolesSystem.Kitchen;
            int systemAdminRole = (int)RolesSystem.SA;

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
                    var criteria = new GetAllDepartmentCriteria();
                    departments = _commonService.GetAllDepartment(criteria);
                }
                else if (isContainFinanceRole)
                {
                    var criteria = new GetEmployeeRoleByIdCriteria
                    {
                        SystemId = CurrentUser.UserId,
                        RoleId = financeRole.ToString()
                    };
                    departments = _userControlService.GetDepartmentUserHasPermission(criteria);
                }
            }

            return departments;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.EmployeeOrderHistoricalReport, Policy = Policy.Read)]
        public EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(
            DateTime fromDate,
            DateTime toDate,
            int orderBy = 0,
            int employeeMenu = 0,
            int orderPlatform = 0,
            string mealId = "",
            string statusId = "",
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            var criteria = new GetEmployeeOrderHistoryCriteria
            {
                UserId = CurrentUser.UserId,
                FromDate = fromDate,
                ToDate = toDate,
                OrderBy = orderBy,
                MyMenu = employeeMenu,
                OrderPlatform = orderPlatform,
                MealId = mealId,
                StatusId = statusId
            };

            EmployeeOrderHistoryReport fullReport;
            GetOrderedByEnum();
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeOrderHistoryReport>(ReportConstant.EmployeeOrderHistoryReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                fullReport = _employeeOrderHistoryService.GetEmployeeOrderHistoryReport(criteria);
                SessionManager.Set<EmployeeOrderHistoryReport>(ReportConstant.EmployeeOrderHistoryReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeOrderHistoryReport pagingReport = _employeeOrderHistoryService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.AdminOrderHistoryReport, Policy = Policy.Read)]
        public AdminOrderHistoryReport GetAdminOrderHistoryReport(
            DateTime fromDate,
            DateTime toDate,
            int? userId = null,
            int employeeMenu = 0,
            int orderPlatform = 0,
            string mealId = "",
            string statusId = "",
            string companyId = "",
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            var criteria = new GetAdminOrderHistoryCriteria
            {
                UserId = userId,
                FromDate = fromDate,
                ToDate = toDate,
                OrderBy = 0,
                MyMenu = employeeMenu,
                OrderPlatform = orderPlatform,
                MealId = mealId,
                StatusId = statusId,
                CompanyId = companyId
            };

            AdminOrderHistoryReport fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId);
            }
            else
            {
                fullReport = _adminOrderHistoryService.GetAdminOrderHistoryReport(criteria);
                SessionManager.Set<AdminOrderHistoryReport>(ReportConstant.AdminOrderHistoryReportSessionKey + criteria.UserId, fullReport);
            }

            var pagingReport = _adminOrderHistoryService.GetPagingReport(fullReport, startIndex, itemsPerPage);

            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MonthlyMainDishReportPerDay, Policy = Policy.Read)]
        public MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(
            DateTime reportDate,
            string departmentId = "",
            string employeeTypeId = "",
            string teamId = "",
            int? workDay = null,
            int groupBy = 0,
            bool isDebug = false,
            int isLocal = 0,
            int startIndex = 0,
            int itemsPerPage = 50,
            bool isPagingCallback = false)
        {
            var criteria = new GetMonthlyMainDishPerDayReportCriteria
            {
                ReportDate = reportDate,
                DepartmentId = departmentId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
                WorkDay = workDay,
                GroupBy = groupBy,
                IsDebug = isDebug,
                IsLocal = isLocal
            };
            MonthlyMainDishPerDayModel fullReport;
            if (isPagingCallback)
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

            MonthlyMainDishPerDayModel pagingReport = _monthlyPerDayService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public MealForecastReportModel GetMealForecastReport(
            DateTime fromDate,
            DateTime toDate,
            string departmentId = "",
            string employeeTypeId = "",
            bool? isVegetarian = null,
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            MealForecastReportModel fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<MealForecastReportModel>(ReportConstant.MealForecastReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                var criteria = new GetMealForecastReportCriteria
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    DepartmentId = departmentId,
                    EmployeeTypeId = employeeTypeId,
                    IsVegetarian = isVegetarian
                };

                fullReport = _mealForecastReportService.GetMealForecastReport(criteria);
                SessionManager.Set<MealForecastReportModel>(ReportConstant.MealForecastReportSessionKey + CurrentUser.UserId, fullReport);
            }

            MealForecastReportModel pagingReport = _mealForecastReportService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public MealForecastDetailReport GetMealForecastDetailReport(
            DateTime date,
            string departmentId = "",
            string employeeTypeId = "",
            bool? isVegetarian = null,
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            MealForecastDetailReport fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<MealForecastDetailReport>(ReportConstant.MealForecastDetailReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                var criteria = new GetMealForecastDetailReportCriteria
                {
                    OnDate = date,
                    DepartmentId = departmentId,
                    EmployeeTypeId = employeeTypeId,
                    IsVegetarian = isVegetarian
                };

                fullReport = _mealForecastDetailReportService.GetMealForecastDetailReport(criteria);
                SessionManager.Set<MealForecastDetailReport>(ReportConstant.MealForecastDetailReportSessionKey + CurrentUser.UserId, fullReport);
            }

            MealForecastDetailReport pagingReport = _mealForecastDetailReportService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public EmployeeRegistrationReportModel GetEmployeeRegistration(
            DateTime date,
            string departmentId = "",
            string employeeTypeId = "",
            int? mealId = null,
            int? menuId = null,
            bool? isVegetarian = null,
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            EmployeeRegistrationReportModel fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeRegistrationReportModel>(ReportConstant.EmployeeRegistrationReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                var criteria = new GetEmployeeRegistrationCriteria
                {
                    WorkDate = date,
                    MealId = mealId,
                    MenuId = menuId,
                    DepartmentId = departmentId,
                    EmployeeTypeId = employeeTypeId,
                    IsVegetarian = isVegetarian
                };

                fullReport = _mealForecastDetailReportService.GetEmployeeRegistration(criteria);
                SessionManager.Set<EmployeeRegistrationReportModel>(ReportConstant.EmployeeRegistrationReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeRegistrationReportModel pagingReport = _mealForecastDetailReportService.GetPagingEmployeeRegistration(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.MealForecastReport, Policy = Policy.Read)]
        public EmployeeRegistrationReportModel GetEmployeeNonRegistration(
            DateTime date,
            string departmentId = "",
            string employeeTypeId = "",
            int? mealId = null,
            int? menuId = null,
            bool? isVegetarian = null,
            bool isPagingCallback = false,
            int startIndex = 0,
            int itemsPerPage = 50)
        {
            EmployeeRegistrationReportModel fullReport;
            if (isPagingCallback)
            {
                fullReport = SessionManager.Get<EmployeeRegistrationReportModel>(ReportConstant.EmployeeNonRegistrationReportSessionKey + CurrentUser.UserId);
            }
            else
            {
                var criteria = new GetEmployeeNonRegistrationCriteria
                {
                    WorkDate = date,
                    MealId = mealId,
                    MenuId = menuId,
                    DepartmentId = departmentId,
                    EmployeeTypeId = employeeTypeId,
                    IsVegetarian = isVegetarian
                };

                fullReport = _mealForecastDetailReportService.GetEmployeeNonRegistration(criteria);
                SessionManager.Set<EmployeeRegistrationReportModel>(ReportConstant.EmployeeNonRegistrationReportSessionKey + CurrentUser.UserId, fullReport);
            }

            EmployeeRegistrationReportModel pagingReport = _mealForecastDetailReportService.GetPagingEmployeeNonRegistration(fullReport, startIndex, itemsPerPage);
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

            foreach (DepartmentIdentity departmentEnum in (DepartmentIdentity[])Enum.GetValues(typeof(DepartmentIdentity)))
            {
                if (departmentEnum != DepartmentIdentity.None)
                {
                    var department = new
                    {
                        Value = (int)departmentEnum,
                        Name = Enumerations.GetDescription(departmentEnum)
                    };

                    companies.Add(department);
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

        public LunchAndDinnerTotalAmountReportModel GetLunchAndDinnerReport(string departmentId, DateTime reportDate)
        {
            return _monthlyMealsTotalAmountService.GetMonthlyLunchAndDinnerReport(reportDate, departmentId);
        }

        public BreakfastAndSupperTotalAmountReportModel GetBreakfastAndSupperReport(string departmentId, DateTime reportDate)
        {
            return _monthlyMealsTotalAmountService.GetMonthlyBreakfastAndSupperReport(reportDate, departmentId);
        }
    }
}