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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using NexFB.WinApp.Core.Models;
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

        /// <summary>
        /// Click on Result Text Block.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Mouse event.</param>
        private void ResultTextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ResultTextBlock.BorderBrush = Brushes.Red;
            OrderFoodViewModel currentViewModel = (OrderFoodViewModel)this.DataContext;
            currentViewModel.TextBlockFocused = true;
            ObservableCollection<CommonKitchenOrder> commonKitchenOrder = currentViewModel.CommonKitchenOrder;
            ObservableCollection<KitchenEmployeeInfo> userInGroup = currentViewModel.UserInGroup;
            if ((commonKitchenOrder.Any() && commonKitchenOrder.Count > 0) || (userInGroup.Any() && userInGroup.Count > 0))
            {
                if (commonKitchenOrder.Count > 0)
                {
                    commonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                }

                if (userInGroup.Count > 0)
                {
                    userInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                }
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OrderFoodViewModel currentViewModel = (OrderFoodViewModel)this.DataContext;
            StartTimerSetData(currentViewModel);
        }

        /// <summary>
        /// Timer tick to get data.
        /// </summary>
        /// <param name="sender">Object Sender.</param>
        /// <param name="currentViewModel">Current View Model.</param>
        private void TimerTickToGetData(object sender, OrderFoodViewModel currentViewModel)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            ObservableCollection<CommonKitchenOrder> commonKitchenOrder = currentViewModel.CommonKitchenOrder;
            ObservableCollection<KitchenEmployeeInfo> userInGroup = currentViewModel.UserInGroup;

            if (commonKitchenOrder.Any(x => x.IsFocusedMealMenu) || userInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                currentViewModel.TextBlockFocused = false;
                ResultTextBlock.BorderBrush = Brushes.White;
            }

            timer.Stop();
        }

        /// <summary>
        /// Timer get data.
        /// </summary>
        /// <param name="currentViewModel">Current view model.</param>
        private void StartTimerSetData(OrderFoodViewModel currentViewModel)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += (sender, e) => TimerTickToGetData(sender, currentViewModel);
            timer.Start();
        }
    }
}
