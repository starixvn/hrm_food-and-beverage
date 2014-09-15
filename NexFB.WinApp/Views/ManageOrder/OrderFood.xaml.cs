// --------------------------------------------------------------------------
// <copyright file="OrderFood.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.29">Created</change>
// </history>

namespace NexFB.WinApp.Views.ManageOrder
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Microsoft.AspNet.SignalR.Client.Hubs;

    /// <summary>
    /// Interaction logic for OrderFood.
    /// </summary>
    public partial class OrderFood : UserControl
    {
        private string _host = "http://localhost:50442/";

        public OrderFood()
        {
            InitializeComponent();
        }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public IHubProxy Proxy { get; set; }

        public Thread Thread { get; set; }

        private void ActionSendButtonClick(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private Task SendMessage()
        {
            return Proxy.Invoke("SendMessage", ClientNameTextBox.Text);
        }

        /// <summary>
        /// On Window Loaded.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Routed event.</param>
        private void ActionWindowLoaded(object sender, RoutedEventArgs e)
        {
            Active = true;
            Thread = new System.Threading.Thread(() =>
            {
                Connection = new HubConnection(_host);
                Proxy = Connection.CreateHubProxy("chat");
                Proxy.On<string>("newMessage", (name) => OnSendData("Recieved addMessage: " + name));
                Connection.Start();

                while (Active)
                {
                    System.Threading.Thread.Sleep(10);
                }
            }) { IsBackground = true };
            Thread.Start();
        }

        private void OnSendData(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => MessagesListBox.Items.Insert(0, message)));
        }
    }
}
