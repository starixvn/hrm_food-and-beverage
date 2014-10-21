// --------------------------------------------------------------------------
// <copyright file="SideDishes.xaml.cs" company="Nexcel Solutions Vietnam">
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
    /// Interaction logic for SideDishes.
    /// </summary>
    public partial class SideDishes : UserControl
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;

        public SideDishes()
        {
            InitializeComponent();
            this.Height = _screeHeight;
            this.Width = _screenWidth;
        }
    }
}
