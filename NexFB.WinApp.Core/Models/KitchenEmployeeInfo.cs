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
        private bool _isLocal;

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
    }
}
