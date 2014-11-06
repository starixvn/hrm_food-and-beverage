// --------------------------------------------------------------------------
// <copyright file="IGroupControlRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;

    /// <summary>
    /// The interface of User Control repository.
    /// </summary>
    public interface IGroupControlRepository
    {
        IList<GroupModel> GetGroup(ICriteria criteria);

        IEnumerable<MemberModel> GetMember(ICriteria criteria);
    }
}
