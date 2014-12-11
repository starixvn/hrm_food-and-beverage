// ----------------------------------------------------------------------
// <copyright file="GetAllEmployeeTypeCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/26/2014 3:22:00 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all employee type criteria.
    /// </summary>
    public class GetAllEmployeeTypeCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
