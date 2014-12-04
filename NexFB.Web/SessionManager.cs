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
    using System;
    using System.Web;
    using NexFB.Core.Constants;
    using NexFB.Models.Authority;

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

        /// <summary>
        /// Function to clear session manager.
        /// </summary>
        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                CurrentUser = null;
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }

        /// <summary>
        /// Sets a value to session.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="value">Value needs to be cached.</param>
        public static void Set<T>(string sessionKey, T value)
        {
            HttpContext.Current.Session[sessionKey] = value;
        }

        /// <summary>
        /// Gets a value from Session, then convert it to a specific type.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Value associated with passed key.</returns>
        public static T Get<T>(string sessionKey)
        {
            try
            {
                T value = (T)HttpContext.Current.Session[sessionKey];
                return value;
            }
            catch (NullReferenceException)
            {
                return default(T);
            }
        }
    }
}
