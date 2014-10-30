// --------------------------------------------------------------------------
// <copyright file="UserSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Models.Authority
{
    using System;

    /// <summary>
    /// User Session.
    /// </summary>
    [Serializable]
    public class UserSession
    {
        public int UserId { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }

        public string NickName { get; set; }

        public string FullName { get; set; }

        public int GroupId { get; set; }

        public bool IsDisabled { get; set; }

        public int DepartmentId { get; set; }
    }
}
