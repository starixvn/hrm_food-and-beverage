// ----------------------------------------------------------------------
// <copyright file="SideDishManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 11:04:06 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.SideDishManagement;
    using NexFB.Models.SideDishManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Side dish management controller.
    /// </summary>
    public class SideDishManagementController : BaseController
    {
        private readonly ISideDishService _sideDishService;
        private readonly IWebCommonService _webCommonService;

        public SideDishManagementController()
            : this(null, null)
        {
        }

        public SideDishManagementController(
            ISideDishService sideDishService,
            IWebCommonService webCommonService)
        {
            _sideDishService = sideDishService ?? new SideDishService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishManagement, Policy = Policy.Read)]
        public IEnumerable<SideDishModel> GetAllSideDish(
            int? foodId = null,
            bool? isActive = null,
            string employeeTypeId = null)
        {
            var criteria = new GetAllSideDishCriteria
            {
                IsActive = isActive,
                FoodId = foodId,
                EmployeeTypeId = employeeTypeId
            };

            return _sideDishService.GetAllSideDish(criteria);
        }

        public IEnumerable<UnitModel> GetAllFoodUnit(int? foodUnitId = null)
        {
            var criteria = new GetAllFoodUnitCriteria
            {
                FoodUnitId = foodUnitId
            };

            return _sideDishService.GetAllFoodUnit(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishManagement, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertSideDish(InsertSideDishCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _sideDishService.InsertSideDish(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateSideDish(UpdateSideDishCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _sideDishService.UpdateSideDish(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult ActivateSideDish(ActivateSideDishCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _sideDishService.ActivateSideDish(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.SideDishManagement, Policy = Policy.Host)]
        [HttpDelete]
        public CommandResult DeleteSideDish(int foodId)
        {
            var criteria = new DeleteSideDishCriteria
            {
                FoodId = foodId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _sideDishService.DeleteSideDish(criteria);
        }

        public int GetUserPermissionOnSideDish()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.SideDishManagement);
            return policy;
        }

        /// <summary>
        /// Update kitchen only by food Id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPut]
        public CommandResult UpdateKitchenOnlyByFoodId(UpdateKitchenOnlyByFoodIdCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _sideDishService.UpdateKitchenOnlyByFoodId(criteria);
        }
    }
}