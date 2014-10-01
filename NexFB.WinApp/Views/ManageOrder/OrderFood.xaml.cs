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
    using System.Windows.Controls;
    using System.Windows.Input;
    using NexFB.WinApp.ViewModels;

    /// <summary>
    /// Interaction logic for OrderFood.
    /// </summary>
    public partial class OrderFood : UserControl
    {
        public OrderFood()
        {
            InitializeComponent();
        }

        ////private void GridActionButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        ////{
        ////    OrderFoodViewModel currentViewModel = (OrderFoodViewModel)this.DataContext;
        ////    currentViewModel.ChangeButtonBorder(0);
        ////}
    }
}
