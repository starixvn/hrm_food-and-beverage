// --------------------------------------------------------------------------
// <copyright file="SettingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.25">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Configuration;
    using System.IO;
    using System.Windows.Input;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Views;

    /// <summary>
    /// Setting ViewModel.
    /// </summary>
    public class SettingViewModel : ViewModelBase
    {
        private bool _devMode;
        private bool _releaseMode;

        public SettingViewModel()
        {
            string releaseMode = ConfigurationManager.AppSettings[Constant.ReleaseMode];
            if (!string.IsNullOrEmpty(releaseMode))
            {
                if (releaseMode == Constant.DevMode)
                {
                    DevMode = true;
                    ReleaseMode = false;
                }
                else
                {
                    DevMode = false;
                    ReleaseMode = true;
                }
            }

            SaveData = new RelayCommand((x) => CanExecuteSaveData(), (y) => ExecuteSaveData(y));
        }

        public ICommand SaveData { get; set; }

        public bool DevMode
        {
            get
            {
                return _devMode;
            }

            set
            {
                _devMode = value;
                this.OnPropertyChanged("DevMode");
            }
        }

        public bool ReleaseMode
        {
            get
            {
                return _releaseMode;
            }

            set
            {
                _releaseMode = value;
                this.OnPropertyChanged("ReleaseMode");
            }
        }

        private bool CanExecuteSaveData()
        {
            return true;
        }

        /// <summary>
        /// Execute Save Data.
        /// </summary>
        /// <param name="data">Object parameter.</param>
        private void ExecuteSaveData(object data)
        {
            SettingWindow settingWindow = null;
            if (data != null)
            {
                settingWindow = (SettingWindow)data;
            }

            string printerMode = string.Empty;
            if (DevMode)
            {
                printerMode = "DEV";
            }
            else
            {
                printerMode = "RELEASE";
            }

            string absolutePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Constant.ConfigFilePath);
            ////string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            ////string configFile = Path.Combine(appPath, "NexFB.WinApp.exe.config");

            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = absolutePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            config.AppSettings.Settings["ReleaseMode"].Value = printerMode;
            config.Save();

            settingWindow.Close();
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
