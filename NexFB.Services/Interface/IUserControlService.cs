// ----------------------------------------------------------------------
// <copyright file="IUserControlService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 4:49:30 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.UserControl;
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

        IEnumerable<Department> GetDepartmentUserHasPermission(GetEmployeeRoleByIdCriteria criteria);
    }
}
