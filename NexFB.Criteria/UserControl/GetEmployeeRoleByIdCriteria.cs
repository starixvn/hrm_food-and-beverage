// ----------------------------------------------------------------------
// <copyright file="GetEmployeeRoleByIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/1/2014 1:57:20 PM">Created.</change>
// </history>

namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get employee role by ID criteria.
    /// </summary>
    public class GetEmployeeRoleByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "UserCtrl_GetEmployeeBySysId";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
