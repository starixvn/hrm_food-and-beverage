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
    using NexFB.Core.Models;
    using NexFB.Criteria.UserControl;
    using NexFB.Models.UserControl;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;

    /// <summary>
    /// The user control controller.
    /// </summary>
    public class UserControlController : ApiController
    {
        private IUserControlService _userControlService;

        public UserControlController()
            : this(null)
        {
        }

        public UserControlController(IUserControlService userControlService)
        {
            _userControlService = userControlService ?? new UserControlService();
        }

        [HttpGet]
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
        public CommandResult AssignPermissionToRole(AssignPermissionToRoleCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = 248;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _userControlService.AssignPermissionToRole(criteria);
        }
    }
}