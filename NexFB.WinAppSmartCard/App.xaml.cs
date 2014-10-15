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
    using System.Windows;
    using NexFB.WinAppSmartCard.SmartCardHelper;

    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SmartcardManager.GetManager(1);
        }
    }
}
