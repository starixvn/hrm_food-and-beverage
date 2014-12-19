// --------------------------------------------------------------------------
// <copyright file="DeleteSpecialUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.5">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete Special Employee criteria.
    /// </summary>
    public class DeleteSpecialUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        [SpParam(Ignored = true)]
        public int ModifiedBy { get; set; }

        [SpParam(Ignored = true)]
        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_Del_SpecialEmp";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
