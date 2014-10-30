// --------------------------------------------------------------------------
// <copyright file="GetLastUserModifiedDateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// GetLastUserModifiedDate Criteria.
    /// </summary>
    public class GetLastUserModifiedDateCriteria : CriteriaBase
    {
        public int DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0;
        }
    }
}
