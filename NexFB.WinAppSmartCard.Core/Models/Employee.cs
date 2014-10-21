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
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.NotifyBase;

    /// <summary>
    /// Class Employee.
    /// </summary>
    public class Employee : PropertyChangedBase
    {
        private int _orderNo;
        private int _sysId;
        private string _nickName;
        private int _menuId;
        private string _menuEngName;
        private string _menuVnmName;
        private int _currentCulture;
        private bool _isEditMenu;
        private string _foodInfoVisibility;
        private string _foodListVisibility;
        private List<Food> _listAlternativeFood;
        private int _selectedValueDropDown;

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

        public string DisplayMenuName
        {
            get
            {
                if (CurrentCulture == ConstantValue.VietNamCulture)
                {
                    if (!string.IsNullOrEmpty(MenuVnmName))
                    {
                        return MenuVnmName;
                    }

                    return ConstantValue.MainDishVietnamese;
                }
                else
                {
                    if (!string.IsNullOrEmpty(MenuEngName))
                    {
                        return MenuEngName;
                    }
                    else
                    {
                        if (CurrentCulture == 3)
                        {
                            return ConstantValue.MainDishChinese;
                        }

                        return ConstantValue.MainDishEnglish;
                    }
                }
            }
        }
    }
}
