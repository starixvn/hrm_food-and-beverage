// ----------------------------------------------------------------------
// <copyright file="IAlternativeMenuService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 2:36:22 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Criteria.AlternativeMenuManagement;
    using NexFB.Models.AlternativeMenuManagement;

    /// <summary>
    /// The interface of alternative service.
    /// </summary>
    public interface IAlternativeMenuService
    {
        IEnumerable<AlternativeMenuModel> GetAllAlternativeMenu(GetAllAlternativeMenuCriteria criteria);
    }
}
