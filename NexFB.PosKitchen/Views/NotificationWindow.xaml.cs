namespace NexFB.PosKitchen.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for NotificationWindow.
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private static double _screenWidth = SystemParameters.PrimaryScreenWidth;
        private double _screeHeight = SystemParameters.PrimaryScreenHeight;

        public NotificationWindow()
        {
            InitializeComponent();
            DisplayPosition();
        }

        private void DisplayPosition()
        {
            this.Left = _screenWidth - (this.Width + 30);
            this.Top = _screeHeight - this.Height;
        }
    }
}
