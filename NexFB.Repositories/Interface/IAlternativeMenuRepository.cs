// ----------------------------------------------------------------------
// <copyright file="IAlternativeMenuRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 2:33:30 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.AlternativeMenuManagement;

    /// <summary>
    /// The interface of alternative menu repository.
    /// </summary>
    public interface IAlternativeMenuRepository
    {
        IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(ICriteria criteria);
    }
}
