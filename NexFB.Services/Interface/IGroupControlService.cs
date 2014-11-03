// ----------------------------------------------------------------------
// <copyright file="IUserControlService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 4:49:30 PM">Created.</change>
// </history>

using NexFB.Core.Models;

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
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
    }
}
