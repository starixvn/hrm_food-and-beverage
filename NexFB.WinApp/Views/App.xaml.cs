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
    using System.Windows;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Mapping;

    /// <summary>
    /// Interaction logic.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IMappingRegister mappingRegister = new CoreMappingRegister();
            mappingRegister.Register();
            WinFormMainResource.Culture = new System.Globalization.CultureInfo(string.Empty);
        }
    }
}
