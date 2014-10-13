// ----------------------------------------------------------------------
// <copyright file="ActivateAlternativeMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/13/2014 9:36:02 AM">Created.</change>
// </history>

namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate or deactivate alternative menu criteria.
    /// </summary>
    public class ActivateAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int AlternativeMenuId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_EditActive";
        }

        public override bool IsValid()
        {
            return AlternativeMenuId > 0;
        }
    }
}
