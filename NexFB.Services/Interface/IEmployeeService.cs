namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;

    public interface IEmployeeService
    {
        UserSession GetUserById(GetEmployeeByIdCriteria criteria);

        UserSession GetUserByDomain(GetEmployeeByDomainCriteria criteria);

        UserSession GetUserByCode(GetEmployeeByCodeCriteria criteria);

        bool IsValidateUser(UserSession user, string password);

        CommandResult UpdatePassword(ChangePasswordCriteria criteria);

        IList<MenuGroup> GetEmployeeMenuById(GetEmployeeMenuBySystemIdCriteria criteria);

        IList<UserFunction> GetAllEmployeeFunction(GetAllEmployeeFunctionCriteria criteria);

        IList<Employee> GetEmployeeByQuery(GetEmployeeCriteria criteria);

        IList<EmployeeShift> GetEmployeeShiftInMonth(GetEmployeeShiftInMonthCriteria criteria);

        Employee GetEmployeeById(GetEmployeeByIdCriteria criteria);

        SpecialUser GetSpecialUserById(GetSpecialUserByIdCriteria criteria);

        IList<SpecialUser> GetSpecialUsers(GetSpecialUserCriteria criteria);

        CommandResult InsertSpecialUser(InsertSpecialUserCriteria criteria);

        CommandResult UpdateSpecialUser(UpdateSpecialUserCriteria criteria);

        CommandResult UpdateStatusSpecialUser(ActivateSpecialUserCriteria criteria);

        CommandResult DeleteSpecialUser(DeleteSpecialUserCriteria criteria);
    }
}