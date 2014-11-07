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
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Threading;
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.InternetBased;
    using NexFB.Pos.Core.Mapping;
    using NexFB.Resources;

    /// <summary>
    /// Interaction logic.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Startup method.
        /// </summary>
        /// <param name="e">Startup events.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (DBConnectionAvailability.IsDBConnectionAvailable())
                {
                    base.OnStartup(e);
                    InitializeApp();
                }
                else
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
            }
            catch
            {
                DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.Exception.InnerException.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.UnhandledException, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }

        private void App_NetworkConnectionException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (MessageBox.Show(Lang.NetworkConnectionError, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }

        /// <summary>
        /// Initialize App data.
        /// </summary>
        private void InitializeApp()
        {
            string defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
            IMappingRegister mappingRegister = new CoreMappingRegister();
            mappingRegister.Register();
            DbSettingProviderManager.Current.Start();
            if (string.IsNullOrEmpty(defaultLanguage))
            {
                WinFormMainResource.Culture = new CultureInfo(Constant.DefaultCulture);
            }
            else
            {
                WinFormMainResource.Culture = new CultureInfo(defaultLanguage);
            }
        }
    }
}
