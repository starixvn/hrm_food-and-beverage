// ----------------------------------------------------------------------
// <copyright file="GetAllDepartmentCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/29/2014 11:11:46 AM">Created.</change>
// </history>

namespace NexFB.Criteria.Department
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all department criteria.
    /// </summary>
    public class GetAllDepartmentCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Department_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
