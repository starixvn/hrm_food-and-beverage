// ----------------------------------------------------------------------
// <copyright file="UpdateDeliveryAreaCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 2:49:16 PM">Created.</change>
// </history>

namespace NexFB.Criteria.DeliveryArea
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update delivery area criteria.
    /// </summary>
    public class UpdateDeliveryAreaCriteria : CriteriaBase
    {
        public int LocationId { get; set; }

        [SpParam(SqlParamName = "VNMName")]
        public string NameVN { get; set; }

        [SpParam(SqlParamName = "ENGName")]
        public string NameEN { get; set; }

        public string FloorName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Location_EditByLocationID";
        }

        public override bool IsValid()
        {
            return LocationId > 0 && !string.IsNullOrEmpty(FloorName) && 
                    (!string.IsNullOrEmpty(NameEN) || !string.IsNullOrEmpty(NameVN));
        }
    }
}
