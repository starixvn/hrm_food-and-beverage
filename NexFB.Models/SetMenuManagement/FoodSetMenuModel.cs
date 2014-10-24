// --------------------------------------------------------------------------
// <copyright file="FoodSetMenuModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.23">Created</change>
// </history>

namespace NexFB.Models.SetMenuManagement
{
    using NexFB.Models.FoodItemManagement;

    /// <summary>
    /// Set menu - food item.
    /// </summary>
    public class FoodSetMenuModel : FoodItemModel
    {
        public int SetMenuId { get; set; }
    }
}
