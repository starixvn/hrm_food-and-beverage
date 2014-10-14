// --------------------------------------------------------------------------
// <copyright file="EditOrderMealCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.13">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Edit Order Meal Criteria.
    /// </summary>
    public class EditOrderMealCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public int LocationId { get; set; }

        public int Owner { get; set; }

        public string Foods { get; set; }

        public string Menus { get; set; }

        public string Alternative { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override bool IsValid()
        {
            return Owner > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Edit_OrderMeal";
        }
    }
}
