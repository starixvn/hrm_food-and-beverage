// ----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/27/2014 10:13:58 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Services;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Services.Interface;

    /// <summary>
    /// The common service.
    /// </summary>
    public class CommonService : BaseService, ICommonService
    {
        /// <summary>
        /// Get all role system.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="exceptSystemAdmin">Except system admin role.</param>
        /// <returns>The enumerator Role System Model.</returns>
        public IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria, bool exceptSystemAdmin = false)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            List<RoleSystemModel> roles = null;

            if (criteria.IsValid())
            {
                roles = GetData<RoleSystemModel>(criteria).ToList();

                if (exceptSystemAdmin)
                {
                    RoleSystemModel systemAdminRole = roles.Where(x => x.RoleId == (int)RolesSystem.SA).First();
                    roles.Remove(systemAdminRole);
                }
            }

            return roles;
        }

        /// <summary>
        /// Get all department.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Department.</returns>
        public IEnumerable<Department> GetAllDepartment(GetAllDepartmentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Department> departments = null;

            if (criteria.IsValid())
            {
                departments = GetData<Department>(criteria);
            }

            return departments;
        }

        /// <summary>
        /// Get all team by company.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Team model.</returns>
        public IEnumerable<Team> GetAllTeamByCompany(GetAllTeamByCompanyCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Team> teams = null;

            if (criteria.IsValid())
            {
                teams = GetData<Team>(criteria);
            }

            return teams;
        }

        /// <summary>
        /// Get all location.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator location model.</returns>
        public IEnumerable<Location> GetAllLocation(GetAllLocationCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<Location> locations = null;

            if (criteria.IsValid())
            {
                locations = GetData<Location>(criteria);
            }

            return locations;
        }

        /// <summary>
        /// Get all meal timesheet.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator meal timesheet.</returns>
        public IEnumerable<MealTimesheet> GetAllMealTimesheet(GetMealTimesheetCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<MealTimesheet> mealTimesheet = null;

            if (criteria.IsValid())
            {
                mealTimesheet = GetData<MealTimesheet>(criteria);
            }

            return mealTimesheet;
        }

        /// <summary>
        /// Get all employee type.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Employee Type model.</returns>
        public IEnumerable<EmployeeTypeModel> GetAllEmployeeType(GetAllEmployeeTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<EmployeeTypeModel> employeeTypes = null;

            if (criteria.IsValid())
            {
                employeeTypes = GetData<EmployeeTypeModel>(criteria);
            }

            return employeeTypes;
        }
    }
}
