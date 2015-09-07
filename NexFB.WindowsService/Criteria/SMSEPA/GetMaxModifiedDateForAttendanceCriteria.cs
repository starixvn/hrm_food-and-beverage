// --------------------------------------------------------------------------
// <copyright file="GetMaxModifiedDateForAttendanceCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetMaxModifiedDateForAttendanceCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Sync_EmployeeAttendance_GetMaxModifiedDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
