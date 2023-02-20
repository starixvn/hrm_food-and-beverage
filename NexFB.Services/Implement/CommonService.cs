namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Services;
    using NexFB.Criteria.Company;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Services.Interface;

    public class CommonService : BaseService, ICommonService
    {
        public IList<Company> GetAllCompany()
        {
            var criteria = new GetAllCompanyCriteria();
            return GetData<Company>(criteria).ToList();
        }

        public IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria, bool exceptSystemAdmin = false)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            List<RoleSystemModel> roles = null;

            if (criteria.IsValid())
            {
                roles = GetData<RoleSystemModel>(criteria).ToList();

                if (exceptSystemAdmin)
                {
                    RoleSystemModel systemAdminRole = roles.First(x => x.RoleId == (int)RolesSystem.SA);
                    roles.Remove(systemAdminRole);
                }
            }

            return roles;
        }

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