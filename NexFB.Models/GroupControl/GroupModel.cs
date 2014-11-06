// --------------------------------------------------------------------------
// <copyright file="GroupModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.6">Created</change>
// </history>

namespace NexFB.Models.GroupControl
{
    /// <summary>
    /// Group model.
    /// </summary>
    public class GroupModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool IsActive { get; set; }

        public int Members { get; set; }
    }
}