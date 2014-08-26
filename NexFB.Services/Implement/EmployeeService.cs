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
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Core.Security;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// The implement of Employee service.
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository = null)
        {
            _employeeRepository = employeeRepository ?? new EmployeeRepository();
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
                //// user = _employeeRepository.GetUserByDomain(criteria);
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
                //// user = _employeeRepository.GetUserByCode(criteria);
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
        public int ValidateUser(UserSession user, string password)
        {
            ////  1: wrong user or password
            ////  0: success
            int retCode = 1;
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    retCode = 1;
                }
                else
                {
                    if (Encryptor.CompareStringToHash(HashProvider.Sha1, password, user.Password))
                    {
                        retCode = 0;
                    }
                    else
                    {
                        retCode = 1;
                    }
                }
            }

            return retCode;
        }
    }
}
