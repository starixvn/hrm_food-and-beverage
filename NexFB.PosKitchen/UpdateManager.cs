namespace NexFB.PosKitchen
{
    using System.Deployment.Application;

    public class UpdateManager
    {
        private readonly static ApplicationDeployment _uppDep = ApplicationDeployment.CurrentDeployment;

        public UpdateManager()
        {
            _uppDep.CheckForUpdateCompleted += UpdateCheckCompleted;
        }

        public static void UpdateApp()
        {
            _uppDep.CheckForUpdateAsync();
        }

        private static void UpdateCheckCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                _uppDep.UpdateAsync();
            }
        }
    }
}