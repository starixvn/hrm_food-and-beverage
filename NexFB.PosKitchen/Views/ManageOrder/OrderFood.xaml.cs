namespace NexFB.PosKitchen.Views.ManageOrder
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Models;
    using NexFB.PosKitchen.ViewModels;

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
            OrderFoodViewModel currentViewModel = (OrderFoodViewModel)this.DataContext;
            currentViewModel.MainTexboxResultBorder = (Border)sender;
            currentViewModel.ExecutePickGroupCommand();
            SetFocusedOnCurrentTextBlock(currentViewModel, 1);

            currentViewModel.MainTextBlockHighLight(true);
            currentViewModel.LocationTextBlockHighLight(false);
            currentViewModel.SearchLocationKeyboard = Constant.Hidden;
        }

        private void SearchLocation_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OrderFoodViewModel currentViewModel = (OrderFoodViewModel)this.DataContext;
            currentViewModel.LocationTexboxResultBorder = (Border)sender;

            SetFocusedOnCurrentTextBlock(currentViewModel, 2);
            currentViewModel.MainTextBlockHighLight(false);
            currentViewModel.LocationTextBlockHighLight(true);
            currentViewModel.ShowSearchLocationKeyBoard();
        }

        /// <summary>
        /// Set focus on specific textBlock.
        /// </summary>
        /// <param name="currentViewModel">Current ViewModel.</param>
        /// <param name="currentFocusedTextblock">Current focused textBlock.</param>
        private void SetFocusedOnCurrentTextBlock(OrderFoodViewModel currentViewModel, int currentFocusedTextblock)
        {
            ObservableCollection<CommonKitchenOrder> commonKitchenOrder = currentViewModel.CommonKitchenOrder;
            ObservableCollection<KitchenEmployeeInfo> userInGroup = currentViewModel.UserInGroup;
            if ((commonKitchenOrder.Any() && commonKitchenOrder.Count > 0) || (userInGroup.Any() && userInGroup.Count > 0))
            {
                if (commonKitchenOrder.Count > 0)
                {
                    commonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                    switch (currentFocusedTextblock)
                    {
                        case 1: //// TextBlock Employee code focused.
                            currentViewModel.CommonKitchenOrder = currentViewModel.AllObservationGroup;
                            break;
                        case 2: //// TextBlock Location focused.
                            if (string.IsNullOrEmpty(currentViewModel.LocationResult))
                            {
                                currentViewModel.CommonKitchenOrder = currentViewModel.AllObservationLocation;
                            }
                            else
                            {
                                currentViewModel.ExecuteSearchLoction();
                            }

                            break;
                    }
                }

                if (userInGroup.Count > 0)
                {
                    userInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                }
            }
        }
    }
}
