// --------------------------------------------------------------------------
// <copyright file="GetFromDateForAttendanceViolationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.11.9">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetFromDateForAttendanceViolationCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Sync_EmployeeAttendanceViolation_GetFromDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
