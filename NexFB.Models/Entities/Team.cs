// ----------------------------------------------------------------------
// <copyright file="Team.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/28/2014 1:40:37 PM">Created.</change>
// </history>

namespace NexFB.Models.Entities
{
    /// <summary>
    /// The Team entity.
    /// </summary>
    public class Team
    {
        public int TeamId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
