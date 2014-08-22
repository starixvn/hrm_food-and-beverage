// ----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:31:02 PM">Created.</change>
// </history>

namespace NexFB.Site.App_Start
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route Config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="routes">The routes collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Registration Meal Index",
               url: "RegistrationMeal/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}