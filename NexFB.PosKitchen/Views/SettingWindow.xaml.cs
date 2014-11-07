// --------------------------------------------------------------------------
// <copyright file="SettingWindow.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.25">Created</change>
// </history>

namespace NexFB.PosKitchen.Views
{
    using System.Windows;
    using NexFB.PosKitchen.ViewModels;

    /// <summary>
    /// Interaction logic for SettingWindow.
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            this.DataContext = new SettingViewModel();
        }
    }
}
