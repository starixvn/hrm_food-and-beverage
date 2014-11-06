// --------------------------------------------------------------------------
// <copyright file="GroupControlService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.31">Created</change>
// </history>

namespace NexFB.Services.Implement
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Models;
    using NexFB.Core.Services;
    using NexFB.Criteria.GroupControl;
    using NexFB.Models.GroupControl;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface;

    /// <summary>
    /// Group Control Service.
    /// </summary>
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

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Command result.</returns>
        public CommandResult DeleteGroup(DeleteGroupByGroupIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Delete(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Insert group.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Command result.</returns>
        public CommandResult InsertGroup(InsertGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Command result.</returns>
        public CommandResult UpdateGroup(UpdateGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Update(criteria);
            }
            else
            {
                commandResult.IsSuccessful = false;
            }

            return commandResult;
        }

        public IEnumerable<MemberModel> GetMember(GetMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _groupControlRepository.GetMember(criteria);
        }

        /// <summary>
        /// Assign member to group.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Command result.</returns>
        public CommandResult AssignMemberToGroup(InsertMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            var commandResult = new CommandResult();

            if (criteria.IsValid())
            {
                commandResult = Repository.Insert(criteria);
            }

            return commandResult;
        }
    }
}
