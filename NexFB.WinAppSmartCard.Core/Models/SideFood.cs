// --------------------------------------------------------------------------
// <copyright file="SideFood.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.22">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NexFB.Pos.Core.NotifyBase;

    /// <summary>
    /// Side Food.
    /// </summary>
    public class SideFood : PropertyChangedBase
    {
        #region Private fields

        private int _orderNo;
        private int _quantity;
        private int _selectedValueDropDown;
        private decimal _originalPrice;
        private decimal _changedPrice;
        private string _selectedFoodName;

        private List<Food> _listSideFood;

        #endregion

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Will fix later.")]
        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Justification = "Will fix later.")]
        public delegate void OnDropDownValueChanged(object sender, EventArgs e);

        public event OnDropDownValueChanged DropDownChanged;

        #region Public Properties

        public int SelectedValueDropDown
        {
            get
            {
                return _selectedValueDropDown;
            }

            set
            {
                _selectedValueDropDown = value;
                if (value != int.MaxValue)
                {
                    if (value > 0)
                    {
                        Food currentSelectedItem = ListSideFood.Where(x => x.FoodId == value).FirstOrDefault();
                        OriginalPrice = currentSelectedItem.Price;
                        ChangedPrice = currentSelectedItem.Price;
                        this.Quantity = 1;
                        SelectedFoodName = currentSelectedItem.DisplayFoodName;
                    }
                }
                else
                {
                    this.Quantity = 0;
                    ChangedPrice = 0;
                }

                DropDownChanged(this.SelectedValueDropDown, new EventArgs());
                this.OnPropertyChanged("SelectedValueDropDown");
            }
        }

        public List<Food> ListSideFood
        {
            get
            {
                return _listSideFood;
            }

            set
            {
                _listSideFood = value;
                this.OnPropertyChanged("ListSideFood");
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

        public decimal OriginalPrice
        {
            get
            {
                return _originalPrice;
            }

            set
            {
                _originalPrice = value;
                this.OnPropertyChanged("Price");
            }
        }

        public decimal ChangedPrice
        {
            get
            {
                return _changedPrice;
            }

            set
            {
                _changedPrice = value;
                this.OnPropertyChanged("ChangedPrice");
            }
        }

        public string SelectedFoodName
        {
            get
            {
                return _selectedFoodName;
            }

            set
            {
                _selectedFoodName = value;
                this.OnPropertyChanged("SelectedFoodName");
            }
        }

        #endregion
    }
}
