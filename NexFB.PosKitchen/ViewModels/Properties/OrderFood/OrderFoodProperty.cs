// --------------------------------------------------------------------------
// <copyright file="OrderFoodProperty.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.27">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels.Properties.OrderFood
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Models;

    /// <summary>
    /// Properties of OrderFoodViewModel.
    /// </summary>
    public class OrderFoodProperty : OrderFoodDynamicScreen
    {
        private static ObservableCollection<CommonKitchenOrder> _commonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableLocation = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableAlternativeFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableGroup = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<KitchenEmployeeInfo> _userInGroup = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<KitchenEmployeeInfo> _duplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<OrderPreview> _orderPreviewData = new ObservableCollection<OrderPreview>();
        private static KitchenEmployeeInfo _employeeInfo;
        private MainViewModel _mainViewModel;

        private string _result;
        private string _borderButtonPickRoom;
        private string _borderButtonMainFood;
        private string _borderButtonAlternativeFood;
        private string _borderButtonPickGroup;
        private string _backgroundButtonPickRoom;
        private string _backgroundButtonMainFood;
        private string _backgroundButtonAlternativeFood;
        private string _backgroundButtonPickGroup;
        private string _gridUserVisibility;
        private string _messageHeader;
        private bool _isMenuFocused;
        private int _currentSelectedEmployeeSysId;
        private int _currentSelectedMainMenuId;
        private int _currentSelectedAlternativeMenuId;
        private int _currentSelectedGroupId;
        private string _locationName;
        private int _locationId;
        private bool _isVnmCulture;
        private int _totalOrder;
        private string _totalPrice;
        private string _orderNo;
        private int _referenceCode;
        private bool _textBlockFocused;
        private string _overlayVisible;
        private bool _isDataEdit;
        private string _saveButtonVisible;
        private string _ownerImage;
        private bool _isCanceledOrder;

        public ICommand NumPadPressed { get; set; }

        public ICommand NumPadReleased { get; set; }

        public ICommand PickGroupCommand { get; set; }

        public ICommand PickRoomCommand { get; set; }

        public ICommand ChooseMainFoodCommand { get; set; }

        public ICommand ChooseAlternativeFoodCommand { get; set; }

        public ICommand DefaulMenuSelected { get; set; }

        public ICommand DefaultEmployeeSelected { get; set; }

        public ICommand ReceiveCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand CancelDialogCommand { get; set; }

        public ICommand SaveDialogCommand { get; set; }

        public ICommand SelectAllUserInGroup { get; set; }

        public ICommand CancelDialogOrderConfirmation { get; set; }

        public ICommand SaveOrderCommand { get; set; }

        public ICommand ChangeKeyPad { get; set; }

        public ICommand CloseVirtualKeyboard { get; set; }

        public bool IsCanceledOrder
        {
            get
            {
                return _isCanceledOrder;
            }

            set
            {
                _isCanceledOrder = value;
                this.OnPropertyChanged("IsCanceledOrder");
            }
        }

        public int TotalOrder
        {
            get
            {
                return _totalOrder;
            }

            set
            {
                _totalOrder = value;
                OnPropertyChanged("TotalOrder");
            }
        }

        public string OwnerImage
        {
            get
            {
                return _ownerImage;
            }

            set
            {
                _ownerImage = value;
                OnPropertyChanged("OwnerImage");
            }
        }

        public string SaveButtonVisible
        {
            get
            {
                return _saveButtonVisible;
            }

            set
            {
                _saveButtonVisible = value;
                OnPropertyChanged("SaveButtonVisible");
            }
        }

        public string OrderNo
        {
            get
            {
                return _orderNo;
            }

            set
            {
                _orderNo = value;
                OnPropertyChanged("OrderNo");
            }
        }

        public int ReferenceCode
        {
            get
            {
                return _referenceCode;
            }

            set
            {
                _referenceCode = value;
                OnPropertyChanged("ReferenceCode");
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
                OnPropertyChanged("LocationId");
            }
        }

        public string TotalPrice
        {
            get
            {
                return _totalPrice;
            }

            set
            {
                _totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        public bool IsMenuFocused
        {
            get
            {
                return _isMenuFocused;
            }

            set
            {
                _isMenuFocused = value;
                OnPropertyChanged("IsMenuFocused");
            }
        }

        public string LocationName
        {
            get
            {
                return _locationName;
            }

            set
            {
                _locationName = value;
                OnPropertyChanged("LocationName");
            }
        }

        public bool IsVnmCulture
        {
            get
            {
                return _isVnmCulture;
            }

            set
            {
                _isVnmCulture = value;
                this.OnPropertyChanged("IsVnmCulture");
            }
        }

        public string MessageHeader
        {
            get
            {
                return _messageHeader;
            }

            set
            {
                _messageHeader = value;
                OnPropertyChanged("MessageHeader");
            }
        }

        public int CurrentSelectedEmployeeSysId
        {
            get
            {
                return _currentSelectedEmployeeSysId;
            }

            set
            {
                _currentSelectedEmployeeSysId = value;
                OnPropertyChanged("CurrentSelectedEmployeeSysId");
            }
        }

        public int CurrentSelectedMainMenuId
        {
            get
            {
                return _currentSelectedMainMenuId;
            }

            set
            {
                _currentSelectedMainMenuId = value;
                OnPropertyChanged("CurrentSelectedMainMenuId");
            }
        }

        public int CurrentSelectedAlternativeMenuId
        {
            get
            {
                return _currentSelectedAlternativeMenuId;
            }

            set
            {
                _currentSelectedAlternativeMenuId = value;
                OnPropertyChanged("CurrentSelectedAlternativeMenuId");
            }
        }

        public string Result
        {
            get
            {
                return _result;
            }

            set
            {
                _result = value;
                this.OnPropertyChanged("Result");
            }
        }

        public string BorderButtonPickRoom
        {
            get
            {
                return _borderButtonPickRoom;
            }

            set
            {
                _borderButtonPickRoom = value;
                this.OnPropertyChanged("BorderButtonPickRoom");
            }
        }

        public string BorderButtonMainFood
        {
            get
            {
                return _borderButtonMainFood;
            }

            set
            {
                _borderButtonMainFood = value;
                this.OnPropertyChanged("BorderButtonMainFood");
            }
        }

        public string BorderButtonAlternativeFood
        {
            get
            {
                return _borderButtonAlternativeFood;
            }

            set
            {
                _borderButtonAlternativeFood = value;
                this.OnPropertyChanged("BorderButtonAlternativeFood");
            }
        }

        public string BorderButtonPickGroup
        {
            get
            {
                return _borderButtonPickGroup;
            }

            set
            {
                _borderButtonPickGroup = value;
                this.OnPropertyChanged("BorderButtonPickGroup");
            }
        }

        public string BackgroundButtonPickRoom
        {
            get
            {
                return _backgroundButtonPickRoom;
            }

            set
            {
                _backgroundButtonPickRoom = value;
                this.OnPropertyChanged("BackgroundButtonPickRoom");
            }
        }

        public string BackgroundButtonPickGroup
        {
            get
            {
                return _backgroundButtonPickGroup;
            }

            set
            {
                _backgroundButtonPickGroup = value;
                this.OnPropertyChanged("BackgroundButtonPickGroup");
            }
        }

        public string BackgroundButtonMainFood
        {
            get
            {
                return _backgroundButtonMainFood;
            }

            set
            {
                _backgroundButtonMainFood = value;
                this.OnPropertyChanged("BackgroundButtonMainFood");
            }
        }

        public string BackgroundButtonAlternativeFood
        {
            get
            {
                return _backgroundButtonAlternativeFood;
            }

            set
            {
                _backgroundButtonAlternativeFood = value;
                this.OnPropertyChanged("BackgroundButtonAlternativeFood");
            }
        }

        public string GridUserVisibility
        {
            get
            {
                return _gridUserVisibility;
            }

            set
            {
                _gridUserVisibility = value;
                this.OnPropertyChanged("GridUserVisibility");
            }
        }

        public int CurrentSelectedGroupId
        {
            get
            {
                return _currentSelectedGroupId;
            }

            set
            {
                _currentSelectedGroupId = value;
                this.OnPropertyChanged("CurrentSelectedGroupId");
            }
        }

        public bool TextBlockFocused
        {
            get
            {
                return _textBlockFocused;
            }

            set
            {
                _textBlockFocused = value;
                this.OnPropertyChanged("TextBlockFocused");
            }
        }

        public string OverlayVisible
        {
            get
            {
                return _overlayVisible;
            }

            set
            {
                _overlayVisible = value;
                this.OnPropertyChanged("OverlayVisible");
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                _mainViewModel = value;
                OnPropertyChanged("MainViewModel");
            }
        }

        public KitchenEmployeeInfo EmployeeInfo
        {
            get
            {
                return _employeeInfo;
            }

            set
            {
                _employeeInfo = value;
                OnPropertyChanged("EmployeeInfo");
            }
        }

        public ObservableCollection<KitchenEmployeeInfo> UserInGroup
        {
            get
            {
                return _userInGroup;
            }

            set
            {
                _userInGroup = value;
                OnPropertyChanged("UserInGroup");
            }
        }

        public ObservableCollection<KitchenEmployeeInfo> DuplicateUser
        {
            get
            {
                return _duplicateUser;
            }

            set
            {
                _duplicateUser = value;
                OnPropertyChanged("DuplicateUser");
            }
        }

        public ObservableCollection<OrderPreview> OrderPreviewData
        {
            get
            {
                return _orderPreviewData;
            }

            set
            {
                _orderPreviewData = value;
                OnPropertyChanged("OrderPreviewData");
            }
        }

        public ObservableCollection<CommonKitchenOrder> CommonKitchenOrder
        {
            get
            {
                return _commonKitchenOrder;
            }

            set
            {
                _commonKitchenOrder = value;
                OnPropertyChanged("CommonKitchenOrder");
            }
        }

        public ObservableCollection<CommonKitchenOrder> AllObservationFood
        {
            get
            {
                return _allObservableFood;
            }

            set
            {
                _allObservableFood = value;
                OnPropertyChanged("AllObservationFood");
            }
        }

        public ObservableCollection<CommonKitchenOrder> AllObservationAlternativeFood
        {
            get
            {
                return _allObservableAlternativeFood;
            }

            set
            {
                _allObservableAlternativeFood = value;
                OnPropertyChanged("AllObservationAlternativeFood");
            }
        }

        public ObservableCollection<CommonKitchenOrder> AllObservationGroup
        {
            get
            {
                return _allObservableGroup;
            }

            set
            {
                _allObservableGroup = value;
                OnPropertyChanged("AllObservationGroup");
            }
        }

        public ObservableCollection<CommonKitchenOrder> AllObservationLocation
        {
            get
            {
                return _allObservableLocation;
            }

            set
            {
                _allObservableLocation = value;
                OnPropertyChanged("AllObservationLocation");
            }
        }

        public bool IsDataEdit
        {
            get
            {
                return _isDataEdit;
            }

            set
            {
                _isDataEdit = value;
                this.OnPropertyChanged("IsDataEdit");
            }
        }

        #region Check Current Menu Properties

        public bool CheckMainFoodMenu
        {
            get
            {
                return CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0
                && CommonKitchenOrder.Count == AllObservationFood.Count;
            }
        }

        public bool CheckAlternativeFoodMenu
        {
            get
            {
                return CommonKitchenOrder.Except(AllObservationAlternativeFood).ToList().Count == 0
                && CommonKitchenOrder.Count == AllObservationAlternativeFood.Count;
            }
        }

        public bool CheckGroupMenu
        {
            get
            {
                return CommonKitchenOrder.Except(AllObservationGroup).ToList().Count == 0
                && CommonKitchenOrder.Count == AllObservationGroup.Count;
            }
        }

        public bool CheckLocationMenu
        {
            get
            {
                return CommonKitchenOrder.Except(AllObservationLocation).ToList().Count == 0
                && CommonKitchenOrder.Count == AllObservationLocation.Count;
            }
        }

        #endregion

        #region Process Button Menu

        /// <summary>
        /// Change button focused color.
        /// </summary>
        /// <param name="currentButton">Value of current button.</param>
        public void ChangeButtonBorder(int currentButton)
        {
            string red = Constant.Red;
            string defaultColor = Constant.DefaultButtonOrderBorderColor;
            string defaulBackgroundColor = Constant.DefaultButtonOrderColor;
            string selectedBackgroundColor = Constant.SelectedButtonOrderColor;
            switch (currentButton)
            {
                case 1:
                    BorderButtonPickRoom = red;
                    BorderButtonPickGroup = BorderButtonMainFood = BorderButtonAlternativeFood = defaultColor;
                    BackgroundButtonPickRoom = selectedBackgroundColor;
                    BackgroundButtonPickGroup = BackgroundButtonMainFood = BackgroundButtonAlternativeFood = defaulBackgroundColor;
                    break;
                case 2:
                    BorderButtonPickRoom = BorderButtonMainFood = BorderButtonAlternativeFood = defaultColor;
                    BorderButtonPickGroup = red;
                    BackgroundButtonPickRoom = BackgroundButtonMainFood = BackgroundButtonAlternativeFood = defaulBackgroundColor;
                    BackgroundButtonPickGroup = selectedBackgroundColor;
                    break;
                case 3:
                    BorderButtonPickRoom = BorderButtonPickGroup = BorderButtonAlternativeFood = defaultColor;
                    BorderButtonMainFood = red;
                    BackgroundButtonPickRoom = BackgroundButtonPickGroup = BackgroundButtonAlternativeFood = defaulBackgroundColor;
                    BackgroundButtonMainFood = selectedBackgroundColor;
                    break;
                case 4:
                    BorderButtonPickRoom = BorderButtonPickGroup = BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = red;

                    BackgroundButtonPickRoom = BackgroundButtonPickGroup = BackgroundButtonMainFood = defaulBackgroundColor;
                    BackgroundButtonAlternativeFood = selectedBackgroundColor;
                    break;
                default:
                    BorderButtonPickRoom = BorderButtonPickGroup = BorderButtonMainFood = BorderButtonAlternativeFood = defaultColor;
                    BackgroundButtonPickRoom = BackgroundButtonPickGroup = BackgroundButtonMainFood = BackgroundButtonAlternativeFood = defaulBackgroundColor;
                    break;
            }
        }

        #endregion

        public void ShowOverlay()
        {
            OverlayVisible = Constant.Visible;
        }

        public void HideOverlay()
        {
            OverlayVisible = Constant.Hidden;
        }

        public void ShowGridDisplayUser()
        {
            GridUserVisibility = Constant.Visible;
        }

        public void HideGridDisplayUser()
        {
            GridUserVisibility = Constant.Hidden;
        }

        /// <summary>
        /// Release data in middle side when user have an action.
        /// </summary>
        /// <param name="isLocationData">Check current menu is Location Data.</param>
        /// <param name="currentSelectedItem">Current selected item.</param>
        public void ReleaseDataMiddleSide(bool isLocationData, CommonKitchenOrder currentSelectedItem)
        {
            if (!isLocationData)
            {
                CommonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                currentSelectedItem.IsSelectedMealMenu = true;
                currentSelectedItem.IsFocusedMealMenu = true;
                IsMenuFocused = true;
            }
            else
            {
                CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                currentSelectedItem.IsSelectedMealMenu = true;
            }
        }
    }
}
