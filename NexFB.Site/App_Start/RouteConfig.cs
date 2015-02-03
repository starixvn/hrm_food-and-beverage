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
    using System.Collections.Generic;
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

            var homeRouteUrls = new List<string> 
            {
                "ManageRole/Index",
                "ManageUser/Index",
                "Ordering/Index",
                "Registration/Index",
                "FoodItemManagement/Index",
                "MenuManagement/Index",
                "SetMenuManagement/Index",
                "MealPlanningManagement/Index",
                "Home/AccessDenied",
                "SideDishManagement/Index",
                "AlternativeMenuManagement/Index",
                "ShiftMealMapping/Index",
                "MonthlyReportLocal/Index",
                "ManageGroup/Index",
                "ManageGroup/User",
                "DailyMainDishReport/Index",
                "MonthlyReportPerDay/Index",
                "SideDishReport/Index",
                "DeliveryArea/Index",
                "EmployeeOrderHistory/Index",
                "MealForecastReport/Index",
                "SpecialEmployee/Index",
                "SideDishSummaryReport/Index",
                "ViewMenuReport/Index",
                "MealForecastReport/Detail"
            };
            homeRouteUrls.ForEach(url => routes.MapRoute(
                name: url,
                url: url,
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}