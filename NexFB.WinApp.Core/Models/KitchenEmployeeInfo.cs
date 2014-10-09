// --------------------------------------------------------------------------
// <copyright file="KitchenEmployeeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.1">Created</change>
// </history>

namespace NexFB.WinApp.Core.Models
{
    using System.Collections.Generic;
    using System.Configuration;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.NotifyBase;

    /// <summary>
    /// Kitchen Employee Info.
    /// </summary>
    public class KitchenEmployeeInfo : PropertyChangedBase
    {
        private static string _defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
        private int _groupOrderId;
        private int _mealId;
        private string _mealEngName;
        private string _mealVnmName;
        private bool _isSelectedMemberInGroup;
        private bool _isPlaceOrder;
        private bool _isLocal;
        private bool _isChecked;

        private int _sysId;
        private int _empId;
        private string _code;
        private string _fullName;
        private string _nickName;

        private int _menuId;
        private string _menuEngName;
        private string _menuVnmName;

        private int _departmentId;
        private string _departmentName;

        private int _locationId;
        private string _locationEngName;
        private string _locationVnmName;

        private bool _isVegetarian;
        private int _quantity;

        private string _employeeOrderMealColor;
        private string _employeeOrderMealText;
        private string _orderSummary;
        private List<EmployeeOrderDetails> _listMainMenuOrderDeatails;
        private List<EmployeeOrderDetails> _listAlternativeMenuOrderDeatails;

        public KitchenEmployeeInfo()
        {
            ListMainMenuOrderDeatails = new List<EmployeeOrderDetails>();
            ListAlternativeMenuOrderDeatails = new List<EmployeeOrderDetails>();
        }

        public List<EmployeeOrderDetails> ListMainMenuOrderDeatails
        {
            get
            {
                return _listMainMenuOrderDeatails;
            }

            set
            {
                _listMainMenuOrderDeatails = value;
                this.OnPropertyChanged("ListMainMenuOrderDeatails");
            }
        }

        public List<EmployeeOrderDetails> ListAlternativeMenuOrderDeatails
        {
            get
            {
                return _listAlternativeMenuOrderDeatails;
            }

            set
            {
                _listAlternativeMenuOrderDeatails = value;
                this.OnPropertyChanged("ListAlternativeMenuOrderDeatails");
            }
        }

        public bool IsPlaceOrder
        {
            get
            {
                return _isPlaceOrder;
            }

            set
            {
                _isPlaceOrder = value;
                this.OnPropertyChanged("IsPlaceOrder");
            }
        }

        public bool IsVegetarian
        {
            get
            {
                return _isVegetarian;
            }

            set
            {
                _isVegetarian = value;
                this.OnPropertyChanged("IsVegetarian");
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

        public int LocationId
        {
            get
            {
                return _locationId;
            }

            set
            {
                _locationId = value;
                this.OnPropertyChanged("LocationId");
            }
        }

        public string LocationEngName
        {
            get
            {
                return _locationEngName;
            }

            set
            {
                _locationEngName = value;
                this.OnPropertyChanged("LocationEngName");
            }
        }

        public string LocationVnmName
        {
            get
            {
                return _locationVnmName;
            }

            set
            {
                _locationVnmName = value;
                this.OnPropertyChanged("LocationVnmName");
            }
        }

        public bool IsSelectedMemberInGroup
        {
            get
            {
                return _isSelectedMemberInGroup;
            }

            set
            {
                _isSelectedMemberInGroup = value;
                this.OnPropertyChanged("IsSelectedMemberInGroup");
            }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            set
            {
                _isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }

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

        public int GroupOrderId
        {
            get 
            { 
                return _groupOrderId; 
            }

            set 
            {
                _groupOrderId = value;
                this.OnPropertyChanged("GroupOrderId");
            }
        }

        public int MealId
        {
            get
            {
                return _mealId;
            }

            set
            {
                _mealId = value;
                this.OnPropertyChanged("MealId");
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

        public int EmpId
        {
            get
            {
                return _empId;
            }

            set
            {
                _empId = value;
                this.OnPropertyChanged("EmpId");
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

        public string MealEngName
        {
            get
            {
                return _mealEngName;
            }

            set
            {
                _mealEngName = value;
                this.OnPropertyChanged("MealEngName");
            }
        }

        public string MealVnmName
        {
            get
            {
                return _mealVnmName;
            }

            set
            {
                _mealVnmName = value;
                this.OnPropertyChanged("MealVnmName");
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

        public string DepartmentName
        {
            get
            {
                return _departmentName;
            }

            set
            {
                _departmentName = value;
                this.OnPropertyChanged("DepartmentName");
            }
        }

        public int DepartmentId
        {
            get
            {
                return _departmentId;
            }

            set
            {
                _departmentId = value;
                this.OnPropertyChanged("DepartmentId");
            }
        }

        public string FullNameUpper
        {
            get
            {
                return FullName.ToUpper();
            }
        }

        public string DeparmentNameDetails
        {
            get
            {
                if (IsLocal)
                {
                    return DepartmentName + " - " + Constant.Local;
                }
                else
                {
                    return DepartmentName + " - " + Constant.Expat;
                }
            }
        }

        public string LocationName
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLanguage) || _defaultLanguage == Constant.DefaultCulture)
                {
                    return LocationVnmName;
                }
                else
                {
                    return LocationEngName;
                }
            }
        }

        public string EmployeeSelectionGeneralInfo
        {
            get
            {
                return FullName + " (" + NickName + ") " + "from " + DeparmentNameDetails; 
            }
        }

        public string EmployeeOrderMealColor
        {
            get
            {
                if (Quantity == 0)
                {
                    if (IsVegetarian)
                    {
                        if (IsPlaceOrder)
                        {
                            _employeeOrderMealColor = Constant.RegisterVegetarianMealColor;
                        }
                        else
                        {
                            _employeeOrderMealColor = Constant.UNRegisterMealColor;
                        }
                    }
                    else
                    {
                        if (IsPlaceOrder)
                        {
                            _employeeOrderMealColor = Constant.RegisterNonVegetarianMealColor;
                        }
                        else
                        {
                            _employeeOrderMealColor = Constant.UNRegisterMealColor;
                        }
                    }
                }
                else
                {
                    _employeeOrderMealColor = Constant.HasOrderMealBeforeColor;
                }

                return _employeeOrderMealColor;
            }

            set
            {
                _employeeOrderMealColor = value;
                this.OnPropertyChanged("EmployeeOrderMealColor");
            }
        }

        public string EmployeeOrderMealText
        {
            get
            {
                return _employeeOrderMealText;
            }

            set
            {
                _employeeOrderMealText = value;
                this.OnPropertyChanged("EmployeeOrderMealText");
            }
        }

        public string OrderSummary
        {
            get
            {
                return _orderSummary;
            }

            set
            {
                _orderSummary = value;
                this.OnPropertyChanged("OrderSummary");
            }
        }
    }
}
