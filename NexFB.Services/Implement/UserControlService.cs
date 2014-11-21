// ----------------------------------------------------------------------
// <copyright file="UserControlService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 1:27:20 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Employee;
    using NexFB.Models.UserControl;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// Authority Service.
    /// </summary>
    public class UserControlService : BaseService, IUserControlService
    {
        private readonly IUserControlRepository _userControlRepository;

        public UserControlService()
            : this(null)
        {
        }

        public UserControlService(IUserControlRepository userControlRepository)
        {
            _userControlRepository = userControlRepository ?? new UserControlRepository();
        }

        /// <summary>
        /// Get all function by role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Function Model.</returns>
        public IEnumerable<FunctionModel> GetFunctionByRole(GetFunctionByRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<FunctionModel> functionGroup = null;

            if (criteria.IsValid())
            {
                IEnumerable<FunctionModel> rawFunctions = GetData<FunctionModel>(criteria);
                functionGroup = rawFunctions.Where<FunctionModel>(func => func.ParentFunctionId == null);

                foreach (FunctionModel rawParentFunctionItem in functionGroup)
                {
                    rawParentFunctionItem.ChildFunctions = GetFunctionByRoleRecursion(rawFunctions, rawParentFunctionItem);
                }
            }

            return functionGroup;
        }

        /// <summary>
        /// Assign permission to role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        public CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Get all employee role.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Employee Model.</returns>
        public IEnumerable<EmployeeModel> GetAllEmployeeRole(GetAllEmployeeRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<EmployeeModel> employees = null;

            if (criteria.IsValid())
            {
                employees = _userControlRepository.GetAllEmployeeRole(criteria);
            }

            return employees;
        }

        /// <summary>
        /// Get employee role by ID.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The employee model.</returns>
        public EmployeeModel GetEmployeeRoleById(GetEmployeeRoleByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            EmployeeModel employee = null;

            if (criteria.IsValid())
            {
                employee = _userControlRepository.GetEmployeeRoleById(criteria);
            }

            return employee;
        }

        /// <summary>
        /// Assign role to user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult AssignRoleToUser(AssignRoleToUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            CommandResult commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }

        public IList<RoleFunctionModel> GetAllRoleFunction(GetFunctionByRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<RoleFunctionModel> roleFunctions = null;
            if (criteria.IsValid())
            {
                roleFunctions = this.GetData<RoleFunctionModel>(criteria).ToList();
            }

            return roleFunctions;
        }

        /// <summary>
        /// Get function by role recursion.
        /// </summary>
        /// <param name="rawFunctions">The enumerator contain all functions.</param>
        /// <param name="rawParentFunctionItem">The current parent function.</param>
        /// <returns>The list Function Model.</returns>
        private List<FunctionModel> GetFunctionByRoleRecursion(
            IEnumerable<FunctionModel> rawFunctions,
            FunctionModel rawParentFunctionItem)
        {
            List<FunctionModel> rawChildFunctions = rawFunctions.Where(func => func.ParentFunctionId == rawParentFunctionItem.FunctionId).ToList();

            if (rawChildFunctions != null && rawChildFunctions.Count() > 0)
            {
                foreach (var rawChildFunctionItem in rawChildFunctions)
                {
                    rawChildFunctionItem.ChildFunctions = GetFunctionByRoleRecursion(rawFunctions, rawChildFunctionItem);
                }
            }

            return rawChildFunctions;
        }
    }
}
