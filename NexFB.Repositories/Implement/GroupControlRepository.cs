// --------------------------------------------------------------------------
// <copyright file="GroupControlRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.31">Created</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models.GroupControl;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// The Group Control repository.
    /// </summary>
    public class GroupControlRepository : IGroupControlRepository
    {
        /// <summary>
        /// Get groups.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The group list.</returns>
        public IList<GroupModel> GetGroup(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var groups = multi.Read<GroupModel>().ToList();
                    
                    return groups;
                }
            }
        }
    }
}
