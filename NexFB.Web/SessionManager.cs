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
    using System.Web;
    using NexFB.Core.Constants;
    using NexFB.Models.Authority;

    /// <summary>
    /// Session Manager.
    /// </summary>
    public class SessionManager
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

        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}
