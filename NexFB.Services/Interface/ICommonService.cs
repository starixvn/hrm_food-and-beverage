// ----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/27/2014 10:13:08 AM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Enums;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Team;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.Entities;
    using NexFB.Models.UserControl;

    /// <summary>
    /// The common service.
    /// </summary>
    public interface ICommonService
    {
        IEnumerable<RoleSystemModel> GetAllSystemRole(GetAllSystemRoleCriteria criteria);

        int GetPermission(string functionKey);

        bool HasPermissionOnFunction(string functionKey, Policy policy);

        IEnumerable<Department> GetAllDepartment(GetAllDepartmentCriteria criteria);

        IEnumerable<Team> GetAllTeamByCompany(GetAllTeamByCompanyCriteria criteria);

        IEnumerable<Location> GetAllLocation(GetAllLocationCriteria criteria);
    }
}
