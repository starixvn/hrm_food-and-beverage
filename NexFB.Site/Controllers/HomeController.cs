// ----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/22/2014 9:03:31 AM">Created.</change>
// </history>

namespace NexFB.Site.Controllers
{
    using System.Web.Mvc;
    using NexFB.Models.Authority;
    using NexFB.Web;

    /// <summary>
    /// The home controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionManager.CurrentUser != null)
            {
                UserSession model = SessionManager.CurrentUser;
                return View(model);
            }

            Response.Redirect("/auth/Login");
            return View();
        }
    }
}
