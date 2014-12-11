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
    using System.Globalization;
    using NexFB.Pos.Core.Enums;
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Food Model.
    /// </summary>
    public class Food : PropertyChangedBase
    {
        #region Private fields

        private int _foodId;
        private int _quantity;
        private int _currentItemNo;
        private int _currentCulture;

        private decimal _price;

        private string _foodVnmName;
        private string _foodEngName;
        private string _foodChineseName;
        private string _ownerName;

        #endregion

        #region Public Properties

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

        public int CurrentItemNo
        {
            get
            {
                return _currentItemNo;
            }

            set
            {
                _currentItemNo = value;
                this.OnPropertyChanged("CurrentItemNo");
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

        public string FoodChineseName
        {
            get
            {
                return _foodChineseName;
            }

            set
            {
                _foodChineseName = value;
                this.OnPropertyChanged("FoodChineseName");
            }
        }

        public string OwnerName
        {
            get
            {
                return _ownerName;
            }

            set
            {
                _ownerName = value;
                this.OnPropertyChanged("OwnerName");
            }
        }

        public string DisplayFoodName
        {
            get
            {
                var foodName = string.Empty;
                if (FoodId == int.MaxValue)
                {
                    switch (CurrentCulture)
                    {
                        case (int)LanguageLocalization.VietNamCulture:
                            foodName = FoodVnmName;
                            break;
                        case (int)LanguageLocalization.EnglishCulture:
                            foodName = FoodEngName;
                            break;
                        case (int)LanguageLocalization.ChinaCulture:
                            foodName = FoodChineseName;
                            break;
                    }
                }
                else
                {
                    foodName = CurrentCulture == (int)LanguageLocalization.VietNamCulture ? FoodVnmName : FoodEngName;
                }

                return foodName;
            }
        }

        public string DisplayPrice
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                return Price > 0 ? Price.ToString("#,###", culture.NumberFormat) : "0";
            }
        }

        #endregion
    }
}
