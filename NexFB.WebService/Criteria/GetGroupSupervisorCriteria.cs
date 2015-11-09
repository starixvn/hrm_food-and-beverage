// --------------------------------------------------------------------------
// <copyright file="GetGroupSupervisorCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.21">Created</change>
// </history>

namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetGroupSupervisorCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey()
        {
            return "GroupSupervisor_GetForSync";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
