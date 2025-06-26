namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.UserControl;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;

    /// <summary>
    /// The interface of authority service.
    /// </summary>
    public interface IUserControlService
    {
        IEnumerable<FunctionModel> GetFunctionByRole(GetFunctionByRoleCriteria criteria);

        CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria);

        IEnumerable<EmployeeModel> GetAllEmployeeRole(GetAllEmployeeRoleCriteria criteria);

        EmployeeModel GetEmployeeRoleById(GetEmployeeRoleByIdCriteria criteria);

        CommandResult AssignRoleToUser(AssignRoleToUserCriteria criteria);
        CommandResult AssignFreeOrderToUser(AssignFreeOrderToUserCriteria criteria);

        IEnumerable<Company> GetCompanyUserHasPermission(GetEmployeeRoleByIdCriteria criteria);
    }
}
