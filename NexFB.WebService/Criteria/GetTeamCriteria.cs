// --------------------------------------------------------------------------
// <copyright file="GetTeamCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.4">Created</change>
// </history>

namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetTeam Criteria.
    /// </summary>
    public class GetTeamCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey()
        {
            return "EmployeeGroup_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
