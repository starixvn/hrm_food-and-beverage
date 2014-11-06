// --------------------------------------------------------------------------
// <copyright file="GroupControlController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
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
        private readonly IGroupControlService _groupControlService;

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

        #region Manage Group

        /// <summary>
        /// Get all group.
        /// </summary>
        /// <returns>Group list.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accept")]
        public IEnumerable<object> GetAllGroup()
        {
            var criteria = new GetGroupCriteria();

            var groups = _groupControlService.GetGroup(criteria);

            return groups.Select(t => new { t.GroupId, t.GroupName });
        }

        /// <summary>
        /// Get group.
        /// </summary>
        /// <param name="groupId">GroupId input.</param>
        /// <param name="isActive">IsActive input.</param>
        /// <returns>Group list.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Read)]
        public IEnumerable<GroupModel> GetGroup(string groupId = null, bool? isActive = null)
        {
            var criteria = new GetGroupCriteria
                {
                    GroupId = groupId,
                    IsActive = isActive
                };

            return _groupControlService.GetGroup(criteria);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accept")]
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

        /// <summary>
        /// Update Group.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Command result.</returns>
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

        #endregion

        #region Manage User In Group

        /// <summary>
        /// Get members.
        /// </summary>
        /// <param name="departmentId">DepartmentId input.</param>
        /// <param name="employeeTypeId">EmployeeTypeId input.</param>
        /// <param name="query">Query input.</param>
        /// <param name="groupId">GroupId input.</param>
        /// <param name="joinedDateFrom">Joined date from.</param>
        /// <param name="joinedDateTo">Joined date to.</param>
        /// <returns>Member list.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUserInGroup, Policy = Policy.Read)]
        public IEnumerable<MemberModel> GetMember(
            string departmentId = null,
            string employeeTypeId = null,
            string query = null,
            string groupId = null,
            DateTime? joinedDateFrom = null,
            DateTime? joinedDateTo = null)
        {
            var criteria = new GetMemberCriteria
                {
                    DepartmentId = departmentId,
                    EmployeeTypeId = employeeTypeId,
                    Query = query,
                    GroupId = groupId,
                    JoinedDateFrom = joinedDateFrom,
                    JoinedDateTo = joinedDateTo
                };

            return _groupControlService.GetMember(criteria);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accept")]
        public int GetUserPermissionOnManageUserInGroup()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageUserInGroup);
            return policy;
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult AssignMemberToGroup(InsertMemberCriteria criteria)
        {
            if (criteria != null)
            {
                TransformXmlPara(criteria);
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _groupControlService.AssignMemberToGroup(criteria);
        }

        /// <summary>
        /// Transform xml para for criteria.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        private void TransformXmlPara(InsertMemberCriteria criteria)
        {
            var sysIds = criteria.SysIds.Split(';').ToList();
            var rawGroupIds = criteria.GroupIds.Split(';').ToList();

            var document = new StringBuilder();
            document.Append("<ROOT>");

            if (sysIds.Count > 0 && sysIds.Count == rawGroupIds.Count)
            {
                var length = sysIds.Count;

                for (int i = 0; i < length; i++)
                {
                    var groups = rawGroupIds[i].Split(',').ToList();

                    foreach (var group in groups)
                    {
                        document.AppendFormat("<User SysId='{0}' GroupId='{1}'></User>", sysIds[i], group);
                    }
                }
            }

            document.Append("</ROOT>");

            criteria.MemberId = document.ToString();
        }

        #endregion
    }
}