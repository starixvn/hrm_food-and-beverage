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
        public int UserId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string EmployeeCard { get; set; }

        public int DepartmentId { get; set; }

        public int EmployeeTypeId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditByFoodID";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(EmployeeCard) && DepartmentId > 0 && EmployeeTypeId > 0 && UserId > 0;
        }
    }
}
