// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.27">Created</change>
// </history>

namespace NexFB.Site.Controllers.API
{
    using System.Web.Http;
    using System.Web.Security;
    using NexFB.Models.Authority;
    using NexFB.Web;
    using NexFB.Web.Attributes;

    /// <summary>
    /// Base Controller.
    /// </summary>
    [ApiAuthorize]
    public class BaseController : ApiController
    {
        /// <summary>
        /// Gets Current Session.
        /// </summary>
        /// <value>Current Session.</value>
        protected UserSession CurrentUser
        {
            get
            {
                var currentUser = SessionManager.CurrentUser;
                if (currentUser == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

                return currentUser;
            }
        }
    }
}
