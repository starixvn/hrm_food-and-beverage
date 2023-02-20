namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
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
        public IEnumerable<GroupModel> GetGroup(string groupId = null, bool? isActive = null)
        {
            var criteria = new GetGroupCriteria
            {
                GroupId = groupId,
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

        #endregion Manage Group

        #region Manage User In Group

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageUserInGroup, Policy = Policy.Read)]
        public IEnumerable<MemberModel> GetMember(
            string companyId = null,
            string employeeTypeId = null,
            string teamId = null,
            string query = null,
            string groupId = null,
            DateTime? joinedDateFrom = null,
            DateTime? joinedDateTo = null)
        {
            var criteria = new GetMemberCriteria
            {
                CompanyId = companyId,
                EmployeeTypeId = employeeTypeId,
                TeamId = teamId,
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

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult AssignMemberToGroup(InsertMemberCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.MemberId = TransformXmlPara(criteria.SysIds, criteria.GroupIds);
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _groupControlService.AssignMemberToGroup(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.ManageGroup, Policy = Policy.Host)]
        [HttpPost]
        public CommandResult AssignGroupToMember(AssignGroupToMemberCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.MemberId = TransformXmlPara(criteria.SysIds, criteria.GroupIds);
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _groupControlService.AssignGroupToMember(criteria);
        }

        [HttpDelete]
        public CommandResult DeleteMemberGroup(string memberId, int groupId)
        {
            var criteria = new DeleteMemberGroupCriteria
            {
                MemberId = memberId,
                GroupId = groupId,
                ModifiedBy = CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _groupControlService.DeleteMemberGroup(criteria);
        }

        [HttpPut]
        public CommandResult ActivateGroup(ActivateGroupCriteria criteria)
        {
            if (criteria != null)
            {
                criteria.ModifiedBy = CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _groupControlService.ActivateGroup(criteria);
        }

        private string TransformXmlPara(string rawSysIds, string rawGroupIds)
        {
            List<string> sysIds = rawSysIds.Split(',').ToList();
            List<string> groupIds = rawGroupIds.Split(',').ToList();

            var document = new StringBuilder();
            document.Append("<ROOT>");

            if (sysIds != null && sysIds.Count > 0)
            {
                int numberUser = sysIds.Count;

                for (int i = 0; i < numberUser; i++)
                {
                    string sysId = sysIds[i];

                    if (groupIds != null && groupIds.Count > 0)
                    {
                        int numberGroups = groupIds.Count;

                        for (int j = 0; j < numberGroups; j++)
                        {
                            string group = groupIds[j];
                            document.AppendFormat("<User SysId='{0}' GroupId='{1}'></User>", sysId, group);
                        }
                    }
                    else
                    {
                        document.AppendFormat("<User SysId='{0}' GroupId='{1}'></User>", sysId, string.Empty);
                    }
                }
            }

            document.Append("</ROOT>");

            return document.ToString();
        }

        #endregion Manage User In Group
    }
}