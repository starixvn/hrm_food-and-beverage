// ----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/27/2014 10:13:58 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;
    using NexFB.Services.Interface;

    /// <summary>
    /// The common service.
    /// </summary>
    public class CommonService : BaseService, ICommonService
    {
        /// <summary>
        /// Get all role system.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Role System Model.</returns>
        public IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<RoleSystemModel> roles = null;

            if (criteria.IsValid())
            {
                roles = GetData<RoleSystemModel>(criteria);
            }

            return roles;
        }

        /// <summary>
        /// Get permission on function.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <returns>The permission.</returns>
        public int GetPermission(string functionKey)
        {
            int policy = (int)Policy.None;

            if (HasPermissionOnFunction(functionKey, Policy.Host))
            {
                policy = (int)Policy.Host;
            }
            else if (HasPermissionOnFunction(functionKey, Policy.Read))
            {
                policy = (int)Policy.Read;
            }

            return policy;
        }

        /// <summary>
        /// Check current user has permission on current function or not.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">THe policy.</param>
        /// <returns>True if has permission, false: otherwise.</returns>
        public bool HasPermissionOnFunction(string functionKey, Policy policy)
        {
            FunctionModel currentFunction = new FunctionModel();

            if (currentFunction != null)
            {
                switch (policy)
                {
                    case Policy.None:
                        return true;

                    case Policy.Read:
                        return currentFunction.IsRead || currentFunction.IsHost;

                    case Policy.Host:
                        return currentFunction.IsHost;
                }
            }

            return false;
        }

        /// <summary>
        /// Get all department.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Department.</returns>
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

        /// <summary>
        /// Get all team by company.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Team model.</returns>
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
    }
}
