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
