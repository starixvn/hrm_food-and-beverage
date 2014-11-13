// --------------------------------------------------------------------------
// <copyright file="ChangePasswordCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.12">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// ChangePassword Criteria.
    /// </summary>
    public class ChangePasswordCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public string Password { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_ChangePassword";
        }

        public override bool IsValid()
        {
            return UserId > 0 && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
