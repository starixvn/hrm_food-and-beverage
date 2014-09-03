// ----------------------------------------------------------------------
// <copyright file="GetAllEmployeeFunctionCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/3/2014 1:56:09 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee function criteria.
    /// </summary>
    public class GetAllEmployeeFunctionCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetAllFunctionBySysID";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
