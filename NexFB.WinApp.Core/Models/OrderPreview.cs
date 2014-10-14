// --------------------------------------------------------------------------
// <copyright file="OrderPreview.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.10">Created</change>
// </history>

namespace NexFB.WinApp.Core.Models
{
    using NexFB.WinApp.Core.NotifyBase;

    /// <summary>
    /// Order Preview.
    /// </summary>
    public class OrderPreview : PropertyChangedBase
    {
        private int _orderNo;
        private string _nickName;
        private string _dishes;
        private int _quantity;
        private decimal _unitPrice;
        private string _price;

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
    }
}
