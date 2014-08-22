// ----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Viet Nam">
// Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/22/2014 9:35:09 AM">Created.</change>
// </history>

namespace NexFB.Site
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using NexFB.Site.App_Start;

    /// <summary>
    /// The MVC Application.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}