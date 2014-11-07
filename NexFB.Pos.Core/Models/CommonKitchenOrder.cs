// --------------------------------------------------------------------------
// <copyright file="CommonKitchenOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.30">Created</change>
// </history>

namespace NexFB.Pos.Core.Models
{
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Common Kitchen Order.
    /// </summary>
    public class CommonKitchenOrder : PropertyChangedBase
    {
        private int _itemId;
        private string _itemName;
        private bool _isSelectedMealMenu;
        private bool _isFocusedMealMenu;
        private string _menuColor;
        private bool _isMainMenu;
        private int _quantity;
        private decimal _price;

        public decimal Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
                this.OnPropertyChanged("Price");
            }
        }

        public bool IsSelectedMealMenu
        {
            get
            {
                return _isSelectedMealMenu;
            }

            set
            {
                _isSelectedMealMenu = value;
                this.OnPropertyChanged("IsSelectedMealMenu");
            }
        }

        public int Quantity
        {
            get
            {
                return _quantity;
            }

            set
            {
                _quantity = value;
                this.OnPropertyChanged("Quantity");
            }
        }

        public bool IsFocusedMealMenu
        {
            get
            {
                return _isFocusedMealMenu;
            }

            set
            {
                _isFocusedMealMenu = value;
                this.OnPropertyChanged("IsFocusedMealMenu");
            }
        }

        public int ItemId
        {
            get
            {
                return _itemId;
            }

            set
            {
                _itemId = value;
                this.OnPropertyChanged("ItemId");
            }
        }

        public string ItemName
        {
            get
            {
                return _itemName;
            }

            set
            {
                _itemName = value;
                this.OnPropertyChanged("ItemName");
            }
        }

        public string MenuColor
        {
            get
            {
                return _menuColor;
            }

            set
            {
                _menuColor = value;
                this.OnPropertyChanged("MenuColor");
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
    }
}
