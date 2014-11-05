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
    using System.Windows;
    using Fanex.Data.Configuration;
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
            InitializeApp();
        }

        private void OnCardNumberChange(CardNumberChangedEventArgs e, MainViewModel mainViewModel, MainWindow mainWindow)
        {
            mainViewModel.OnSmartCardNumberchanged(e.CardNumber, mainWindow);
        }

        /// <summary>
        /// Initialize app.
        /// </summary>
        private void InitializeApp()
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
    }
}
