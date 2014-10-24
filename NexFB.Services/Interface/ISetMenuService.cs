// --------------------------------------------------------------------------
// <copyright file="ISetMenuService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.22">Created</change>
// </history>

namespace NexFB.Services.Interface
{
    using System.Collections.Generic;
    using NexFB.Core.Models;
    using NexFB.Criteria.SetMenuManagement;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The interface of set menu service.
    /// </summary>
    public interface ISetMenuService
    {
        CommandResult InsertSetMenu(InsertSetMenuCriteria criteria);

        CommandResult UpdateSetMenu(UpdateSetMenuCriteria criteria);

        CommandResult DeleteSetMenu(DeleteSetMenuCriteria criteria);

        CommandResult UpdateStatusSetMenu(UpdateStatusSetMenuCriteria criteria);

        SetMenuModel GetOrderById(GetSetMenuByIdCriteria criteria);

        IList<SetMenuModel> GetSetMenu(GetSetMenuCriteria criteria);
    }
}
