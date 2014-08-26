// --------------------------------------------------------------------------
// <copyright file="GetEmployeeByCodeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetEmployeeByCode Criteria.
    /// </summary>
    public class GetEmployeeByCodeCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeByCode";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Code);
        }
    }
}
