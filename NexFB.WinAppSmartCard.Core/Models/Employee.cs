// --------------------------------------------------------------------------
// <copyright file="Employee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Models
{
    using System.Collections.Generic;
    using NexFB.Pos.Core.Enums;
    using NexFB.Pos.Core.NotifyBase;
    using NexFB.WinAppSmartCard.Core.Resources;

    /// <summary>
    /// Class Employee.
    /// </summary>
    public class Employee : PropertyChangedBase
    {
        #region Private fields

        private int _sysId;
        private int _menuId;
        private int _orderNo;
        private int _quantity;
        private int _currentCulture;
        private int _selectedValueDropDown;

        private string _fullName;
        private string _code;
        private string _nickName;
        private string _menuEngName;
        private string _menuVnmName;
        private string _displayMenuName;
        private string _foodInfoVisibility;
        private string _foodListVisibility;
        private string _groupOrderNickName;
        private string _displayNotifyMenuName;
        private string _displayGeneralOrderInfo;
        private string _displayTotalPayment;

        private bool _isEditMenu;
        private bool _allowOrder;
        private bool _isLocal;

        private List<Food> _listAlternativeFood;
        private List<Food> _listSideFood;
        private List<Food> _originalFood;

        #endregion

        #region Public Properties

        public bool IsLocal
        {
            get
            {
                return _isLocal;
            }

            set
            {
                _isLocal = value;
                this.OnPropertyChanged("IsLocal");
            }
        }

        public int SelectedValueDropDown
        {
            get
            {
                return _selectedValueDropDown;
            }

            set
            {
                _selectedValueDropDown = value;
                this.OnPropertyChanged("SelectedValueDropDown");
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

        public int SysId
        {
            get
            {
                return _sysId;
            }

            set
            {
                _sysId = value;
                this.OnPropertyChanged("SysId");
            }
        }

        public int MenuId
        {
            get
            {
                return _menuId;
            }

            set
            {
                _menuId = value;
                this.OnPropertyChanged("MenuId");
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }

            set
            {
                _fullName = value;
                this.OnPropertyChanged("FullName");
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
                this.OnPropertyChanged("Code");
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

        public string MenuEngName
        {
            get
            {
                return _menuEngName;
            }

            set
            {
                _menuEngName = value;
                this.OnPropertyChanged("MenuEngName");
            }
        }

        public string MenuVnmName
        {
            get
            {
                return _menuVnmName;
            }

            set
            {
                _menuVnmName = value;
                this.OnPropertyChanged("MenuVnmName");
            }
        }

        public string FoodInfoVisibility
        {
            get
            {
                return _foodInfoVisibility;
            }

            set
            {
                _foodInfoVisibility = value;
                this.OnPropertyChanged("FoodInfoVisibility");
            }
        }

        public string FoodListVisibility
        {
            get
            {
                return _foodListVisibility;
            }

            set
            {
                _foodListVisibility = value;
                this.OnPropertyChanged("FoodListVisibility");
            }
        }

        public string DisplayMenuName
        {
            get
            {
                if (CurrentCulture == (int)LanguageLocalization.VietNamCulture)
                {
                    _displayMenuName = string.IsNullOrEmpty(MenuVnmName) ? CommonResources.MainDishVietnamese : MenuVnmName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(MenuEngName))
                    {
                        _displayMenuName = MenuEngName;
                    }
                    else
                    {
                        if (CurrentCulture == (int)LanguageLocalization.ChinaCulture)
                        {
                            _displayMenuName = CommonResources.MainDishChinese;
                        }

                        _displayMenuName = CommonResources.MainDishEnglish;
                    }
                }

                return _displayMenuName;
            }

            set
            {
                _displayMenuName = value;
                this.OnPropertyChanged("DisplayMenuName");
            }
        }

        public string DisplayNotifyMenuName
        {
            get
            {
                return _displayNotifyMenuName;
            }

            set
            {
                _displayNotifyMenuName = value;
                this.OnPropertyChanged("DisplayNotifyMenuName");
            }
        }

        public string DisplayGeneralOrderInfo
        {
            get
            {
                return _displayGeneralOrderInfo;
            }

            set
            {
                _displayGeneralOrderInfo = value;
                this.OnPropertyChanged("DisplayGeneralOrderInfo");
            }
        }

        public string GroupOrderNickName
        {
            get
            {
                return _groupOrderNickName;
            }

            set
            {
                _groupOrderNickName = value;
                this.OnPropertyChanged("GroupOrderNickName");
            }
        }

        public string DisplayOrderNickname
        {
            get
            {
                if (string.IsNullOrEmpty(GroupOrderNickName))
                {
                    return NickName;
                }
                else
                {
                    return GroupOrderNickName;
                }
            }
        }

        public bool AllowOrder
        {
            get
            {
                return _allowOrder;
            }

            set
            {
                _allowOrder = value;
                this.OnPropertyChanged("AllowOrder");
            }
        }

        public bool IsEditMenu
        {
            get
            {
                return _isEditMenu;
            }

            set
            {
                _isEditMenu = value;
                this.OnPropertyChanged("IsEditMenu");
            }
        }

        public List<Food> ListAlternativeFood
        {
            get
            {
                return _listAlternativeFood;
            }

            set
            {
                _listAlternativeFood = value;
                this.OnPropertyChanged("istAlternativeFood");
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

        public List<Food> OriginalFood
        {
            get
            {
                return _originalFood;
            }

            set
            {
                _originalFood = value;
                this.OnPropertyChanged("OriginalFood");
            }
        }

        public string DisplayTotalPayment
        {
            get
            {
                if (ListSideFood == null)
                {
                    _displayTotalPayment = "0";
                }

                return _displayTotalPayment;
            }

            set
            {
                _displayTotalPayment = value;
                this.OnPropertyChanged("DisplayTotalPayment");
            }
        }

        #endregion
    }
}
