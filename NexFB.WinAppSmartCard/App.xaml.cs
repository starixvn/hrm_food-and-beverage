// --------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Threading;
    using Fanex.Data;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using NexFB.Pos.Core.InternetBased;
    using NexFB.Resources;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Mapping;
    using NexFB.WinAppSmartCard.Events;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Services;
    using NexFB.WinAppSmartCard.SmartCardHelper;
    using NexFB.WinAppSmartCard.ViewModels;

    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App : Application
    {
        public App()
            : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        /// <summary>
        /// Startup Method.
        /// </summary>
        /// <param name="e">Startup Event.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);

            if (processes.Length > 1)
            {
                MessageBox.Show("POS Employee Application Already Running");
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                Application_Run();
            }
        }

        /// <summary>
        /// Application Run.
        /// </summary>
        private void Application_Run()
        {
            if (DBConnectionAvailability.MainDBConnectionAvailable())
            {
                InitializeApp();
            }
            else
            {
                if (DBConnectionAvailability.LocalDBConnectionAvailable())
                {
                    ChangeConnectionToLocalDB();
                }
                else
                {
                    App_NetworkConnectionException();
                }
            }
        }

        private void OnCardNumberChange(CardNumberChangedEventArgs e, MainViewModel mainViewModel, MainWindow mainWindow)
        {
            mainViewModel.OnSmartCardNumberChanged(e.CardNumber, mainWindow);
        }

        /// <summary>
        /// Initialize app.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]        
        private void InitializeApp()
        {
            DbSettingProviderManager.Current.Start();
            IMappingRegister mappingRegister = new CoreMappingRegister();
            SmartcardManager manager = new SmartcardManager();
            MainWindow mainWindow = new MainWindow();
            MainViewModel mainViewModel = new MainViewModel();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();

            manager.CardNumberChanged += new EventHandler<CardNumberChangedEventArgs>((sender, ex) => OnCardNumberChange(ex, mainViewModel, mainWindow));
            ResourceManagerService.RegisterManager(@"MainScreenResources", MainScreenResources.ResourceManager, true);
            mappingRegister.Register();
        }

        /// <summary>
        /// Change connection From server to local when have problem to connect server DB.
        /// </summary>
        private void ChangeConnectionToLocalDB()
        {
            InitializeApp();
            string databaseLocalBackup = ConfigurationManager.AppSettings[ConstantValue.LocalDB];
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

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Application_Error(e);
            string errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.Exception.InnerException.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.UnhandledException, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }

        private void App_NetworkConnectionException()
        {
            if (MessageBox.Show(Lang.NetworkConnectionError, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }

        private void Application_Error(DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            string message = ex.Message +
                    "\r\nSource: " + ex.Source +
                     "\r\nDomain: POS Employee" +
                    "\r\nTargetSite: " + ex.TargetSite +
                    "\r\nStackTrace: " + ex.StackTrace;

            LogService.Error(message);
        }
    }
}
