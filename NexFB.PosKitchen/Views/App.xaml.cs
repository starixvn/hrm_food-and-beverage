// --------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.6">Created</change>
// </history>

namespace NexFB.PosKitchen
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
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

    /// <summary>
    /// Interaction logic.
    /// </summary>
    public partial class App : Application
    {
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += App_DispatcherUnhandledException;
        }

        /// <summary>
        /// Startup method.
        /// </summary>
        /// <param name="e">Startup events.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (DBConnectionAvailability.MainDBConnectionAvailable())
            {
                InitializeApp();
            }
            else
            {
                ChangeConnectionToLocalDB();
            }
        }

        /// <summary>
        /// Initialize App data.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]        
        private void InitializeApp()
        {
            string defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
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

            ////UpdateManager.UpdateApp();
        }

        /// <summary>
        /// Change connection From server to local when have problem to connect server DB.
        /// </summary>
        private void ChangeConnectionToLocalDB()
        {
            InitializeApp();
            string databaseLocalBackup = ConfigurationManager.AppSettings[Constant.LocalDB];
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
            ////Application_Error(e);
            string errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.Exception.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.UnhandledException, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void App_NetworkConnectionException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ////Application_Error(e);
            e.Handled = true;
            if (MessageBox.Show(Lang.NetworkConnectionError, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void Application_Error(DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string message = ex.Message +
                    "\r\nSource: " + ex.Source +
                     "\r\nDomain: POS Kitchen" +
                    "\r\nTargetSite: " + ex.TargetSite +
                    "\r\nStackTrace: " + ex.StackTrace;

            LogService.Error(message);
        }
    }
}
