// ----------------------------------------------------------------------
// <copyright file="GetAllEmployeeRoleCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 2:24:11 PM">Created.</change>
// </history>

namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee role criteria.
    /// </summary>
    public class GetAllEmployeeRoleCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "query")]
        public string Keyword { get; set; }

        public string RoleId { get; set; }

        public override string GetSettingKey()
        {
            return "UserCtrl_GetEmployees";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
