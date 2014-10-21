// ----------------------------------------------------------------------
// <copyright file="ISideDishRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/9/2014 10:06:23 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using NexFB.Models.SideDishManagement;

    /// <summary>
    /// The side dish management repository.
    /// </summary>
    public interface ISideDishRepository
    {
        IEnumerable<SideDishModel> GetAllSideDish(ICriteria criteria);

        IEnumerable<SideDishModel> GetSideDishByUserId(ICriteria criteria);
    }
}
