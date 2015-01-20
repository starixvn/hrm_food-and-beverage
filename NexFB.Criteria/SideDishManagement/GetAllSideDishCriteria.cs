// ----------------------------------------------------------------------
// <copyright file="GetAllSideDishCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 9:46:30 AM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all side dish criteria.
    /// </summary>
    public class GetAllSideDishCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        public bool? IsActive { get; set; }

        public string EmployeeTypeId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
