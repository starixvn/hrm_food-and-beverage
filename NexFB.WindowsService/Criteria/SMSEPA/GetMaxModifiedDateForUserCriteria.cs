// --------------------------------------------------------------------------
// <copyright file="GetMaxModifiedDateForUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetMaxModifiedDateForUserCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Sync_Employee_GetMaxModifiedDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
