// ----------------------------------------------------------------------
// <copyright file="UserControlController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 2:04:39 PM">Created.</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Models;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.UserControl;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The user control controller.
    /// </summary>
    public class UserControlController : BaseController
    {
        private IUserControlService _userControlService;

        private ICommonService _commonService;

        public UserControlController()
            : this(null, null)
        {
        }

        public UserControlController(IUserControlService userControlService, ICommonService commonService)
        {
            _userControlService = userControlService ?? new UserControlService();
            _commonService = commonService ?? new CommonService();
        }

        [HttpGet]
        ////[ApiAuthorize(FunctionKey = FBFunctionKey.ManageRole, Policy = Policy.Read)]
        public IEnumerable<FunctionModel> GetFunctionByRole(int roleId)
        {
            var criteria = new GetFunctionByRoleCriteria()
            {
                RoleId = roleId
            };

            return _userControlService.GetFunctionByRole(criteria);
        }

        [HttpGet]
        public IEnumerable<RoleSystemModel> GetAllSystemRole()
        {
            var criteria = new GetAllSystemRoleCriteria();

            return _userControlService.GetAllSystemRole(criteria);
        }

        [HttpPost]
        ////[ApiAuthorize(FunctionKey = FBFunctionKey.ManageRole, Policy = Policy.Host)]
        public CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = 248;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _userControlService.AssignPermissionToRole(criteria);
        }

        /// <summary>
        /// Get permission of user on current function.
        /// </summary>
        /// <returns>The policy.</returns>
        [HttpGet]
        public int GetManageRolePermission()
        {
            int policy = _commonService.GetPermission(FBFunctionKey.ManageRole);
            return policy;
        }
    }
}