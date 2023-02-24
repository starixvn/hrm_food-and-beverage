namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Core.Services;
    using NexFB.Criteria.Company;
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

        public IEnumerable<Team> GetAllTeamByCompany(GetAllTeamByCompanyCriteria criteria)
        {
            IEnumerable<Team> teams = null;
            if (criteria.IsValid())
            {
                teams = GetData<Team>(criteria);
            }

            return teams;
        }

        public IEnumerable<MealTimesheet> GetAllMealTimesheet(GetMealTimesheetCriteria criteria)
        {
            IEnumerable<MealTimesheet> mealTimesheet = null;
            if (criteria.IsValid())
            {
                mealTimesheet = GetData<MealTimesheet>(criteria);
            }

            return mealTimesheet;
        }

        public IEnumerable<EmployeeTypeModel> GetAllEmployeeType(GetAllEmployeeTypeCriteria criteria)
        {
            IEnumerable<EmployeeTypeModel> employeeTypes = null;
            if (criteria.IsValid())
            {
                employeeTypes = GetData<EmployeeTypeModel>(criteria);
            }

            return employeeTypes;
        }
    }
}