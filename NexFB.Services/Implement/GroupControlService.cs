namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.GroupControl;
    using NexFB.Models.GroupControl;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    public class GroupControlService : BaseService, IGroupControlService
    {
        private readonly IGroupControlRepository _groupControlRepository;

        public GroupControlService()
           : this(null)
        {
        }

        public GroupControlService(IGroupControlRepository groupControlRepository)
        {
            _groupControlRepository = groupControlRepository ?? new GroupControlRepository();
        }

        public IEnumerable<GroupModel> GetGroup(GetGroupCriteria criteria)
        {
            return GetData<GroupModel>(criteria);
        }

        public CommandResult DeleteGroup(DeleteGroupByGroupIdCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        public CommandResult InsertGroup(InsertGroupCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        public CommandResult UpdateGroup(UpdateGroupCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }
            else
            {
                commandResult.Code = 1;
            }

            return commandResult;
        }

        public IEnumerable<MemberModel> GetMember(GetMemberCriteria criteria)
        {
            return _groupControlRepository.GetMember(criteria);
        }

        public CommandResult AssignMemberToGroup(InsertMemberCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        public CommandResult AssignGroupToMember(AssignGroupToMemberCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        public CommandResult DeleteMemberGroup(DeleteMemberGroupCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        public CommandResult ActivateGroup(ActivateGroupCriteria criteria)
        {
            var commandResult = new CommandResult();
            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }

            return commandResult;
        }
    }
}