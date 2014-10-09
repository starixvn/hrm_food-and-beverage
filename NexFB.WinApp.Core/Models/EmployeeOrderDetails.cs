// --------------------------------------------------------------------------
// <copyright file="EmployeeOrderDetails.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.7">Created</change>
// </history>

namespace NexFB.WinApp.Core.Models
{
    using NexFB.WinApp.Core.NotifyBase;

    /// <summary>
    /// Employee Order Details.
    /// </summary>
    public class EmployeeOrderDetails : PropertyChangedBase
    {
        private int _selectedMenuItemId;
        private int _selectedMenuItemQuantity;
        private string _selectedMenuItemName;

        public int SelectedMenuItemId
        {
            get
            {
                return _selectedMenuItemId;
            }

            set
            {
                _selectedMenuItemId = value;
                this.OnPropertyChanged("SelectedMenuItemId");
            }
        }

        public int SelectedMenuItemQuantity
        {
            get
            {
                return _selectedMenuItemQuantity;
            }

            set
            {
                _selectedMenuItemQuantity = value;
                this.OnPropertyChanged("SelectedMenuItemQuantity");
            }
        }

        public string SelectedMenuItemName
        {
            get
            {
                return _selectedMenuItemName;
            }

            set
            {
                _selectedMenuItemName = value;
                this.OnPropertyChanged("SelectedMenuItemName");
            }
        }
    }
}
