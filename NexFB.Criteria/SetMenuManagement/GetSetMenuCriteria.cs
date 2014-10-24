// --------------------------------------------------------------------------
// <copyright file="GetSetMenuCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Criteria.SetMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get set menu criteria.
    /// </summary>
    public class GetSetMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "MenuGroupID")]
        public int? MenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
