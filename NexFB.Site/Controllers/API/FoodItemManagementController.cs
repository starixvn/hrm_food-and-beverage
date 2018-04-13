// ----------------------------------------------------------------------
// <copyright file="FoodItemManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/16/2014 2:09:44 PM">Created.</change>
//   <change who="Quinn.Vo" date="2014.10.27">Add UpdateStatusFoodItem.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodItemManagement;
    using NexFB.Models.FoodItemManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Food item management controller.
    /// </summary>
    public class FoodItemManagementController : BaseController
    {
        private readonly IFoodItemService _foodItemService;
        private readonly IWebCommonService _webCommonService;

        public FoodItemManagementController()
            : this(null, null)
        {
        }

        public FoodItemManagementController(
            IFoodItemService foodItemService,
            IWebCommonService webCommonService)
        {
            _foodItemService = foodItemService ?? new FoodItemService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Read)]
        public IList<FoodItemModel> GetAllFoodItem(string setMenuId = "", bool? isActive = null)
        {
            var criteria = new GetAllFoodItemCriteria
            {
                SetMenuId = setMenuId,
                IsActive = isActive
            };

            return _foodItemService.GetAllFoodItem(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertFoodItem(InsertFoodItemCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _foodItemService.InsertFoodItem(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateFoodItem(UpdateFoodItemCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _foodItemService.UpdateFoodItem(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Host)]
        [HttpDelete]
        public CommandResult DeleteFoodItem(int foodId)
        {
            var criteria = new DeleteFoodItemCriteria
            {
                FoodId = foodId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _foodItemService.DeleteFoodItem(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateStatusFoodItem(UpdateStatusFoodItemCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _foodItemService.UpdateStatusFoodItem(criteria);
        }

        public int GetUserPermissionOnFoodItem()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.FoodItemManagement);
            return policy;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodItemManagement, Policy = Policy.Read)]
        public IEnumerable<FoodItemModel> GetBasicFoodItem(
            int? foodId = null,
            string menuGroupId = null,
            bool? isActive = null)
        {
            var criteria = new GetBasicFoodItemCriteria
            {
                FoodId = foodId,
                MenuGroupId = menuGroupId,
                IsActive = isActive
            };

            return _foodItemService.GetBasicFoodItem(criteria);
        }
    }
}