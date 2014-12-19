// --------------------------------------------------------------------------
// <copyright file="UpdateSpecialUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Update Special Employee criteria.
    /// </summary>
    public class UpdateSpecialUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public string Code { get; set; }

        public string NickName { get; set; }

        [SpParam(SqlParamName = "CardNumber")]
        public string EmployeeCard { get; set; }

        public int DepartmentId { get; set; }

        public int EmployeeTypeId { get; set; }

        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public int ModifiedBy { get; set; }

        [SpParam(Ignored = true)]
        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_Upd_SpecialEmp";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(NickName) && DepartmentId > 0 && EmployeeTypeId > 0 && UserId > 0;
        }
    }
}
