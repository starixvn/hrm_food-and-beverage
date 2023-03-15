namespace NexFB.Site
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Fanex.Data.Configuration;
    using HRM.UserControl.Client.AspNetMvc.Configurations;
    using HRM.UserControl.Client.AspNetMvc.Session;
    using HRM.UserControl.Client.Core;

    public class MvcApplication : HttpApplication
    {
        private const string _WebApiPrefix = "api";
        private const string _WebReportApiPrefix = "api/report";
        private static string _webApiExecutionPath = string.Format("~/{0}", _WebApiPrefix);
        private static string _webReportApiExecutionPath = string.Format("~/{0}", _WebReportApiPrefix);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapHubs();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DbSettingProviderManager.Current.Start();
            UserControlClientManager
              .UseSettings(new UserControlSettings())
              .UseSession(new UserControlSession());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
                Exception ex = Server.GetLastError().GetBaseException();
                string message = ex.Message +
                        "\r\nSource: " + ex.Source +
                        "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                        "\r\nTargetSite: " + ex.TargetSite +
                        "\r\nStackTrace: " + ex.StackTrace;
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                if (IsReportApiRequest())
                {
                    HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
                }
                else
                {
                    HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.ReadOnly);
                }
            }
        }

        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_webApiExecutionPath, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsReportApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_webReportApiExecutionPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}