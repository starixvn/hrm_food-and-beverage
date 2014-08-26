// ----------------------------------------------------------------------
// <copyright file="RoleSystemModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 1:53:55 PM">Created.</change>
// </history>

namespace NexFB.Models.UserControl
{
    /// <summary>
    /// The model of role system.
    /// </summary>
    public class RoleSystemModel
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public bool Permission { get; set; }
    }
}
