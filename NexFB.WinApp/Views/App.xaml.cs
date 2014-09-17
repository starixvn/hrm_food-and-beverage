// --------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.27">Created</change>
// </history>

namespace NexFB.WinApp
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Threading;
    using Fanex.Data.Configuration;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Mapping;
    using NexFB.WinApp.DependencyProperties;

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
                if (InternetAvailability.IsInternetAvailable())
                {
                    base.OnStartup(e);
                    string defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
                    ////string host = ConfigurationManager.AppSettings[Constant.HubHost];
                    IMappingRegister mappingRegister = new CoreMappingRegister();
                    if (string.IsNullOrEmpty(defaultLanguage))
                    {
                        WinFormMainResource.Culture = new CultureInfo(Constant.DefaultCulture);
                    }
                    else
                    {
                        WinFormMainResource.Culture = new CultureInfo(defaultLanguage);
                    }

                    mappingRegister.Register();
                    DbSettingProviderManager.Current.Start();
                    ////var hubConnection = new HubConnection(host);
                    ////hubConnection.CreateHubProxy("order");
                    ////hubConnection.Start().Wait();
                }
                else
                {
                    DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_NetworkConnectionException);
                }
            }
            catch
            {
                DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = Lang.ErrorMessage + "\n\nError details:\n" + e.Exception.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
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
    }
}
