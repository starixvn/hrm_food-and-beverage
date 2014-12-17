// ----------------------------------------------------------------------
// <copyright file="ActivateDeliveryAreaCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 3:11:01 PM">Created.</change>
// </history>

namespace NexFB.Criteria.DeliveryArea
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate delivery area criteria.
    /// </summary>
    public class ActivateDeliveryAreaCriteria : CriteriaBase
    {
        public int LocationId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Location_EditActiveByLocationID";
        }

        public override bool IsValid()
        {
            return LocationId > 0;
        }
    }
}
