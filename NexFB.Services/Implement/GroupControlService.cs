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
    using NexFB.Services.Interface;

    /// <summary>
    /// Group Control Service.
    /// </summary>
    public class GroupControlService : BaseService, IGroupControlService
    {
        public IEnumerable<GroupModel> GetGroup(GetGroupCriteria criteria)
        {
            return GetData<GroupModel>(criteria);
        }

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
    }
}
