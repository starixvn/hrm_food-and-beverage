// --------------------------------------------------------------------------
// <copyright file="EmployeeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Core.Security;
    using NexFB.Core.Services;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Services.Interface;

    /// <summary>
    /// The implement of Employee service.
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        /// <summary>
        /// Function to get user by id.
        /// </summary>
        /// <param name="criteria">GGetEmployeeById Criteria.</param>
        /// <returns>User Session.</returns>
        public UserSession GetUserById(GetEmployeeByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            UserSession user = null;

            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        /// <summary>
        /// Function to get user by domain.
        /// </summary>
        /// <param name="criteria">GetEmployeeByDomain Criteria.</param>
        /// <returns>User Session.</returns>
        public UserSession GetUserByDomain(GetEmployeeByDomainCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            UserSession user = null;

            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        /// <summary>
        /// Function to get user by code.
        /// </summary>
        /// <param name="criteria">GetEmployeeByCode Criteria.</param>
        /// <returns>User Session.</returns>
        public UserSession GetUserByCode(GetEmployeeByCodeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            UserSession user = null;

            if (criteria.IsValid())
            {
                user = GetDataEntity<UserSession>(criteria);
            }

            return user;
        }

        /// <summary>
        /// Function to validate user.
        /// </summary>
        /// <param name="user">Employee need validate.</param>
        /// <param name="password">Password of employee.</param>
        /// <returns>Result of validate.</returns>
        public bool IsValidateUser(UserSession user, string password)
        {
            bool isValid = user != null
                && !string.IsNullOrWhiteSpace(user.Password)
                && Encryptor.CompareStringToHash(HashProvider.Sha1, password, user.Password);

            return isValid;
        }

        /// <summary>
        /// Update password of user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public CommandResult UpdatePassword(ChangePasswordCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();

            if (criteria.IsValid())
            {
                string hashPassword = Encryptor.ComputeHash(HashProvider.Sha1, criteria.Password);
                criteria.Password = hashPassword;
                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Get employee menu by Id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list group menu.</returns>
        public IList<MenuGroup> GetEmployeeMenuById(GetEmployeeMenuBySystemIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<MenuGroup> menuGroups = null;

            if (criteria.IsValid())
            {
                IEnumerable<MenuItem> menuItems = GetData<MenuItem>(criteria);
                menuGroups = GroupMenuItem(menuItems);
            }

            return menuGroups;
        }

        /// <summary>
        /// Get all function of employee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list Function model.</returns>
        public IList<UserFunction> GetAllEmployeeFunction(GetAllEmployeeFunctionCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<UserFunction> functions = null;

            if (criteria.IsValid())
            {
                functions = GetData<UserFunction>(criteria).ToList();
            }

            return functions;
        }

        /// <summary>
        /// Function to get employees by query.
        /// </summary>
        /// <param name="criteria">GetEmployee Criteria.</param>
        /// <returns>The List of employees.</returns>
        public IList<Employee> GetEmployeeByQuery(GetEmployeeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<Employee> employees = null;

            if (criteria.IsValid())
            {
                employees = GetData<Employee>(criteria).ToList();
            }

            return employees;
        }

        /// <summary>
        /// Function to insert a special employee.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult InsertSpecialUser(InsertSpecialUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Insert(criteria, hasGetOutputParams: true);
            }

            return result;
        }

        /// <summary>
        /// Function to update a special employee.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateSpecialUser(UpdateSpecialUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to update status a special employee.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult UpdateStatusSpecialUser(ActivateSpecialUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Update(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to delete a special employee.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <returns>Command Result.</returns>
        public CommandResult DeleteSpecialUser(DeleteSpecialUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = new CommandResult();
            if (criteria.IsValid())
            {
                result = Repository.Delete(criteria);
            }

            return result;
        }

        /// <summary>
        /// Function to get special employees.
        /// </summary>
        /// <param name="criteria">GetSpecialEmployee Criteria.</param>
        /// <returns>The List of employees.</returns>
        public IList<SpecialUser> GetSpecialUsers(GetSpecialUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IList<SpecialUser> employees = null;

            if (criteria.IsValid())
            {
                employees = GetData<SpecialUser>(criteria).ToList();
            }

            return employees;
        }

        /// <summary>
        /// Function to get special employee by id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Employee.</returns>
        public SpecialUser GetSpecialUserById(GetSpecialUserByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            SpecialUser employee = null;

            if (criteria.IsValid())
            {
                employee = GetDataEntity<SpecialUser>(criteria);
            }

            return employee;
        }

        /// <summary>
        /// Function to get special employee by id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Employee.</returns>
        public Employee GetEmployeeById(GetEmployeeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            Employee employee = null;

            if (criteria.IsValid())
            {
                employee = GetDataEntity<Employee>(criteria);
            }

            return employee;
        }

        /// <summary>
        /// Group menu item.
        /// </summary>
        /// <param name="menuItems">The enumerator menu item.</param>
        /// <returns>The list group menu.</returns>
        private IList<MenuGroup> GroupMenuItem(IEnumerable<MenuItem> menuItems)
        {
            var menuGroups = new List<MenuGroup>();
            IEnumerable<MenuItem> parentMenu = menuItems.Where(item => item.ParentId == null).ToList();

            foreach (MenuItem parent in parentMenu)
            {
                var menuGroupItem = new MenuGroup
                {
                    MenuId = parent.MenuId,
                    Name = parent.Name,
                    MenuLink = parent.MenuLink
                };

                menuGroupItem.MenuItems = menuItems.Where(menuItem => menuItem.ParentId == menuGroupItem.MenuId);
                menuGroups.Add(menuGroupItem);
            }

            return menuGroups;
        }
    }
}
