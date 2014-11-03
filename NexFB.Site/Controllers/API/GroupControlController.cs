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
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.GroupControl;
    using NexFB.Models.GroupControl;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// The user control controller.
    /// </summary>
    public class GroupControlController : BaseController
    {
        private IGroupControlService _groupControlService;
        private readonly IWebCommonService _webCommonService;

        public GroupControlController()
            : this(null, null)
        {
        }

        public GroupControlController(
            IGroupControlService groupControlService,
            IWebCommonService webCommonService)
        {
            _groupControlService = groupControlService ?? new GroupControlService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Read)]
        public IEnumerable<GroupModel> GetGroup(string groupId, bool? isActive)
        {
            var criteria = new GetGroupCriteria
                {
                    GroupID = groupId,
                    IsActive = isActive
                };

            return _groupControlService.GetGroup(criteria);
        }

        public int GetUserPermissionOnManageGroup()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageGroup);
            return policy;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        public CommandResult DeleteGroup(int groupId)
        {
            var criteria = new DeleteGroupByGroupIdCriteria
            {
                GroupId = groupId,
                ModifiedBy = CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _groupControlService.DeleteGroup(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult InsertGroup(InsertGroupCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.CreatedBy = CurrentUser.UserId;
                criteria.CreatedDate = DateTime.Now;
            }

            return _groupControlService.InsertGroup(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        [HttpPut]
        public CommandResult UpdateGroup(UpdateGroupCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _groupControlService.UpdateGroup(criteria);
        }
    }
}