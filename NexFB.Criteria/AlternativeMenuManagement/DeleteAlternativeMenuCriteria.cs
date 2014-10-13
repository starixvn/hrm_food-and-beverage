// ----------------------------------------------------------------------
// <copyright file="DeleteAlternativeMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/13/2014 10:07:24 AM">Created.</change>
// </history>

namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete alternative menu criteria.
    /// </summary>
    public class DeleteAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int AlternativeMenuId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_Delete";
        }

        public override bool IsValid()
        {
            return AlternativeMenuId > 0;
        }
    }
}
