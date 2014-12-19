// ----------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/5/2014 9:03:28 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Report;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Services.Implement;
    using NexFB.Services.Implement.Report;
    using NexFB.Services.Interface;
    using NexFB.Services.Interface.Report;
    using NexFB.Web;
    using NexFB.Web.Attributes;

    /// <summary>
    /// The report controller.
    /// </summary>
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly ICommonService _commonService;
        private readonly IUserControlService _userControlService;
        private readonly IEmployeeOrderHistoryService _employeeOrderHistoryService;
        private readonly IMonthlyMainDishPerDayReportService _monthlyPerDayService;

        public ReportController()
            : this(null, null, null, null, null)
        {
        }

        public ReportController(
            IReportService reportService,
            ICommonService commonService,
            IUserControlService userControlService,
            IEmployeeOrderHistoryService employeeOrderHistoryService,
            IMonthlyMainDishPerDayReportService monthlyPerDayService)
        {
            _reportService = reportService ?? new ReportService();
            _commonService = commonService ?? new CommonService();
            _userControlService = userControlService ?? new UserControlService();
            _employeeOrderHistoryService = employeeOrderHistoryService ?? new EmployeeOrderHistoryService();
            _monthlyPerDayService = monthlyPerDayService ?? new MonthlyMainDishPerDayReportService();
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
                IsLocal = isLocal,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
                IsPagingCallback = isPagingCallback
            };
            MonthlyLocalReportModel fullReport = null;

            if (criteria.IsPagingCallback)
            {
                 fullReport = SessionManager.Get<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId);
            }

            if (fullReport == null)
            {
                fullReport = _reportService.GetMonthlyMainDishReportLocal(criteria);
                SessionManager.Set<MonthlyLocalReportModel>(ReportConstant.MonthlyMainDishReportLocalSessionKey + CurrentUser.UserId, fullReport);
            }

            MonthlyLocalReportModel pagingReport = _reportService.GetPaingReport(fullReport, criteria.StartIndex, criteria.ItemsPerPage);
            return pagingReport;
        }

        /// <summary>
        /// Get daily main dish report local.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="orderCount">The order count.</param>
        /// <param name="departmentId">The department Id.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="userId">The user Id.</param>
        /// <param name="query">The query.</param>
        /// <param name="isDebug">Is debug or not.</param>
        /// <param name="isLocal">Is local or not.</param>
        /// <returns>The report.</returns>
        [HttpGet]
        public DailyMainDishReportModel GetDailyMainDishReport(
            DateTime fromDate,
            DateTime toDate,
            int orderCount,
            string departmentId = "",
            string teamId = "",
            string mealId = "",
            int? userId = null,
            string query = "",
            bool isDebug = false,
            int? isLocal = null)
        {
            var criteria = new GetDailyMainDishPricePerMealReportCriteria
            {
                FromDate = fromDate,
                ToDate = toDate,
                OrderCount = orderCount,
                DepartmentId = departmentId,
                TeamId = teamId,
                MealId = mealId,
                IsDebug = isDebug,
                IsLocal = isLocal,
                UserId = userId,
                Query = query
            };
            DailyMainDishReportModel fullReport = _reportService.GetDailyMainDishReport(criteria);
            DailyMainDishReportModel pagingReport = null;
            SessionManager.Set<DailyMainDishReportModel>(ReportConstant.DailyMainDishReportSessionKey, fullReport);

            pagingReport = _dailyMainDishReportService.GetPagingReport(fullReport, startIndex, itemsPerPage);
            return pagingReport;
        }

        /// <summary>
        /// Get department by user role.
        /// </summary>
        /// <returns>The department Id.</returns>
        [HttpGet]
        public IEnumerable<Department> GetDepartmentByUserRole()
        {
            IEnumerable<Department> departments = null;
            int financeRole = (int)RolesSystem.Finance;
            int employeeRole = (int)RolesSystem.Employee;
            int adminRole = (int)RolesSystem.Admin;
            int systemAdminRole = (int)RolesSystem.SA;
            int kitchenRole = (int)RolesSystem.Kitchen;

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

        /// <summary>
        /// Get employee order history report.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="employeeMenu">The menu of employee.</param>
        /// <param name="orderPlatform">The order platform.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <param name="statusId">The status Id.</param>
        /// <param name="isPagingCallback">The is paging call back.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The report.</returns>
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
            EmployeeOrderHistoryReport fullReport = null;
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

         /// <summary>
        /// Monthly main dish per day report.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <param name="departmentId">The department Id.</param>
        /// <param name="employeeTypeId">The employee Type Id.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="workDay">The work day.</param>
        /// <param name="groupBy">The is group by : employee or team.</param>
        /// <param name="isDebug">The is debug.</param>
        /// <param name="isLocal">The is local.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="isPagingCallback">The is paging callback.</param>
        /// <returns>The report.</returns>
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
            MonthlyMainDishPerDayModel fullReport = null;

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

        /// <summary>
        /// Get Day Type list.
        /// </summary>
        /// <returns>The list object Day Type.</returns>
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
            
        /// <summary>
        /// Get Ordered By ENUM.
        /// </summary>
        /// <returns>The list object Ordered By.</returns>
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
        
        /// <summary>
        /// Get Order Platform ENUM.
        /// </summary>
        /// <returns>The list object Order Platform.</returns>
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

        /// <summary>
        /// Get Order Status ENUM.
        /// </summary>
        /// <returns>The list object Order Status.</returns>
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

        /// <summary>
        /// Get Menu Type ENUM.
        /// </summary>
        /// <returns>The list object Menu Type.</returns>
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