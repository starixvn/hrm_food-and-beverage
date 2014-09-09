// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Web
{
    using System.Collections.Generic;
    using System.Web;
    using NexFB.Core.Constants;
    using NexFB.Models.Authority;
    using NexFB.Models.UserControl;

    /// <summary>
    /// Session Manager.
    /// </summary>
    public static class SessionManager
    {
        public static UserSession CurrentUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return (UserSession)HttpContext.Current.Session[Constants.CurrentSessionKey];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                HttpContext.Current.Session[Constants.CurrentSessionKey] = value;
            }
        }

        public static IList<SystemRole> UserRoles { get; set; }

        public static IList<FunctionModel> Functions { get; set; }

        public static IList<MenuGroup> MenuGroups { get; set; }

        /// <summary>
        /// Function to clear session manager.
        /// </summary>
        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                UserRoles = null;
                Functions = null;
                MenuGroups = null;
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}
