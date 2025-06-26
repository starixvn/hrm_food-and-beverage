namespace NexFB.WindowsService
{
    using System.ComponentModel;

    /// <summary>
    /// Project Installer.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
