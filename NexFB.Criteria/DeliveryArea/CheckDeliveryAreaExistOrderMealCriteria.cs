// ----------------------------------------------------------------------
// <copyright file="CheckDeliveryAreaExistOrderMealCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 10:07:55 AM">Created.</change>
// </history>

namespace NexFB.Criteria.DeliveryArea
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Check delivery area exist in order meal.
    /// </summary>
    public class CheckDeliveryAreaExistOrderMealCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "LocationID")]
        public int DeliveryAreaId { get; set; }

        public string StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Location_ExistOrderMealByLocationID";
        }

        public override bool IsValid()
        {
            return DeliveryAreaId > 0;
        }
    }
}
