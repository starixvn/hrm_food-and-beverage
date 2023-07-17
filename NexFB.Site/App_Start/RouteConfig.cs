namespace NexFB.Site
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
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
                "MealForecastReport/Detail",
                "EmployeeRegistration/Detail",
                "AdminOrderHistoryReport/Index",
                "MealTimeManagement/Index",
                "MonthlyLunchAndDinnerReport/Index",
                "MonthlyBreakfastAndSupperReport/Index",
                "KitchenStaff/Order",
                "ManageVegetarian/Index"
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