// --------------------------------------------------------------------------
// <copyright file="InsertSetMenuCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Criteria.SetMenuManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert set menu criteria.
    /// </summary>
    public class InsertSetMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "FoodSetID")]
        public int? SetMenuId { get; set; }

        [SpParam(SqlParamName = "FoodSetCode")]
        public string SetMenuCode { get; set; }

        [SpParam(SqlParamName = "FoodSetVNMName")]
        public string SetMenuVnmName { get; set; }

        [SpParam(SqlParamName = "FoodSetEngName")]
        public string SetMenuEngName { get; set; }

        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public IList<int> RawFoodIds { get; set; }

        [SpParam(SqlParamName = "FoodID")]
        public string FoodItemId
        {
            get
            {
                string ids = RawFoodIds != null && RawFoodIds.Any()
                    ? string.Join(",", RawFoodIds)
                    : string.Empty;
                return ids;
            }
        }

        [SpParam(Ignored = true)]
        public IList<int> RawMenuIds { get; set; }

        [SpParam(SqlParamName = "MenuGroupID")]
        public string MenuId
        {
            get
            {
                string ids = RawMenuIds != null && RawMenuIds.Any()
                    ? string.Join(",", RawMenuIds)
                    : string.Empty;
                return ids;
            }
        }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(SetMenuVnmName)
                && RawFoodIds != null && RawFoodIds.Any();
        }
    }
}
