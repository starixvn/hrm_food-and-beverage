// --------------------------------------------------------------------------
// <copyright file="IGroupControlService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.GroupControl;
    using NexFB.Models.GroupControl;

    /// <summary>
    /// The interface of group control service.
    /// </summary>
    public interface IGroupControlService
    {
        IEnumerable<GroupModel> GetGroup(GetGroupCriteria criteria);

        CommandResult DeleteGroup(DeleteGroupByGroupIdCriteria criteria);

        CommandResult InsertGroup(InsertGroupCriteria criteria);

        CommandResult UpdateGroup(UpdateGroupCriteria criteria);

        IEnumerable<MemberModel> GetMember(GetMemberCriteria criteria);

        CommandResult AssignMemberToGroup(InsertMemberCriteria criteria);

        CommandResult DeleteMemberGroup(DeleteMemberGroupCriteria criteria);

        CommandResult ActivateGroup(ActivateGroupCriteria criteria);

        CommandResult AssignGroupToMember(AssignGroupToMemberCriteria criteria);
    }
}
