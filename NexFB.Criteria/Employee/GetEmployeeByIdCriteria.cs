// --------------------------------------------------------------------------
// <copyright file="GetEmployeeByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.12">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetEmployeeById Criteria.
    /// </summary>
    public class GetEmployeeByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeBySysID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
