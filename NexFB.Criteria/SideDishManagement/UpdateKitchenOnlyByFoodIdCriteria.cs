// ----------------------------------------------------------------------
// <copyright file="UpdateKitchenOnlyByFoodIdCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/26/2014 1:27:09 PM">Created.</change>
// </history>

namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Edit kitchen only by food Id criteria.
    /// </summary>
    public class UpdateKitchenOnlyByFoodIdCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public bool KitchenOnly { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditKitchenOnlyByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0;
        }
    }
}
