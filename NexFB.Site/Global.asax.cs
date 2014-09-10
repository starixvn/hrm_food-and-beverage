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
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Validation.Providers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Fanex.Data.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// The MVC Application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        private const string _WebApiPrefix = "api";
        private static string _webApiExecutionPath = string.Format("~/{0}", _WebApiPrefix);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapHubs();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">The EventArgs.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            if (!Request.IsLocal)
            {
                Exception ex = Server.GetLastError().GetBaseException();
                string message = ex.Message +
                        "\r\nSource: " + ex.Source +
                        "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                        "\r\nTargetSite: " + ex.TargetSite +
                        "\r\nStackTrace: " + ex.StackTrace;

                LogService.Error(message);
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_webApiExecutionPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}