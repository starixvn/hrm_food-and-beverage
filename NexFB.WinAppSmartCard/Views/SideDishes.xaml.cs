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
