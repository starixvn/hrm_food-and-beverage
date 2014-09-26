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
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web.Security;
    using NexFB.Models.UserControl;

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

        ////public bool HasRole(short roleId)
        ////{
        ////    bool hasRole = UserRoles.Any(t => t.RoleId == roleId);
        ////    return hasRole;
        ////}

        /// <summary>
        /// Check user is in specific roles.
        /// </summary>
        /// <param name="roles">Role of user.</param>
        /// <returns>Return Boolean.</returns>
        ////public bool IsInRoles(IList<string> roles)
        ////{
        ////    if (roles == null || roles.Count == 0)
        ////    {
        ////        return false;
        ////    }
            
        ////    string[] userRoles = Roles.GetRolesForUser();
        ////    return roles.Any(role => userRoles.Contains(role));
        ////}
    }
}
