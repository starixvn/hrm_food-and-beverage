// --------------------------------------------------------------------------
// <copyright file="GetSetMenuByIdCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get set menu by id criteria.
    /// </summary>
    public class GetSetMenuByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FoodSetID")]
        public int SetMenuId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_GetItem";
        }

        public override bool IsValid()
        {
            return SetMenuId > 0;
        }
    }
}
