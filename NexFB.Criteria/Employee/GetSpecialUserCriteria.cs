// --------------------------------------------------------------------------
// <copyright file="GetSpecialUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.5">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Special Employee Criteria.
    /// </summary>
    public class GetSpecialUserCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        [SpParam(SqlParamName = "Status")]
        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_Sel_SpecialEmp";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
