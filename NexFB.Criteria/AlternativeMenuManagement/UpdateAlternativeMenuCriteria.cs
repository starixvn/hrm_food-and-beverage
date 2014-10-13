// ----------------------------------------------------------------------
// <copyright file="UpdateAlternativeMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/13/2014 9:25:52 AM">Created.</change>
// </history>

namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int AlternativeMenuId { get; set; }

        [SpParam(SqlParamName = "AltMenuCode")]
        public string AlternativeMenuCode { get; set; }

        [SpParam(SqlParamName = "AltMenuEngName")]
        public string AlternativeMenuEngName { get; set; }

        [SpParam(SqlParamName = "AltMenuVNMName")]
        public string AlternativeMenuVnmName { get; set; }

        [SpParam(SqlParamName = "AltMenuPhoto")]
        public string AlternativeMenuPhoto { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_Edit";
        }

        public override bool IsValid()
        {
            return AlternativeMenuId > 0;
        }
    }
}
