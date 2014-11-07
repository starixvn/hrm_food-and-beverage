// --------------------------------------------------------------------------
// <copyright file="EmployeeOrderDetails.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.7">Created</change>
// </history>

namespace NexFB.Pos.Core.Models
{
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Employee Order Details.
    /// </summary>
    public class EmployeeOrderDetails : PropertyChangedBase
    {
        private int _selectedMenuItemId;
        private int _selectedMenuItemQuantity;
        private string _selectedMenuItemName;
        private decimal _selectedMenuItemPrice;
        private bool _isMainMenu;

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

        public bool IsMainMenu
        {
            get
            {
                return _isMainMenu;
            }

            set
            {
                _isMainMenu = value;
                this.OnPropertyChanged("IsMainMenu");
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

        public decimal SelectedMenuItemPrice
        {
            get
            {
                return _selectedMenuItemPrice;
            }

            set
            {
                _selectedMenuItemPrice = value;
                this.OnPropertyChanged("SelectedMenuItemPrice");
            }
        }
    }
}
