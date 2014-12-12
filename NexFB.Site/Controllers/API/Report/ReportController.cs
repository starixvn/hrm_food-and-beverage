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
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Report;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
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

        public ReportController()
            : this(null, null, null)
        {
        }

        public ReportController(
            IReportService reportService,
            ICommonService commonService,
            IUserControlService userControlService)
        {
            _reportService = reportService ?? new ReportService();
            _commonService = commonService ?? new CommonService();
            _userControlService = userControlService ?? new UserControlService();
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.MonthlyMainDishReportLocal, Policy = Policy.Read)]
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

        ////public DailyMainDishReportModel GetMonthlyMainDishReportLocal(
        ////    DateTime fromDate,
        ////    DateTime toDate,
        ////    int orderCount,
        ////    string departmentId = "",
        ////    string teamId = "",
        ////    string mealId = "",
        ////    bool isDebug = false,
        ////    int? isLocal = null,
        ////    int? userId = null,
        ////    string query = ""
        ////    )
        ////{
        ////    var criteria = new GetDailyMainDishPricePerMealReportCriteria
        ////    {
        ////    };
        ////}

        /// <summary>
        /// Get department by user role.
        /// </summary>
        /// <returns>The department Id.</returns>
        [HttpGet]
        public IEnumerable<Department> GetDepartmentByUserRole()
        {
            IEnumerable<Department> departments = null;
            int systemAdminRole = (int)RolesSystem.SA;
            int adminRole = (int)RolesSystem.Admin;
            string roles = CurrentUser.Roles;

            if (!string.IsNullOrEmpty(roles))
            {
                if (roles.Contains(systemAdminRole.ToString()))
                {
                    var criteria = new GetAllDepartmentCriteria();
                    departments = _commonService.GetAllDepartment(criteria);
                }
                else if (roles.Contains(adminRole.ToString()))
                {
                    var criteria = new GetEmployeeRoleByIdCriteria
                    {
                        SystemId = CurrentUser.UserId,
                        RoleId = adminRole.ToString()
                    };
                    departments = _userControlService.GetDepartmentUserHasPermission(criteria);
                }
            }

            return departments;
        }
    }
}