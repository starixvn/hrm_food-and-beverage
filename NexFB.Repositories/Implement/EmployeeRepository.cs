// --------------------------------------------------------------------------
// <copyright file="EmployeeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Authority;
    using NexFB.Repositories.Interface;

    /// <summary>
    /// Employee Repository.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        public UserSession GetUserByDomain(ICriteria criteria)
        {
            return null;
        }

        public UserSession GetUserByCode(ICriteria criteria)
        {
            return null;
        }
    }
}
