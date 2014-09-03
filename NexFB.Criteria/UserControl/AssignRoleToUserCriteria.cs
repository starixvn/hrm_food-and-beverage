// ----------------------------------------------------------------------
// <copyright file="AssignRoleToUserCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/1/2014 12:43:31 PM">Created.</change>
// </history>

namespace NexFB.Criteria.UserControl
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Assign role to user criteria.
    /// </summary>
    public class AssignRoleToUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int SystemId { get; set; }

        public string RoleId { get; set; }

        public string RoleDepartments { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "UserCtrl_SetEmployee";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
