// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Menu Entity.
    /// </summary>
    public class Menu
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public IList<Food> FoodItems { get; set; }
    }
}
