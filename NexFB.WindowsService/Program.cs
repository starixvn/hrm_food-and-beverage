namespace NexFB.WindowsService
{
    using System;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;

    public static class Program
    {
        public static void Main()
        {
            try
            {
                LogManager
                    .SetDefaultLogCategory(AppSettings.Sentry_Category)
                    .Use(new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(AppSettings.Sentry_Dsn),
                        WriteLogTimeoutSeconds = AppSettings.Sentry_Timeout
                    }));

                DbSettingProviderManager.Current.Start();
                log4net.Config.XmlConfigurator.Configure();

                var servicesToRun = new ServiceBase[]
                {
                    new AutoService()
                };
                ServiceBase.Run(servicesToRun);
            } catch(Exception exception)
            {
                var logMessage = new LogMessage(exception.Message)
                    .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                    .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                    .AddSection("Exception", CommonInfoGetters.GetExceptionInfo(exception))
                    .ToString();

                LogService.Error(logMessage);
            }
        }

        ////public static void Main()
        ////{
        ////    DbSettingProviderManager.Current.Start();
        ////    var service = new CommonService();

        ////    //service.UpdateDepartment();
        ////    //service.UpdateEmployeeAttendanceViolation();
        ////    //service.UpdateEmployeeSupervisor();
        ////    //service.UpdateGroupSupervisor();
        ////    //service.UpdateUser();
        ////    //service.UpdateShift();

        ////    //service.UpdateJobTitle();
        ////    //service.UpdateLeaveType();
        ////    //service.UpdateEmployeeCalendar();
        ////    //service.UpdateEmployeeRequestHistory();
        ////    //service.UpdateEmployeeRequest();
        ////    //service.UpdateEmployeeAttendance();
        ////    //service.UpdatePhoto();

        ////    service.UpdateEmployeeCalendarForNexcel();

        ////    Console.WriteLine("OK");
        ////    Console.ReadLine();
        ////}
    }
}