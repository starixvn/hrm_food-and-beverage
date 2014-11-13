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
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using Fanex.Data;
    using Fanex.Data.Configuration;
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
        /// <summary>
        /// Startup Method.
        /// </summary>
        /// <param name="e">Startup Event.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
            mainViewModel.OnSmartCardNumberchanged(e.CardNumber, mainWindow);
        }

        /// <summary>
        /// Initialize app.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]        
        private void InitializeApp()
        {
            try
            {
                IMappingRegister mappingRegister = new CoreMappingRegister();
                SmartcardManager manager = new SmartcardManager();
                MainWindow mainWindow = new MainWindow();
                MainViewModel mainViewModel = new MainViewModel();
                mainWindow.DataContext = mainViewModel;
                mainWindow.Show();

                manager.CardNumberChanged += new EventHandler<CardNumberChangedEventArgs>((sender, ex) => OnCardNumberChange(ex, mainViewModel, mainWindow));
                ResourceManagerService.RegisterManager(@"MainScreenResources", MainScreenResources.ResourceManager, true);
                DbSettingProviderManager.Current.Start();
                mappingRegister.Register();
            }
            catch (Exception e)
            {
                App_DispatcherUnhandledException(e);
            }
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

        private void App_DispatcherUnhandledException(Exception e)
        {
            string errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.InnerException;
            if (MessageBox.Show(errorMessage, Lang.UnhandledException, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }

        private void App_NetworkConnectionException()
        {
            if (MessageBox.Show(Lang.NetworkConnectionError, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }
    }
}
