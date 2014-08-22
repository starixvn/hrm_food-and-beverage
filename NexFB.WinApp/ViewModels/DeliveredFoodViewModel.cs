// --------------------------------------------------------------------------
// <copyright file="DeliveredFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    /// <summary>
    /// Delivered Food ViewModel.
    /// </summary>
    public class DeliveredFoodViewModel : ViewModelBase
    {
        private bool _isDeliveredDefault;

        public bool IsDeliveredDefault
        {
            get
            {
                return _isDeliveredDefault;
            }

            set
            {
                if (_isDeliveredDefault == value)
                {
                    return;
                }

                _isDeliveredDefault = value;
                OnPropertyChanged("IsDeliveredDefault");
            }
        }
    }
}
