// --------------------------------------------------------------------------
// <copyright file="InsertOrderMealCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.Core.Extensions;

    /// <summary>
    /// Insert Order Meal Criteria.
    /// </summary>
    public class InsertOrderMealCriteria : CriteriaBase
    {
        [Output]
        public int OrderId { get; set; }

        public int LocationId { get; set; }

        [SpParam(SqlParamName = "Owner")]
        public int OwnerId { get; set; }

        public string Foods { get; set; }

        public string Menus { get; set; }

        public string Alternative { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool TakeAway { get; set; }

        public override bool IsValid()
        {
            return OwnerId > 0 && LocationId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Insert_OrderMeal";
        }
    }
}
