// ----------------------------------------------------------------------
// <copyright file="IDeliveryAreaService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:36:40 AM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.DeliveryArea;

    /// <summary>
    /// The interface of delivery area service..
    /// </summary>
    public interface IDeliveryAreaService
    {
        IEnumerable<DeliveryAreaModel> GetAllDeliveryArea(GetAllDeliveryAreaCriteria criteria);
    }
}
