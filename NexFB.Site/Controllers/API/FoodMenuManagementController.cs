// ----------------------------------------------------------------------
// <copyright file="FoodMenuManagementController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/18/2014 10:36:15 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.FoodMenuManagement;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Food menu management.
    /// </summary>
    public class FoodMenuManagementController : BaseController
    {
        private readonly IFoodMenuService _foodMenuService;
        private readonly IWebCommonService _webCommonService;

        public FoodMenuManagementController() 
            : this(null, null)
        {
        }

        public FoodMenuManagementController(
            IFoodMenuService foodMenuService,
            IWebCommonService webCommonService)
        {
            _foodMenuService = foodMenuService ?? new FoodMenuService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodMenuManagement, Policy = Policy.Read)]
        public IList<FoodMenuModel> GetAllFoodMenu(
            string mealId = "",
            string employeeTypeId = "",
            bool? isActive = null)
        {
            var criteria = new GetAllFoodMenuCriteria
            {
                EmployeeTypeId = employeeTypeId,
                MealId = mealId,
                IsActive = isActive
            };

            return _foodMenuService.GetAllFoodMenu(criteria);
        }

        public IEnumerable<FoodMenuModel> GetAllFoodMenuToFilter()
        {
            var criteria = new GetAllFoodMenuCriteria
            {
                IsActive = true
            };

            return _foodMenuService.GetAllFoodMenuToFilter(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodMenuManagement, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertFoodMenu(InsertFoodMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = this.CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _foodMenuService.InsertFoodMenu(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodMenuManagement, Policy = Policy.Host)]
        public CommandResult DeleteFoodMenu(int menuId)
        {
            var criteria = new DeleteFoodMenuByMenuIdCriteria
            {
                MenuId = menuId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _foodMenuService.DeleteFoodMenu(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodMenuManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateFoodMenu(UpdateFoodMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _foodMenuService.UpdateFoodMenu(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.FoodMenuManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult ActivateFoodMenu(ActivateFoodMenuCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _foodMenuService.ActivateFoodMenu(criteria);
        }

        public int GetUserPermissionOnFoodMenu()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.FoodMenuManagement);
            return policy;
        }
    }
}