// --------------------------------------------------------------------------
// <copyright file="Food.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.21">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Models
{
    using NexFB.WinApp.Core.NotifyBase;
    using NexFB.WinAppSmartCard.Core.Constants;

    /// <summary>
    /// Food Model.
    /// </summary>
    public class Food : PropertyChangedBase
    {
        private int _foodId;
        private string _foodVnmName;
        private string _foodEngName;
        private int _quantity;
        private decimal _price;
        private int _currentCulture;

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

        public int FoodId
        {
            get
            {
                return _foodId;
            }

            set
            {
                _foodId = value;
                this.OnPropertyChanged("FoodId");
            }
        }

        public string FoodVnmName
        {
            get
            {
                return _foodVnmName;
            }

            set
            {
                _foodVnmName = value;
                this.OnPropertyChanged("FoodVnmName");
            }
        }

        public string FoodEngName
        {
            get
            {
                return _foodEngName;
            }

            set
            {
                _foodEngName = value;
                this.OnPropertyChanged("FoodEngName");
            }
        }

        public int CurrentCulture
        {
            get
            {
                return _currentCulture;
            }

            set
            {
                _currentCulture = value;
                this.OnPropertyChanged("CurrentCulture");
            }
        }

        public string DisplayFoodName
        {
            get
            {
                if (CurrentCulture == ConstantValue.VietNamCulture)
                {
                    return FoodVnmName;
                }
                else
                {
                    return FoodEngName;
                }
            }
        }
    }
}
