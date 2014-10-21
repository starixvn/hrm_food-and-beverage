// --------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.14">Created</change>
// </history>

namespace NexFB.WinAppSmartCard
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = _screeHeight;
            this.Width = _screenWidth;
        }

        private void LanguageButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var contextMenu = languageButton.ContextMenu;
            contextMenu.PlacementTarget = this;
            contextMenu.IsOpen = true;
        }
    }
}
