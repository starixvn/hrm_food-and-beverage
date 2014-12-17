// ----------------------------------------------------------------------
// <copyright file="DeliveryAreaModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:23:12 AM">Created.</change>
// </history>

namespace NexFB.Models.DeliveryArea
{
    using System;

    /// <summary>
    /// The delivery area model.
    /// </summary>
    public class DeliveryAreaModel
    {
        public int DeliveryAreaId { get; set; }

        public string VNMName { get; set; }

        public string ENGName { get; set; }

        public string FloorName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
