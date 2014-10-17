// --------------------------------------------------------------------------
// <copyright file="NotificationWindow.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.23">Created</change>
// </history>

namespace NexFB.WinApp.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for NotificationWindow.
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;

        public NotificationWindow()
        {
            InitializeComponent();
            DisplayPosition();
        }

        private void DisplayPosition()
        {
            this.Left = _screenWidth - (this.Width + 30);
            this.Top = _screeHeight - this.Height;
        }
    }
}
