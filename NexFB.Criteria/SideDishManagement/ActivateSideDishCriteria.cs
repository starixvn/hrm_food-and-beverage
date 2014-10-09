// ----------------------------------------------------------------------
// <copyright file="ActivateSideDishCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 2:56:41 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate side dish criteria.
    /// </summary>
    public class ActivateSideDishCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_EditActiveByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
