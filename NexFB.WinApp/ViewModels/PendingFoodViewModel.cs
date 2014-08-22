// --------------------------------------------------------------------------
// <copyright file="PendingFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class PendingFoodViewModel : ViewModelBase
    {
        private bool _isPendingDefault;

        public bool IsPendingDefault
        {
            get
            {
                return _isPendingDefault;
            }

            set
            {
                if (_isPendingDefault == value)
                {
                    return;
                }

                _isPendingDefault = value;
                OnPropertyChanged("IsPendingDefault");
            }
        }
    }
}
