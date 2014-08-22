// ----------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:32:16 PM">Created.</change>
// </history>
 
namespace NexFB.Site.App_Start
{
    using System.Web.Http;

    /// <summary>
    /// Web API config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register API route.
        /// </summary>
        /// <param name="config">The http configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CustomApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
