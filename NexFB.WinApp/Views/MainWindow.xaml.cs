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
    using NexFB.WinApp.Core.Constants;
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
        }

        private void GridKeyPad_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var abc = (MainViewModel)this.DataContext;
            abc.KeyPadVisibility = Constant.Hidden;
            e.Handled = true;
        }
    }
}
