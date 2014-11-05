namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public IEnumerable<object> GetAllGroup()
        {
            var criteria = new GetGroupCriteria();

            var groups = _groupControlService.GetGroup(criteria);

            return groups.Select(t => new { t.GroupId, t.GroupName });
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

        #endregion

        #region Manage User In Group

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

        public int GetUserPermissionOnManageUserInGroup()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.ManageUserInGroup);
            return policy;
        }

        #endregion
    }
}