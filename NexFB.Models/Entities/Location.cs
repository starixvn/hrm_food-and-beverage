// --------------------------------------------------------------------------
// <copyright file="Location.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    /// <summary>
    /// Location Entity.
    /// </summary>
    public class Location
    {
        public int LocationId { get; set; }

        public string NameVN { get; set; }

        public string NameEN { get; set; }

        public string Name
        {
            get
            {
                return NameEN;
            }
        }
    }
}
