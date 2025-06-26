namespace NexFB.Pos.Core.Models
{
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Order Preview.
    /// </summary>
    public class OrderPreview : PropertyChangedBase
    {
        #region Private Fields

        private int _orderNo;
        private int _quantity;
        private decimal _unitPrice;

        private string _nickName;
        private string _dishes;
        private string _price;
        private string _color;

        #endregion

        #region Public Properties

        public string Price
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

        public string Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                this.OnPropertyChanged("Color");
            }
        }

        public decimal UnitPrice
        {
            get
            {
                return _unitPrice;
            }

            set
            {
                _unitPrice = value;
                this.OnPropertyChanged("UnitPrice");
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

        public string Dishes
        {
            get
            {
                return _dishes;
            }

            set
            {
                _dishes = value;
                this.OnPropertyChanged("Dishes");
            }
        }

        public string NickName
        {
            get
            {
                return _nickName;
            }

            set
            {
                _nickName = value;
                this.OnPropertyChanged("NickName");
            }
        }

        public int OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
                this.OnPropertyChanged("OrderNo");
            }
        }

        #endregion
    }
}
