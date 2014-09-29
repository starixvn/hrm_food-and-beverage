// ----------------------------------------------------------------------
// <copyright file="DeleteFoodMenuByMenuIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/29/2014 11:08:42 AM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete food menu by menu Id criteria.
    /// </summary>
    public class DeleteFoodMenuByMenuIdCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_DeleteByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
