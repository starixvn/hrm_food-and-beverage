// --------------------------------------------------------------------------
// <copyright file="GetLastTeamModifiedDateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLastTeamModifiedDate Criteria.
    /// </summary>
    public class GetLastTeamModifiedDateCriteria : CriteriaBase
    {
        public int DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "EmpTeam_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0;
        }
    }
}
