// ----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:31:02 PM">Created.</change>
// </history>

namespace Fanex.NexFB.Site
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route Config.
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}