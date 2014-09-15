// --------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.27">Created</change>
// </history>

namespace NexFB.WinApp
{
    using System.Windows;
    using Microsoft.AspNet.SignalR.Client.Hubs;
    using NexFB.WinApp.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();

            var hubConnection = new HubConnection("http://localhost:50442/");
            hubConnection.CreateHubProxy("chat");
            hubConnection.Start().Wait();
        }
    }
}
