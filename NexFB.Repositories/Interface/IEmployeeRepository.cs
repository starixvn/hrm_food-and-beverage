// --------------------------------------------------------------------------
// <copyright file="IEmployeeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.Authority;

    public interface IEmployeeRepository
    {
        UserSession GetUserByDomain(ICriteria criteria);

        UserSession GetUserByCode(ICriteria criteria);
    }
}
