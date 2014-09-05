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
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Threading;
    using Fanex.Data.Configuration;
    using NexFB.Resources;
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
            if (InternetAvailability.IsInternetAvailable())
            {
                base.OnStartup(e);
                IMappingRegister mappingRegister = new CoreMappingRegister();
                mappingRegister.Register();
                WinFormMainResource.Culture = new System.Globalization.CultureInfo(string.Empty);
                DbSettingProviderManager.Current.Start();
            }
            else
            {
                DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = Lang.NetworkConnectionError + "\n\nError details:\n" + e.Exception.Message;
            e.Handled = true;
            if (MessageBox.Show(errorMessage, Lang.ErrorNetworkConnectionHeader, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                Shutdown();
            }
        }
    }
}
