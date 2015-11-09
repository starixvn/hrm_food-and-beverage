// --------------------------------------------------------------------------
// <copyright file="GetMaxModifiedDateForGroupSupervisorCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.21">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetMaxModifiedDateForGroupSupervisorCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Sync_GroupSupervisor_GetMaxModifiedDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
