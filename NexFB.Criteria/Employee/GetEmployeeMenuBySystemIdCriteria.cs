// ----------------------------------------------------------------------
// <copyright file="GetEmployeeMenuBySystemIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/1/2014 3:15:10 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get employee menu by system Id criteria.
    /// </summary>
    public class GetEmployeeMenuBySystemIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "UserCtrl_GetMenuFunctionBySysID";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
