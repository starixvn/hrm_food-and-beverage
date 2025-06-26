namespace NexFB.PosKitchen
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Threading;
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.InternetBased;
    using NexFB.Pos.Core.Mapping;
    using NexFB.Resources;

    public partial class App : Application
    {
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += App_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var allowMultipleInstance = ConfigurationManager.AppSettings[Constant.AllowMultipleInstance];
            if (string.IsNullOrEmpty(allowMultipleInstance))
            {
                allowMultipleInstance = "FALSE";
            }

            if (allowMultipleInstance.ToUpper() == "FALSE")
            {
                var procName = Process.GetCurrentProcess().ProcessName;
                var processes = Process.GetProcessesByName(procName);

                if (processes.Length > 1)
                {
                    MessageBox.Show("POS Kitchen Application Already Running");
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    Application_Run();
                }
            }
            else
            {
                Application_Run();
            }
        }

        private void Application_Run()
        {
            if (DBConnectionAvailability.MainDBConnectionAvailable())
            {
                InitializeApp();
            }
            else
            {
                ChangeConnectionToLocalDB();
            }
        }

        private void InitializeApp()
        {
            var defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
            IMappingRegister mappingRegister = new CoreMappingRegister();
            mappingRegister.Register();

            if (string.IsNullOrEmpty(defaultLanguage))
            {
                WinFormMainResource.Culture = new CultureInfo(Constant.DefaultCulture);
            }
            else
            {
                WinFormMainResource.Culture = new CultureInfo(defaultLanguage);
            }

            DbSettingProviderManager.Current.Start();
        }

        private void ChangeConnectionToLocalDB()
        {
            InitializeApp();
            var databaseLocalBackup = ConfigurationManager.AppSettings[Constant.LocalDB];
            if (string.IsNullOrEmpty(databaseLocalBackup))
            {
                DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_NetworkConnectionException);
            }
            else
            {
                var conStr = ConnectionStringGetter.GetConnectionString(databaseLocalBackup);
                if (!string.IsNullOrEmpty(conStr))
                {
                    var settingDataSource = DbSettingProviderManager.Current.SettingDataSource;
                    if (settingDataSource != null)
                    {
                        foreach (var item in settingDataSource)
                        {
                            item.Value.ConnectionString = conStr;
                            item.Value.DbName = databaseLocalBackup;
                        }
                    }
                }
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Application_Error(e);
            var errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.Exception.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.UnhandledException, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void App_NetworkConnectionException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Application_Error(e);
            e.Handled = true;
            if (MessageBox.Show(Lang.NetworkConnectionError, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void Application_Error(DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;
            var message = ex.Message +
                    "\r\nSource: " + ex.Source +
                     "\r\nDomain: POS Kitchen" +
                    "\r\nTargetSite: " + ex.TargetSite +
                    "\r\nStackTrace: " + ex.StackTrace;

            LogService.Error(message);
        }
    }
}