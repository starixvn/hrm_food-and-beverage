// ----------------------------------------------------------------------
// <copyright file="DeliveryAreaService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:39:16 AM">Created.</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.DeliveryArea;
    using NexFB.Services.Interface;

    /// <summary>
    /// The implement of Delivery Area service.
    /// </summary>
    public class DeliveryAreaService : BaseService, IDeliveryAreaService
    {
        /// <summary>
        /// Get all delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The enumerator Delivery Area model.</returns>
        public IEnumerable<DeliveryAreaModel> GetAllDeliveryArea(GetAllDeliveryAreaCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            IEnumerable<DeliveryAreaModel> deliveryAreas = null;

            if (criteria.IsValid())
            {
                deliveryAreas = GetData<DeliveryAreaModel>(criteria);
            }

            return deliveryAreas;
        }
    }
}
