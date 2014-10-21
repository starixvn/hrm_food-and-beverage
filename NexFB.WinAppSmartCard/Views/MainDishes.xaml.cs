// --------------------------------------------------------------------------
// <copyright file="MainDishes.xaml.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.17">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Views
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainDishes.
    /// </summary>
    public partial class MainDishes : UserControl
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;

        public MainDishes()
        {
            InitializeComponent();
            this.Height = _screeHeight;
            this.Width = _screenWidth;
        }
    }
}
