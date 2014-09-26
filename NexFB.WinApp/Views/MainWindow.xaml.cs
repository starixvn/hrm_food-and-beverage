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
    using System.Windows.Controls;
    using System.Windows.Input;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.DependencyProperties;
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

        /// <summary>
        /// Show hide keypad.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">MouseButton Event.</param>
        private void SortResult_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var currentViewModel = (MainViewModel)this.DataContext;
            if (currentViewModel.KeyPadVisibility == Constant.Visible)
            {
                currentViewModel.KeyPadVisibility = Constant.Hidden;
            }
            else
            {
                currentViewModel.KeyPadVisibility = Constant.Visible;
            }
        }

        /// <summary>
        /// Sort Result Changed.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Text Change event.</param>
        private void SortResult_TextChanged(object sender, TextChangedEventArgs e)
        {
            var itemSender = sender as TextBox;
            if (!string.IsNullOrEmpty(SortResult.Text))
            {
                Watermark.RemoveWatermark(itemSender);
            }
            else
            {
                Watermark.ShowWatermark(itemSender);
            }
        }

        private void Contentcontrol_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var currentViewModel = (MainViewModel)this.DataContext;
            if (currentViewModel.KeyPadVisibility == Constant.Visible)
            {
                currentViewModel.KeyPadVisibility = Constant.Hidden;
            }
        }
    }
}
