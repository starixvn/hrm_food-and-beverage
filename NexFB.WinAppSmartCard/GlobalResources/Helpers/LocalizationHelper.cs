namespace NexFB.WinAppSmartCard.GlobalResources.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using NexFB.WinAppSmartCard.GlobalResources.Services;

    /// <summary>
    /// Localization Helper.
    /// </summary>
    public class LocalizationHelper : INotifyPropertyChanged
    {
        public LocalizationHelper()
        {
            if (!DesignHelpers.IsInDesignModeStatic)
            {
                // Refresh all the bindings when the locale changes
                ResourceManagerService.LocaleChanged += (s, e) =>
                {
                    RaisePropertyChanged(string.Empty);
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a resource string from the ResourceManager.
        /// You can bind to this property using the .[KEY].
        /// {Binding Source={StaticResource Localization}, Path=.[MainScreenResources.IntroTextLine1]}.
        /// </summary>
        /// <param name="key">Key to retrieve in the format [ManagerName].[ResourceKey].</param>
        /// <returns>String key.</returns>
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Reviewed. Suppression is OK here.")]
        public string this[string key]
        {
            get
            {
                if (!ValidKey(key))
                {
                    throw new ArgumentException(@"Key is not in the valid [ManagerName].[ResourceKey] format");
                }

                if (DesignHelpers.IsInDesignModeStatic)
                {
                    throw new Exception("Design mode is not supported");
                }

                return ResourceManagerService.GetResourceString(GetManagerKey(key), GetResourceKey(key));
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Reviewed. Suppression is OK here.")]
        protected void RaisePropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;

            if (evt != null)
            {
                evt.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Private Key Methods
        private bool ValidKey(string input)
        {
            return input.Contains(".");
        }

        private string GetManagerKey(string input)
        {
            return input.Split('.')[0];
        }

        private string GetResourceKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }
        #endregion
    }
}
