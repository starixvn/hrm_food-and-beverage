// ----------------------------------------------------------------------
// <copyright file="IBaseRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/25/2014 9:32:56 AM">Created.</change>
// </history>

namespace NexFB.Core.Repositories
{
    using Fanex.Data.Repository;
    using NexFB.Core.Models;

    /// <summary>
    /// The interface of base repository.
    /// </summary>
    public interface IBaseRepository
    {
        CommandResult Insert(ICriteria criteria, bool hasGetOutputParams = false);

        CommandResult Update(ICriteria criteria, bool hasGetOutputParams = false);

        CommandResult Delete(ICriteria criteria, bool hasGetOutputParams = false);
    }
}
