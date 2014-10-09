// ----------------------------------------------------------------------
// <copyright file="DeleteSideDishCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 3:00:18 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete side dish criteria.
    /// </summary>
    public class DeleteSideDishCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_DeleteByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
