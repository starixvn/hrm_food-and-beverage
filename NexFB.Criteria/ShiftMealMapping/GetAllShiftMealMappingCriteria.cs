// ----------------------------------------------------------------------
// <copyright file="GetAllShiftMealMappingCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/14/2014 11:09:38 AM">Created.</change>
// </history>

namespace NexFB.Criteria.ShiftMealMapping
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all shift meal mapping criteria.
    /// </summary>
    public class GetAllShiftMealMappingCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }
        public string ShiftCode { get; set; }
        public string TeamCode { get; set; }

        public override string GetSettingKey()
        {
            return "Shift_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
