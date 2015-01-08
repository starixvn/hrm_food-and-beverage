// ----------------------------------------------------------------------
// <copyright file="GetAllTeamByCompanyCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/29/2014 12:45:51 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Team
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all team criteria.
    /// </summary>
    public class GetAllTeamByCompanyCriteria : CriteriaBase
    {
        public int? DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "EmpTeam_GetByDepartmentID";
        }

        public override bool IsValid()
        {
            return (DepartmentId.HasValue && DepartmentId.Value > 0) || !DepartmentId.HasValue;
        }
    }
}
