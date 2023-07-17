namespace NexFB.Site
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.Sentry.RavenClient;
    using HRM.UserControl.Client.AspNetMvc.Configurations;
    using HRM.UserControl.Client.AspNetMvc.Session;
    using HRM.UserControl.Client.Core;
    using Microsoft.AspNet.SignalR;
    using SharpRaven;

    public class MvcApplication : HttpApplication
    {
        private const string _WebApiPrefix = "api";
        private const string _WebReportApiPrefix = "api/report";
        private static string _webApiExecutionPath = string.Format("~/{0}", _WebApiPrefix);
        private static string _webReportApiExecutionPath = string.Format("~/{0}", _WebReportApiPrefix);

        protected void Application_Start()
        {
            LogManager
                .SetDefaultLogCategory(ConfigurationManager.AppSettings["Sentry:Category"])
                .Use(new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(ConfigurationManager.AppSettings["Sentry:Dsn"]),
                    WriteLogTimeoutSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["Sentry:Timeout"])
                }));

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .GetHttpHeaderInfo()
                .GetHttpSessionInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

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

            GlobalHost.HubPipeline.RequireAuthentication();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            ExceptionHandler.Handle(ex, TraceEventType.Error);
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(IsReportApiRequest() ? SessionStateBehavior.Required : SessionStateBehavior.ReadOnly);
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