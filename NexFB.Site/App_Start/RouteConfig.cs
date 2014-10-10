// ----------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/21/2014 3:31:02 PM">Created.</change>
// </history>

namespace NexFB.Site
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route Config.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Register Routes.
        /// </summary>
        /// <param name="routes">The routes collection.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Login",
               url: "Auth/Login",
               defaults: new { controller = "Auth", action = "Login" });

            routes.MapRoute(
               name: "Manage Role",
               url: "ManageRole/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Manage User",
               url: "ManageUser/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Meal ordering",
               url: "Ordering/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Registration Meal",
               url: "Registration/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Food Item Management",
               url: "FoodItemManagement/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Food Menu Management",
               url: "MenuManagement/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Meal Planning Management",
               url: "MealPlanningManagement/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Access denied",
               url: "Home/AccessDenied",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
               name: "Side Dish Management",
               url: "SideDishManagement/Index",
               defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}