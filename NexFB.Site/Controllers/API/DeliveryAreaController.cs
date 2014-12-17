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
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.DeliveryArea;
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
        public IEnumerable<DeliveryAreaModel> GetAllDeliveryArea(
            bool isActive,
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
    }
}
