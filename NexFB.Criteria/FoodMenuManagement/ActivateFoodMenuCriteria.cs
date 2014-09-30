// ----------------------------------------------------------------------
// <copyright file="ActivateFoodMenuCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/30/2014 11:02:31 AM">Created.</change>
// </history>

namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate or deactivate food menu criteria.
    /// </summary>
    public class ActivateFoodMenuCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_EditActiveByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
