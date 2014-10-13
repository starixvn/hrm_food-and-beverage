// ----------------------------------------------------------------------
// <copyright file="InsertAlternativeMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/13/2014 9:14:32 AM">Created.</change>
// </history>

namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert alternative menu criteria.
    /// </summary>
    public class InsertAlternativeMenuCriteria : CriteriaBase
    {
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

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
