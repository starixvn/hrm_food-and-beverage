// --------------------------------------------------------------------------
// <copyright file="GetLastEmployeeCalendarModifiedDateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetLastEmployeeCalendarModifiedDate Criteria.
    /// </summary>
    public class GetLastEmployeeCalendarModifiedDateCriteria : CriteriaBase
    {
        public int DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "EmpCalendar_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0;
        }
    }
}
