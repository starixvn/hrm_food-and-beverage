// --------------------------------------------------------------------------
// <copyright file="DeleteSetMenuCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Criteria.SetMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete set menu by id criteria.
    /// </summary>
    public class DeleteSetMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FoodSetID")]
        public int SetMenuId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_DeleteByFoodSetID";
        }

        public override bool IsValid()
        {
            return SetMenuId > 0;
        }
    }
}
