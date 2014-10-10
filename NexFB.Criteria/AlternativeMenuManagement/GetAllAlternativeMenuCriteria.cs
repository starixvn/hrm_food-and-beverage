// ----------------------------------------------------------------------
// <copyright file="GetAllAlternativeMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 3:28:46 PM">Created.</change>
// </history>

namespace NexFB.Criteria.AlternativeMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all alternative menu criteria.
    /// </summary>
    public class GetAllAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int? AlternativeMenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
