// ----------------------------------------------------------------------
// <copyright file="InsertDeliveryAreaCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:13:44 AM">Created.</change>
// </history>

namespace NexFB.Criteria.DeliveryArea
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert delivery area criteria.
    /// </summary>
    public class InsertDeliveryAreaCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "VNMName")]
        public string NameVN { get; set; }

        [SpParam(SqlParamName = "ENGName")]
        public string NameEN { get; set; }

        public string FloorName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Location_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FloorName) && (!string.IsNullOrEmpty(NameEN) || !string.IsNullOrEmpty(NameVN));
        }
    }
}
