// --------------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Models.Authority
{
    /// <summary>
    /// The Menu Item.
    /// </summary>
    public class MenuItem
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string MenuLink { get; set; }

        public int? ParentId { get; set; }

        public int FunctionId { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsDisabled { get; set; }
    }
}
