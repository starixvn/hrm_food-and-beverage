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

        CommandResult InsertMember(InsertMemberCriteria criteria);

        CommandResult RemoveMember(RemoveMemberCriteria criteria);
    }
}
