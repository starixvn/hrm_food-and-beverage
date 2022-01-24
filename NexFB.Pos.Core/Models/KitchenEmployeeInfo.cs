namespace NexFB.Pos.Core.Models
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using NexFB.Core.Enums;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Helpers;
    using NexFB.Pos.Core.NotifyBase;

    public class KitchenEmployeeInfo : PropertyChangedBase
    {
        #region Private Fields

        private static string _defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
        private static string _imageFolderPath = ConfigurationManager.AppSettings[Constant.ImageFolder];
        private int _sysId;
        private int _empId;
        private int _mealId;
        private int _menuId;
        private int _quantity;
        private int _locationId;
        private int _departmentId;
        private int _groupOrderId;

        private string _code;
        private string _fullName;
        private string _nickName;
        private string _menuEngName;
        private string _menuVnmName;
        private string _mealEngName;
        private string _imageSource;
        private string _mealVnmName;
        private string _orderSummary;
        private string _departmentName;
        private string _locationEngName;
        private string _locationVnmName;
        private string _employeeOrderMealText;
        private string _employeeOrderMealColor;

        private bool _isDeleteMainFood;
        private bool _disabledAutoAddFood;
        private bool _isVegetarian;
        private bool _isSelectedMemberInGroup;
        private bool _isPlaceOrder;
        private bool _isLocal;
        private bool _isChecked;

        private List<EmployeeOrderDetails> _listMainMenuOrderDeatails;
        private List<EmployeeOrderDetails> _listAlternativeMenuOrderDeatails;

        #endregion

        public KitchenEmployeeInfo()
        {
            ListMainMenuOrderDeatails = new List<EmployeeOrderDetails>();
            ListAlternativeMenuOrderDeatails = new List<EmployeeOrderDetails>();
        }

        #region Public Properties

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

        public string ImageSource
        {
            get
            {
                string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                string defaultImageSource = Path.Combine(baseDirectory, Constant.DefaultAvatar);
                switch (DepartmentId)
                {
                    case (int)DepartmentIdentity.Nexcel:
                        var imageNexcelTemplate = string.Format(Constant.NexcelAvatar, Code.Trim());
                        _imageSource = _imageFolderPath + imageNexcelTemplate;
                        break;
                    case (int)DepartmentIdentity.Luminet:
                        var imageLuminetTemplate = string.Format(Constant.LuminetAvatar, Code.Trim());
                        _imageSource = _imageFolderPath + imageLuminetTemplate;
                        break;
                    case (int)DepartmentIdentity.Sms:
                        var imageSmsTemplate = string.Format(Constant.SmsAvatar, Code.Trim());
                        _imageSource = _imageFolderPath + imageSmsTemplate;
                        break;
                    case (int)DepartmentIdentity.HongMA:
                        var imageHongMaTemplate = string.Format(Constant.HongMAAvatar, Code.Trim());
                        _imageSource = _imageFolderPath + imageHongMaTemplate;
                        break;
                    default:
                        _imageSource = defaultImageSource;
                        break;
                }

                return ResourceManagerHelper.GetResourceNames(_imageSource) ? _imageSource : defaultImageSource;
            }

            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
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

        public string NickNameHasOrdered
        {
            get
            {
                return Quantity > 0 ? NickName + " (" + Quantity + ") " : NickName;
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

        public string FullNameUpper => FullName.ToUpperInvariant();

        public string DeparmentNameDetails
            => IsLocal ? DepartmentName + " - " + Constant.Local : DepartmentName + " - " + Constant.Expat;

        public string LocationName
            => (string.IsNullOrEmpty(_defaultLanguage) || _defaultLanguage == Constant.DefaultCulture) ? LocationVnmName : LocationEngName;

        public string EmployeeSelectionGeneralInfo
            => FullName + " (" + NickName + ") " + "from " + DeparmentNameDetails;

        public string EmployeeOrderMealColor
        {
            get
            {
                if (IsPlaceOrder)
                {
                    _employeeOrderMealColor = IsVegetarian ? Constant.RegisterVegetarianMealColor : Constant.RegisterNonVegetarianMealColor;
                }
                else
                {
                    _employeeOrderMealColor = Quantity > 0 ? Constant.HasOrderMealBeforeColor : Constant.UNRegisterMealColor;
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

        public bool CanOrderMainFood
        {
            get
            {
                return !string.IsNullOrEmpty(MenuVnmName);
            }
        }

        public bool DisabledAutoAddFood
        {
            get
            {
                return _disabledAutoAddFood;
            }

            set
            {
                _disabledAutoAddFood = value;
                OnPropertyChanged("DisabledAutoAddFood");
            }
        }

        public bool IsDeleteMainFood
        {
            get
            {
                return _isDeleteMainFood;
            }

            set
            {
                _isDeleteMainFood = value;
                this.OnPropertyChanged("IsDeleteMainFood");
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

        #endregion
    }
}