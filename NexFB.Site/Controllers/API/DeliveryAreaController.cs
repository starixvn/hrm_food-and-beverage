// ----------------------------------------------------------------------
// <copyright file="DeliveryAreaController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/17/2014 8:44:39 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Models;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The delivery area controller.
    /// </summary>
    public class DeliveryAreaController : BaseController
    {
        private readonly IDeliveryAreaService _deliveryAreaService;

        public DeliveryAreaController()
            : this(null)
        {
        }

        public DeliveryAreaController(IDeliveryAreaService deliveryAreaService)
        {
            _deliveryAreaService = deliveryAreaService ?? new DeliveryAreaService();
        }

        [HttpGet]
        public IEnumerable<Location> GetAllDeliveryArea(
            bool? isActive = null,
            int? deliveryAreaId = null,
            string query = "")
        {
            var criteria = new GetAllDeliveryAreaCriteria
            {
                DeliveryAreaId = deliveryAreaId,
                IsActive = isActive,
                Query = query
            };

            return _deliveryAreaService.GetAllDeliveryArea(criteria);
        }

        /// <summary>
        /// Insert delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        [HttpPost]
        public CommandResult InsertDeliveryArea(InsertDeliveryAreaCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _deliveryAreaService.InsertDeliveryArea(criteria);
        }

        /// <summary>
        /// Update delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        [HttpPut]
        public CommandResult UpdateDeliveryArea(UpdateDeliveryAreaCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _deliveryAreaService.UpdateDeliveryArea(criteria);
        }

        /// <summary>
        /// Activate or deactivate delivery area.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Command Result.</returns>
        [HttpPut]
        public CommandResult ActivateDeliveryArea(ActivateDeliveryAreaCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _deliveryAreaService.ActivateDeliveryArea(criteria);
        }

        [HttpDelete]
        public CommandResult DeleteDeliveryArea(int locationId)
        {
            var criteria = new DeleteDeliveryAreaCriteria
            {
                LocationId = locationId,
                ModifiedBy = CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _deliveryAreaService.DeleteDeliveryArea(criteria);
        }
    }
}
