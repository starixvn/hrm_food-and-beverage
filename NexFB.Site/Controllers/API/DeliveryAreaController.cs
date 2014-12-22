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
    using System.Linq;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.DeliveryArea;
    using NexFB.Models.Entities;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// The delivery area controller.
    /// </summary>
    public class DeliveryAreaController : BaseController
    {
        private readonly IDeliveryAreaService _deliveryAreaService;
        private readonly IWebCommonService _webCommonService;

        public DeliveryAreaController()
            : this(null, null)
        {
        }

        public DeliveryAreaController(IDeliveryAreaService deliveryAreaService, IWebCommonService webCommonService)
        {
            _deliveryAreaService = deliveryAreaService ?? new DeliveryAreaService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [HttpGet]
        [ApiAuthorize(FunctionKey = FBFunctionKey.DeliveryAreaManagement, Policy = Policy.Read)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.DeliveryAreaManagement, Policy = Policy.Host)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.DeliveryAreaManagement, Policy = Policy.Host)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.DeliveryAreaManagement, Policy = Policy.Host)]
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
        [ApiAuthorize(FunctionKey = FBFunctionKey.DeliveryAreaManagement, Policy = Policy.Host)]
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

        public int GetUserPermissionOnDeliveryArea()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.DeliveryAreaManagement);
            return policy;
        }

        /// <summary>
        /// Check delivery area is exist or not.
        /// </summary>
        /// <param name="deliveryAreaId">The delivery Area Id.</param>
        /// <returns>The boolean value.</returns>
        [HttpGet]
        public bool CheckDeliveryAreaExist(int deliveryAreaId)
        {
            var criteria = new GetAllDeliveryAreaCriteria
            {
                DeliveryAreaId = deliveryAreaId
            };
            IEnumerable<Location> locations = _deliveryAreaService.GetAllDeliveryArea(criteria);

            if (locations != null && locations.Count() > 0)
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        public bool CheckDeliveryAreaExistOrderMeal(int deliveryAreaId, string statusId)
        {
            var criteria = new CheckDeliveryAreaExistOrderMealCriteria
            {
                DeliveryAreaId = deliveryAreaId,
                StatusId = statusId
            };

            return _deliveryAreaService.CheckDeliveryAreaExistOrderMeal(criteria);
        }
    }
}
