// --------------------------------------------------------------------------
// <copyright file="MenuSetMenuModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.23">Created</change>
// </history>

namespace NexFB.Models.SetMenuManagement
{
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// Set menu - menu item.
    /// </summary>
    public class MenuSetMenuModel : FoodMenuModel
    {
        public int SetMenuId { get; set; }
    }
}
