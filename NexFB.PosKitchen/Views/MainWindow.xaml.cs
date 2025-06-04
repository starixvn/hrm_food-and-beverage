namespace NexFB.PosKitchen
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interop;
    using NexFB.Pos.Core.Constants;
    using NexFB.PosKitchen.DependencyProperties;
    using NexFB.PosKitchen.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Screen screen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            this.DataContext = new MainViewModel();
            MainViewModel currentViewModel = (MainViewModel)this.DataContext;
            currentViewModel.ScreenWidth = screen.Bounds.Width;
            currentViewModel.ScreenHeight = screen.Bounds.Height;
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
            var itemSender = sender as System.Windows.Controls.TextBox;
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
