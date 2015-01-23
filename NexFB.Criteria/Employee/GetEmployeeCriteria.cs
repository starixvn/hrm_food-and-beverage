// --------------------------------------------------------------------------
// <copyright file="GetEmployeeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.11">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetEmployee Criteria.
    /// </summary>
    public class GetEmployeeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int? UserId { get; set; }

        public string DepartmentId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "query")]
        public string Keyword { get; set; }

        public string RoleId { get; set; }

        public bool IsShowDisable { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployees";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
