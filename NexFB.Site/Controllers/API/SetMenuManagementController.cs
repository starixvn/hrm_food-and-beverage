// --------------------------------------------------------------------------
// <copyright file="SetMenuManagementController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.SetMenuManagement;
    using NexFB.Models.SetMenuManagement;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Set menu management.
    /// </summary>
    public class SetMenuManagementController : BaseController
    {
        private readonly ISetMenuService _setMenuService;
        private readonly IWebCommonService _webCommonService;

        public SetMenuManagementController() 
            : this(null, null)
        {
        }

        public SetMenuManagementController(
            ISetMenuService setMenuService,
            IWebCommonService webCommonService)
        {
            _setMenuService = setMenuService ?? new SetMenuService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        /// <summary>
        /// Function insert a Set Menu.
        /// </summary>
        /// <param name="model">SetMenu Model.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SetMenuManagement, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertSetMenu(SetMenuModel model)
        {
            var criteria = new InsertSetMenuCriteria()
            {
                CreatedBy = this.CurrentUser.UserId,
                CreatedDate = DateTime.Now,
                SetMenuCode = model.SetMenuCode,
                SetMenuVnmName = model.SetMenuVnmName,
                SetMenuEngName = model.SetMenuEngName,
                IsActive = model.IsActive,
                RawFoodIds = model.RawFoodIds,
                RawMenuIds = model.RawMenuIds
            };

            CommandResult result = _setMenuService.InsertSetMenu(criteria);
            return result;
        }

        /// <summary>
        /// Function update a Set Menu.
        /// </summary>
        /// <param name="model">SetMenu Model.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SetMenuManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateSetMenu(SetMenuModel model)
        {
            var criteria = new UpdateSetMenuCriteria()
            {
                SetMenuId = model.SetMenuId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now,
                SetMenuCode = model.SetMenuCode,
                SetMenuVnmName = model.SetMenuVnmName,
                SetMenuEngName = model.SetMenuEngName,
                IsActive = model.IsActive,
                RawFoodIds = model.RawFoodIds,
                RawMenuIds = model.RawMenuIds
            };

            CommandResult result = _setMenuService.UpdateSetMenu(criteria);
            return result;
        }

        /// <summary>
        /// Function delete a Set Menu.
        /// </summary>
        /// <param name="setMenuId">Set menu id.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SetMenuManagement, Policy = Policy.Host)]
        public CommandResult DeleteSetMenu(int setMenuId)
        {
            var criteria = new DeleteSetMenuCriteria
            {
                SetMenuId = setMenuId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            CommandResult result = _setMenuService.DeleteSetMenu(criteria);
            return result;
        }

        /// <summary>
        /// Function to update status of set menu.
        /// </summary>
        /// <param name="model">Set menu model.</param>
        /// <returns>Command Result.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.SetMenuManagement, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateStatusSetMenu(SetMenuModel model)
        {
            var criteria = new UpdateStatusSetMenuCriteria
            {
                SetMenuId = model.SetMenuId,
                IsActive = model.IsActive,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            CommandResult result = _setMenuService.UpdateStatusSetMenu(criteria);
            return result;
        }

        public IList<SetMenuModel> GetSetMenu(string menuId = "", bool? isActive = null)
        {
            var criteria = new GetSetMenuCriteria
            {
                IsActive = isActive,
                MenuId = menuId
            };

            IList<SetMenuModel> setMenus = _setMenuService.GetSetMenu(criteria);
            return setMenus;
        }

        public SetMenuModel GetSetMenuById(int setMenuId)
        {
            var criteria = new GetSetMenuByIdCriteria
            {
                SetMenuId = setMenuId
            };

            SetMenuModel setMenu = _setMenuService.GetOrderById(criteria);
            return setMenu;
        }

        public int GetUserPermissionOnSetMenu()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.SetMenuManagement);
            return policy;
        }
    }
}