namespace NexFB.Pos.Core.Models
{
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Common Kitchen Order.
    /// </summary>
    public class CommonKitchenOrder : PropertyChangedBase
    {
        #region Private fields

        private int _itemId;
        private int _quantity;
        private decimal _price;

        private string _itemName;
        private string _menuColor;

        private bool _isSelectedMealMenu;
        private bool _isFocusedMealMenu;
        private bool _isMainMenu;

        #endregion

        #region Public Properties

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

        #endregion
    }
}
