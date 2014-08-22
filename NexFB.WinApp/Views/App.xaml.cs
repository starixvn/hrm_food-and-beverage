using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using NexFB.WinApp.Core.Mapping;

namespace NexFB.WinApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IMappingRegister mappingRegister = new CoreMappingRegister();
            mappingRegister.Register();
        }
    }
}
