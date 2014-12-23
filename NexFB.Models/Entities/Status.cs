// --------------------------------------------------------------------------
// <copyright file="Status.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.9">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// Status Class.
    /// </summary>
    [Serializable]
    public class Status
    {
        public int StatusId { get; set; }

        public string Name { get; set; }
    }
}
