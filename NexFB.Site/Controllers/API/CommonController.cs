// ----------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/29/2014 11:19:47 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Collections.Generic;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// Common controller.
    /// </summary>
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonService;

        public CommonController()
            : this(null)
        {
        }

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService ?? new CommonService();
        }

        public IEnumerable<Department> GetAllDepartment()
        {
            var criteria = new GetAllDepartmentCriteria();
            return _commonService.GetAllDepartment(criteria);
        }

        public IEnumerable<Team> GetAllTeamByCompany(int departmentId)
        {
            var criteria = new GetAllTeamByCompanyCriteria
            {
                DepartmentId = departmentId
            };

            return _commonService.GetAllTeamByCompany(criteria);
        }

        public IEnumerable<Location> GetAllLocation()
        {
            var criteria = new GetAllLocationCriteria();
            return _commonService.GetAllLocation(criteria);
        }

        public IEnumerable<MealTimesheet> GetAllMealTimesheet()
        {
            var criteria = new GetMealTimesheetCriteria();
            return _commonService.GetAllMealTimesheet(criteria);
        }

        public IEnumerable<EmployeeTypeModel> GetAllEmployeeType(string departmentId = "")
        {
            var criteria = new GetAllEmployeeTypeCriteria
            {
                DepartmentId = departmentId
            };

            return _commonService.GetAllEmployeeType(criteria);
        }
    }
}