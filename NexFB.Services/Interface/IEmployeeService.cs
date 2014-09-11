// --------------------------------------------------------------------------
// <copyright file="IEmployeeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;

    public interface IEmployeeService
    {
        UserSession GetUserByDomain(GetEmployeeByDomainCriteria criteria);

        UserSession GetUserByCode(GetEmployeeByCodeCriteria criteria);

        int ValidateUser(UserSession user, string password);

        IList<MenuGroup> GetEmployeeMenuById(GetEmployeeMenuBySystemIdCriteria criteria);

        IList<FunctionModel> GetAllEmployeeFunction(GetAllEmployeeFunctionCriteria criteria);

        IList<Employee> GetEmployeeByQuery(GetEmployeeCriteria criteria);
    }
}
