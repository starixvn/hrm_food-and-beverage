// --------------------------------------------------------------------------
// <copyright file="ISetMenuRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.23">Created</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.SetMenuManagement;

    public interface ISetMenuRepository
    {
        IList<SetMenuModel> GetSetMenu(ICriteria criteria);
    }
}
