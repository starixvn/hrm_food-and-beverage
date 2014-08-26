// --------------------------------------------------------------------------
// <copyright file="MenuGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Models.Authority
{
    using System.Collections.Generic;

    /// <summary>
    /// The Menu Group.
    /// </summary>
    public class MenuGroup
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
    }
}
