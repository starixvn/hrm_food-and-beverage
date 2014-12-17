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
    using System;

    /// <summary>
    /// Location Entity.
    /// </summary>
    public class Location
    {
        public short LocationId { get; set; }

        public string NameVN { get; set; }

        public string NameEN { get; set; }

        public string Name
        {
            get
            {
                return NameEN;
            }
        }

        public string FloorName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
