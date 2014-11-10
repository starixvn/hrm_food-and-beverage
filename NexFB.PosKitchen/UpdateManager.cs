// --------------------------------------------------------------------------
// <copyright file="UpdateManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.11.7">Created</change>
// </history>

namespace NexFB.PosKitchen
{
    using System.Deployment.Application;

    /// <summary>
    /// Update Manager.
    /// </summary>
    public class UpdateManager
    {
        private static ApplicationDeployment _uppDep;

        public UpdateManager()
        {
            _uppDep = ApplicationDeployment.CurrentDeployment;
            _uppDep.CheckForUpdateCompleted += UpdateCheckCompleted;
        }

        public static void UpdateApp()
        {
            _uppDep.CheckForUpdateAsync();
        }

        private void UpdateCheckCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                _uppDep.UpdateAsync();
            }
        }
    }
}