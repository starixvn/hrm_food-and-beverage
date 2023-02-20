namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;

    public interface ICommonService
    {
        IList<Company> GetAllCompany();

        IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria, bool exceptSystemAdmin = false);

        IEnumerable<Department> GetAllDepartment(GetAllDepartmentCriteria criteria);

        IEnumerable<Team> GetAllTeamByCompany(GetAllTeamByCompanyCriteria criteria);

        IEnumerable<MealTimesheet> GetAllMealTimesheet(GetMealTimesheetCriteria criteria);

        IEnumerable<EmployeeTypeModel> GetAllEmployeeType(GetAllEmployeeTypeCriteria criteria);
    }
}
