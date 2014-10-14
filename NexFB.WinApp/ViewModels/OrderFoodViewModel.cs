// --------------------------------------------------------------------------
// <copyright file="OrderFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.28">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using AutoMapper;
    using NexFB.Core.Models;
    using NexFB.Criteria.Department;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Entities;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Models;
    using NexFB.WinApp.Views;
    using NexFB.WinApp.Views.ManageOrder;

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public class OrderFoodViewModel : ViewModelBase
    {
        #region Fields
        private const string _AdditionalMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";
        private const string _MainMenuTemplate = "<User SysId='{0}' MenuId='{1}' Qty='{2}'></User>";
        private const string _AlternativeMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";
        private const string _BeginRootTag = "<ROOT>";
        private const string _EndRootTag = "</ROOT>";
        private static readonly OrderFoodPreviewData OrderFoodPreviewData = new OrderFoodPreviewData();
        private static string _defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];
        private static ObservableCollection<CommonKitchenOrder> _commonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableLocation = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableAlternativeFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableGroup = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<KitchenEmployeeInfo> _userInGroup = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<KitchenEmployeeInfo> _duplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<OrderPreview> _orderPreviewData = new ObservableCollection<OrderPreview>();
        private static KitchenEmployeeInfo _employeeInfo;

        private string _result;
        private string _borderButtonPickRoom;
        private string _borderButtonMainFood;
        private string _borderButtonAlternativeFood;
        private string _borderButtonPickGroup;
        private string _imageCancel;
        private string _gridUserVisibility;
        private string _messageHeader;
        private bool _isMenuFocused;
        private int _currentSelectedEmployeeSysId;
        private int _currentSelectedMainMenuId;
        private int _currentSelectedAlternativeMenuId;
        private string _locationName;
        private int _locationId;
        private bool _isVnmCulture;
        private int _totalOrder;
        private string _totalPrice;
        private string _orderNo;
        private int _referenceCode;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed. Suppression is OK here.")]
        public OrderFoodViewModel(Order order = null)
        {
            ResetCurrentModel();
            CurrentCulture();
            ChangeButtonBorder(0);
            GridUserVisibility = Constant.Hidden;
            AddCommand = new RelayCommand((x) => CanExecuteAddCommand(), (y) => ExecuteAddCommand());
            CancelCommand = new RelayCommand((x) => CanExecuteCancelCommand(), (y) => ExecuteCancelCommand());
            ReceiveCommand = new RelayCommand((x) => CanExecuteReceiveCommand(), (y) => ExecuteReceiveCommand());

            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadReleased());
            PickRoomCommand = new RelayCommand((x) => CanExecutePickRoomCommand(), (y) => ExecutePickRoomCommand());
            PickGroupCommand = new RelayCommand((x) => CanExecutePickGroupCommand(), (y) => ExecutePickGroupCommand());
            DefaulMenuSelected = new RelayCommand((x) => CanExecuteMealMenuSelected(), (y) => ExecuteMealMenuSelected(y));
            ChooseMainFoodCommand = new RelayCommand((x) => CanExecuteChooseMainFoodCommand(), (y) => ExecuteChooseMainFoodCommand());
            ChooseAlternativeFoodCommand = new RelayCommand((x) => CanExecuteChooseAlternativeFoodCommand(), (y) => ExecuteChooseAlternativeFoodCommand());
            DefaultEmployeeSelected = new RelayCommand((x) => CanExecuteEmployeeInGroupSelected(), (y) => ExecuteEmployeeInGroupSelected(y));

            SaveDialogCommand = new RelayCommand((x) => CanExecuteSaveDialogCommand(), (y) => ExecuteSaveDialogCommand(y));
            CancelDialogCommand = new RelayCommand((x) => CanExecuteCancelDialogCommand(), (y) => ExecuteCancelDialogCommand(y));
            SelectAllUserInGroup = new RelayCommand((x) => CanExecuteSelectAllUserInGroupCommand(), (y) => ExecuteSelectAllUserInGroupCommand());

            CancelDialogOrderConfirmation = new RelayCommand((x) => CanExecuteCancelDialogOrderConfirmation(), (y) => ExecuteCancelDialogOrderConfirmation(y));
            SaveOrderCommand = new RelayCommand((x) => CanSaveOrderCommand(), (y) => ExecuteSaveOrderCommand(y));

            AllObservationFood = this.GetAllFood;
            AllObservationAlternativeFood = this.GetAllAlternativeFood;
            AllObservationLocation = this.GetAllLocation;
            AllObservationGroup = this.GetAllGroup;

            if (order != null)
            {
                PrepareEditData(order);
            }
        }

        #endregion

        #region Properties

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

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);
                return new ObservableCollection<Order>(waitingMeals);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllLocation
        {
            get
            {
                var getAllLocation = new GetAllLocationCriteria();
                List<Location> locations = this.CommonService.GetAllLocation(getAllLocation).ToList();
                if (locations != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllFood
        {
            get
            {
                var getAlternativeFood = new GetAlternativeFoodCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllAlternativeFoodOrder(getAlternativeFood).ToList();
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllAlternativeFood
        {
            get
            {
                var getAllFood = new GetFoodNameCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllFoodOrder(getAllFood);
                if (foods != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllGroup
        {
            get
            {
                var getAllGroup = new GetGroupNameCriteria();
                List<GroupOrder> groups = this.KitchenService.GetAllGroupOrder(getAllGroup);
                if (groups != null)
                {
                    List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                    data = data.Where(x => !string.IsNullOrEmpty(x.ItemName)).ToList();
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
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

        public string ImageCancel
        {
            get
            {
                return _imageCancel;
            }

            set
            {
                _imageCancel = value;
                this.OnPropertyChanged("ImageCancel");
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

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecuteSelectAllUserInGroupCommand()
        {
            ////UserInGroup.Count > 0 && UserInGroup.Any(x => !x.IsPlaceOrder
            return true;
        }

        public bool CanSaveOrderCommand()
        {
            return true;
        }

        public bool CanExecuteSaveDialogCommand()
        {
            return true;
        }

        public bool CanExecuteCancelDialogCommand()
        {
            return true;
        }

        public bool CanExecuteCancelDialogOrderConfirmation()
        {
            return true;
        }

        public bool CanExecuteReceiveCommand()
        {
            if (EmployeeInfo != null && UserInGroup.Any(x => x.IsPlaceOrder
                && (x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0)))
            {
                return true;
            }

            return false;
        }

        public bool CanExecuteAddCommand()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                return true;
            }

            return false;
        }

        public bool CanExecuteCancelCommand()
        {
            if (EmployeeInfo != null || CommonKitchenOrder.Count > 0 || (Result != null && Result.Count() > 0))
            {
                return true;
            }

            return false;
        }

        public bool CanExecutePickRoomCommand()
        {
            if (EmployeeInfo != null)
            {
                return true;
            }

            return false;
        }

        public bool CanExecutePickGroupCommand()
        {
            return true;
        }

        /// <summary>
        /// Can Execute button.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CanExecuteChooseMainFoodCommand()
        {
            if (UserInGroup != null)
            {
                if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanExecuteChooseAlternativeFoodCommand()
        {
            return CanExecuteChooseMainFoodCommand();
        }

        public bool CanExecuteNumPadPressed()
        {
            if (Result != null && Result.Length == 5)
            {
                return false;
            }

            return true;
        }

        public bool CanExecuteMealMenuSelected()
        {
            return true;
        }

        public bool CanExecuteEmployeeInGroupSelected()
        {
            return true;
        }

        /// <summary>
        /// Action for Keypad Released.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CanExecuteNumPadReleased()
        {
            bool isMainFoodMenu = CheckMainFoodMenu;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup) || (CommonKitchenOrder.Any(x => x.IsSelectedMealMenu) && isMainFoodMenu))
            {
                ImageCancel = Constant.CancelImageActive;
                return true;
            }
            else
            {
                ImageCancel = Constant.CancelImageDisabled;
                return false;
            }
        }

        // Execute Command

        /// <summary>
        /// Save Order Command.
        /// </summary>
        /// <param name="value">Current Screen.</param>
        public void ExecuteSaveOrderCommand(object value)
        {
            int locationId = EmployeeInfo.LocationId;
            var mainMenuData = new StringBuilder(_BeginRootTag);
            var alternativeMenuData = new StringBuilder(_BeginRootTag);
            var additionalMenuData = new StringBuilder(_BeginRootTag);

            foreach (KitchenEmployeeInfo user in UserInGroup)
            {
                List<EmployeeOrderDetails> mainOrders = user.ListMainMenuOrderDeatails;
                List<EmployeeOrderDetails> additionalOrders = user.ListAlternativeMenuOrderDeatails;
                if (mainOrders != null && mainOrders.Count > 0)
                {
                    foreach (EmployeeOrderDetails mainOrder in mainOrders)
                    {
                        if (mainOrder.SelectedMenuItemId == int.MaxValue)
                        {
                            mainMenuData.AppendFormat(_MainMenuTemplate, user.SysId, 1, mainOrder.SelectedMenuItemQuantity);
                        }
                        else
                        {
                            alternativeMenuData.AppendFormat(_AlternativeMenuTemplate, user.SysId, mainOrder.SelectedMenuItemId, mainOrder.SelectedMenuItemQuantity);
                        }
                    }
                }

                if (additionalOrders != null && additionalOrders.Count > 0)
                {
                    foreach (EmployeeOrderDetails additionalOrder in additionalOrders)
                    {
                        additionalMenuData.AppendFormat(_AdditionalMenuTemplate, user.SysId, additionalOrder.SelectedMenuItemId, additionalOrder.SelectedMenuItemQuantity);
                    }
                }
            }

            if (ReferenceCode == 0)
            {
                InsertNewOrder(locationId, mainMenuData, alternativeMenuData, additionalMenuData, value);
            }
            else
            {
                EditOrder(locationId, mainMenuData, alternativeMenuData, additionalMenuData, value);
            }
        }

        /// <summary>
        /// Edit an existing order.
        /// </summary>
        /// <param name="locationId">Current owner location.</param>
        /// <param name="mainMenuData">Data Main menu.</param>
        /// <param name="alternativeMenuData">Data Alternative Menu.</param>
        /// <param name="additionalMenuData">Data Additional Menu.</param>
        /// <param name="value">Current object window.</param>
        public void EditOrder(int locationId, StringBuilder mainMenuData, StringBuilder alternativeMenuData, StringBuilder additionalMenuData, object value)
        {
            var editOrderMealCriteria = new EditOrderMealCriteria()
            {
                Owner = EmployeeInfo.SysId,
                ModifiedDate = DateTime.Now,
                LocationId = locationId > 0 ? locationId : LocationId,
                Menus = mainMenuData.Append(_EndRootTag).ToString(),
                Alternative = alternativeMenuData.Append(_EndRootTag).ToString(),
                Foods = additionalMenuData.Append(_EndRootTag).ToString(),
                OrderId = ReferenceCode
            };
            CommandResult result = KitchenService.EditOrderFood(editOrderMealCriteria);
            if (result.Code != 0)
            {
                MessageBox.Show(result.Message);
            }
            else
            {
                if (MessageBox.Show("Order thành công!", string.Empty, MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    ExecuteCancelDialogOrderConfirmation(value);
                    ExecutePrintInvoice(ReferenceCode, Constant.DeliverOrderFoodInvoice);
                    ResetCurrentModel();
                }
            }
        }

        /// <summary>
        /// Insert new order.
        /// </summary>
        /// <param name="locationId">Current owner location.</param>
        /// <param name="mainMenuData">Data Main menu.</param>
        /// <param name="alternativeMenuData">Data Alternative Menu.</param>
        /// <param name="additionalMenuData">Data Additional Menu.</param>
        /// <param name="value">Current object window.</param>
        public void InsertNewOrder(int locationId, StringBuilder mainMenuData, StringBuilder alternativeMenuData, StringBuilder additionalMenuData, object value)
        {
            var insertOrderMealCriteria = new InsertOrderMealCriteria()
            {
                Owner = EmployeeInfo.SysId,
                CreatedDate = DateTime.Now,
                LocationId = locationId > 0 ? locationId : LocationId,
                Menus = mainMenuData.Append(_EndRootTag).ToString(),
                Alternative = alternativeMenuData.Append(_EndRootTag).ToString(),
                Foods = additionalMenuData.Append(_EndRootTag).ToString()
            };

            OrderResult result = KitchenService.InsertOrderFood(insertOrderMealCriteria);
            if (result.CommandResult.Code != 0)
            {
                MessageBox.Show(result.CommandResult.Message);
            }
            else
            {
                if (MessageBox.Show("Order thành công!", string.Empty, MessageBoxButton.OK) == MessageBoxResult.OK)
                {
                    ExecuteCancelDialogOrderConfirmation(value);
                    ExecutePrintInvoice(result.OrderId, Constant.DeliverOrderFoodInvoice);
                    ResetCurrentModel();
                }
            }
        }

        /// <summary>
        /// Receive order.
        /// </summary>
        public void ExecuteReceiveCommand()
        {
            bool dataValid = CheckDataValid();
            if (dataValid)
            {
                OrderConfirmationWindow confirmationWindow = new OrderConfirmationWindow();
                List<OrderPreview> orderPreview = OrderFoodPreviewData.PrepareDataForKitchenPreview(UserInGroup.ToList(), IsVnmCulture);
                OrderPreviewData = new ObservableCollection<OrderPreview>(orderPreview);
                TotalOrder = orderPreview.Sum(x => x.Quantity);
                decimal totalPrice = orderPreview.Sum(x => x.UnitPrice);
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                TotalPrice = IsVnmCulture ? string.Format(culture, "{0:C}", totalPrice) : string.Format("{0:C}", totalPrice);

                confirmationWindow.DataContext = this;
                confirmationWindow.Owner = Application.Current.MainWindow;
                confirmationWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Validate data before save order.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CheckDataValid()
        {
            if (EmployeeInfo != null)
            {
                if (EmployeeInfo.LocationId < 1 && LocationId < 1)
                {
                    MessageBox.Show("Vui lòng lựa chọn phòng");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Chưa có thông tin người gọi món");
                return false;
            }

            if (UserInGroup != null && UserInGroup.Count > 0)
            {
                if (!UserInGroup.Any(x => x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0))
                {
                    MessageBox.Show("Không thể order với tổng s món ăn bằng 0");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập thông tin gọi món!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Cancel Command.
        /// </summary>
        public void ExecuteCancelCommand()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                Result = null;
            }
            else
            {
                MessageBoxResult confirmCancel = MessageBox.Show("Xác nhận hủy gọi món?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmCancel == MessageBoxResult.Yes)
                {
                    ResetCurrentModel();
                }
            }
        }

        public void ExecuteSelectAllUserInGroupCommand()
        {
            UserInGroup.ToList().ForEach(x => x.IsPlaceOrder = true);
            foreach (KitchenEmployeeInfo item in UserInGroup)
            {
                ChangeMenuColor(item);
            }

            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
        }

        /// <summary>
        /// Add member to order.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void ExecuteAddCommand()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                if (Result.StartsWith("0", StringComparison.CurrentCulture))
                {
                    if (Result.Length == 5)
                    {
                        var getEmployeeByCodeCriteria = new GetEmployeeByCodeCriteria() { Code = Result };
                        List<EmployeeInfo> employees = this.KitchenService.GetEmployeeOrderInfo(getEmployeeByCodeCriteria);
                        List<KitchenEmployeeInfo> listKitchenEmployeeInfo = Mapper.Map<List<EmployeeInfo>, List<KitchenEmployeeInfo>>(employees);
                        if (employees.Any())
                        {
                            if (employees.Count == 1)
                            {
                                if (EmployeeInfo == null)
                                {
                                    EmployeeInfo = listKitchenEmployeeInfo.SingleOrDefault();
                                    if (!string.IsNullOrEmpty(EmployeeInfo.LocationName))
                                    {
                                        LocationName = EmployeeInfo.LocationName;
                                    }

                                    GetEmployeeInGroup(EmployeeInfo.SysId);
                                }
                                else
                                {
                                    GetEmployeeInGroup(listKitchenEmployeeInfo.SingleOrDefault().SysId);
                                }
                            }
                            else
                            {
                                DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>(listKitchenEmployeeInfo);
                                MessageHeader = string.Format(WinFormMainResource.DuplicateEmployeeMessage, DuplicateUser.Count);
                                EmployeeSelectionWindow employeeSelectionWindow = new EmployeeSelectionWindow();
                                employeeSelectionWindow.DataContext = this;
                                employeeSelectionWindow.Owner = Application.Current.MainWindow;
                                employeeSelectionWindow.ShowDialog();
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format(WinFormMainResource.CannotFindEmployeeCode, Result));
                        }
                    }
                    else
                    {
                        MessageBox.Show(WinFormMainResource.IllegalEmployeeCode);
                    }
                }
                else
                {
                    bool isMainMenu = CheckMainFoodMenu;
                    bool isAlternativeMenu = CheckAlternativeFoodMenu;
                    if (isMainMenu || isAlternativeMenu)
                    {
                        var currentSelectedItem = CommonKitchenOrder.Where(x => x.IsFocusedMealMenu && x.IsSelectedMealMenu).SingleOrDefault();
                        if (currentSelectedItem != null)
                        {
                            KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                            if (isMainMenu)
                            {
                                AddSelectedMenuItemToList(currentSelectedItem.ItemId, currentSelectedMember, 1, int.Parse(Result));
                            }
                            else
                            {
                                AddSelectedMenuItemToList(currentSelectedItem.ItemId, currentSelectedMember, 2, int.Parse(Result));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select an item to add more quantity!");
                        }
                    }
                }

                Result = null;
            }
        }

        public void ExecutePickRoomCommand()
        {
            ChangeButtonBorder(1);
            CommonKitchenOrder = AllObservationLocation;
        }

        public void ExecutePickGroupCommand()
        {
            ChangeButtonBorder(2);
            CommonKitchenOrder = AllObservationGroup;
        }

        /// <summary>
        /// Choose Menu command.
        /// </summary>
        public void ExecuteChooseMainFoodCommand()
        {
            ChangeButtonBorder(3);
            CommonKitchenOrder = AllObservationFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                ChangeMenuColor(currentSelectedMember);
            }
        }

        /// <summary>
        /// Choose Alternative Food command.
        /// </summary>
        public void ExecuteChooseAlternativeFoodCommand()
        {
            ChangeButtonBorder(4);
            CommonKitchenOrder = AllObservationAlternativeFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                ChangeMenuColor(currentSelectedMember);
            }
        }

        /// <summary>
        /// Execute Meal Menu Selected.
        /// </summary>
        /// <param name="value">Value pass from user interface.</param>
        public void ExecuteMealMenuSelected(object value)
        {
            if (value != null)
            {
                bool isMainFoodMenu = CheckMainFoodMenu;
                bool isAlternativeFoodMenu = CheckAlternativeFoodMenu;
                var itemId = (int)value;
                var currentSelectedItem = CommonKitchenOrder.Where(x => x.ItemId == itemId).SingleOrDefault();
                if (isMainFoodMenu || isAlternativeFoodMenu)
                {
                    CommonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                    currentSelectedItem.IsFocusedMealMenu = true;
                    currentSelectedItem.IsSelectedMealMenu = true;
                    IsMenuFocused = true;
                    KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();

                    if (isMainFoodMenu)
                    {
                        if (itemId == int.MaxValue)
                        {
                            currentSelectedMember.IsDeleteMainFood = false;
                        }

                        CurrentSelectedMainMenuId = itemId;
                        AddSelectedMenuItemToList(itemId, currentSelectedMember, 1, 0);
                    }
                    else
                    {
                        CurrentSelectedAlternativeMenuId = itemId;
                        AddSelectedMenuItemToList(itemId, currentSelectedMember, 2, 0);
                    }
                }

                if (CheckGroupMenu)
                {
                    currentSelectedItem.IsSelectedMealMenu = !currentSelectedItem.IsSelectedMealMenu;
                    AddMemberInGroupToGrid(currentSelectedItem);
                }

                if (CheckLocationMenu)
                {
                    CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                    currentSelectedItem.IsSelectedMealMenu = true;
                    this.LocationName = currentSelectedItem.ItemName;
                    this.LocationId = itemId;
                }
            }
        }

        /// <summary>
        /// Add Menu to employee menu list.
        /// </summary>
        /// <param name="itemId">Menu item id.</param>
        /// <param name="currentSelectedMember">Current member.</param>
        /// <param name="menu">Main or snack menu.</param>
        /// <param name="increaseValue">Additional value.</param>
        public void AddSelectedMenuItemToList(int itemId, KitchenEmployeeInfo currentSelectedMember, int menu, int increaseValue)
        {
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                EmployeeOrderDetails employeeOrderDetails = null;
                List<EmployeeOrderDetails> currentMenuList = null;
                var orderDetails = new EmployeeOrderDetails()
                {
                    SelectedMenuItemId = itemId,
                    SelectedMenuItemQuantity = 1,
                };

                switch (menu)
                {
                    case 1:
                        currentMenuList = currentSelectedMember.ListMainMenuOrderDeatails;
                        employeeOrderDetails = currentMenuList.Where(x => x.SelectedMenuItemId == itemId).SingleOrDefault();

                        break;
                    case 2:
                        currentMenuList = currentSelectedMember.ListAlternativeMenuOrderDeatails;
                        employeeOrderDetails = currentMenuList.Where(x => x.SelectedMenuItemId == itemId).SingleOrDefault();
                        break;
                }

                CommonKitchenOrder selectedMenu = CommonKitchenOrder.Where(x => x.ItemId == itemId).SingleOrDefault();
                if (selectedMenu != null)
                {
                    orderDetails.SelectedMenuItemName = selectedMenu.ItemName;
                    orderDetails.SelectedMenuItemPrice = selectedMenu.Price;
                    if (!currentMenuList.Any(x => x.SelectedMenuItemId == itemId))
                    {
                        currentMenuList.Add(orderDetails);
                        selectedMenu.Quantity = orderDetails.SelectedMenuItemQuantity;
                    }
                    else
                    {
                        if (increaseValue != 0)
                        {
                            employeeOrderDetails.SelectedMenuItemQuantity += increaseValue;
                            selectedMenu.Quantity = employeeOrderDetails.SelectedMenuItemQuantity;
                        }
                    }
                }

                currentSelectedMember.OrderSummary = DisplayEmployeeOrderSummary(currentSelectedMember);
                UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
            }
        }

        /// <summary>
        /// Selected Employee In Group.
        /// </summary>
        /// <param name="value">Employee Sys Id.</param>
        public void ExecuteEmployeeInGroupSelected(object value)
        {
            if (value != null)
            {
                var sysId = (int)value;
                KitchenEmployeeInfo previousEmployee = UserInGroup.Where(x => x.ListMainMenuOrderDeatails.Count == 0 && x.IsDeleteMainFood).SingleOrDefault();
                if (previousEmployee != null)
                {
                    if (previousEmployee.SysId != sysId)
                    {
                        MessageBox.Show("Xin chọn ít nhất một món ăn");
                    }
                }
                else
                {
                    IsMenuFocused = false;
                    var currentSelectedEmployee = UserInGroup.Where(x => x.SysId == sysId).SingleOrDefault();
                    UserInGroup.ToList().ForEach(y => y.IsSelectedMemberInGroup = false);
                    currentSelectedEmployee.IsSelectedMemberInGroup = true;
                    currentSelectedEmployee.IsPlaceOrder = true;

                    CurrentSelectedEmployeeSysId = sysId;
                    ChangeMemberOrderColor(currentSelectedEmployee);

                    if (EmployeeInfo == null)
                    {
                        EmployeeInfo = currentSelectedEmployee;
                    }

                    ChangeMenuColor(currentSelectedEmployee);
                }
            }
        }

        public List<KitchenEmployeeInfo> GetUserByGroup(int groupId)
        {
            var getUserByGroupIdCriteria = new GetUserByGroupIdCriteria() { GroupOrderId = groupId };
            List<EmployeeInfo> users = this.KitchenService.GetUserByGroupId(getUserByGroupIdCriteria);
            List<KitchenEmployeeInfo> data = Mapper.Map<List<EmployeeInfo>, List<KitchenEmployeeInfo>>(users);

            return data;
        }

        /// <summary>
        /// Add employee in dialog after user select an Employee.
        /// </summary>
        /// <param name="value">Dialog object.</param>
        public void ExecuteSaveDialogCommand(object value)
        {
            if (DuplicateUser.Any(x => x.IsChecked == true))
            {
                if (EmployeeInfo == null)
                {
                    EmployeeInfo = DuplicateUser.Where(x => x.IsChecked == true).FirstOrDefault();
                    GetEmployeeInGroup(EmployeeInfo.SysId, value);
                }
                else
                {
                    GetEmployeeInGroup(DuplicateUser.Where(x => x.IsChecked == true).FirstOrDefault().SysId, value);
                }
                ////CloseDialog(value);
            }
        }

        public void ExecuteCancelDialogCommand(object value)
        {
            CloseDialog(value);
        }

        public void CloseDialog(object value)
        {
            if (value != null)
            {
                var currentWindow = (EmployeeSelectionWindow)value;
                currentWindow.Close();
            }
        }

        public void ExecuteCancelDialogOrderConfirmation(object value)
        {
            if (value != null)
            {
                var currentWindow = (OrderConfirmationWindow)value;
                currentWindow.Close();
            }
        }

        /// <summary>
        /// Get Employee Belong to a Group.
        /// </summary>
        /// <param name="sysId">Employee Sys Id.</param>
        /// <param name="value">Instance of EmployeeSelectionWindow.</param>
        public void GetEmployeeInGroup(int sysId, object value = null)
        {
            var getEmployeeInGroupBySysIdCriteria = new GetEmployeeInGroupBySysIdCriteria() { SysId = sysId };
            EmployeeInGroup employeeInGroup = this.KitchenService.GetEmployeeInGroup(getEmployeeInGroupBySysIdCriteria);
            KitchenEmployeeInfo currentSelectedEmployee = null;
            if (employeeInGroup != null)
            {
                if (employeeInGroup.GroupOrder != null)
                {
                    int groupOrderId = employeeInGroup.GroupOrder.GroupOrderId;
                    EmployeeInfo.GroupOrderId = groupOrderId;
                    ExecutePickGroupCommand();

                    CommonKitchenOrder currentSelectedItem = CommonKitchenOrder.Where(x => x.ItemId == groupOrderId).FirstOrDefault();
                    currentSelectedItem.IsSelectedMealMenu = true;
                    AddMemberInGroupToGrid(currentSelectedItem);
                    currentSelectedEmployee = UserInGroup.Where(x => x.SysId == sysId).SingleOrDefault();
                    currentSelectedEmployee.IsPlaceOrder = true;
                    currentSelectedEmployee.IsSelectedMemberInGroup = true;
                    CloseEmployeeSelectionDialog(value);
                }
                else
                {
                    if (!UserInGroup.Any(x => x.SysId == sysId))
                    {
                        KitchenEmployeeInfo employee = this.GetEmployeeBySysId(sysId);
                        if (employee != null)
                        {
                            UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                            employee.IsSelectedMemberInGroup = true;
                            employee.IsPlaceOrder = true;
                            UserInGroup.Add(employee);
                            GridUserVisibility = Constant.Visible;
                            currentSelectedEmployee = employee;
                            CloseEmployeeSelectionDialog(value);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee has already existed in the current order");
                    }
                }

                CurrentSelectedEmployeeSysId = sysId;
                ChangeMenuColor(currentSelectedEmployee);
            }
        }

        public void CloseEmployeeSelectionDialog(object value)
        {
            if (value != null)
            {
                CloseDialog(value);
            }
        }

        /// <summary>
        /// Add Member In Group to Grid.
        /// </summary>
        /// <param name="currentSelectedItem">Current Selected member.</param>
        public void AddMemberInGroupToGrid(CommonKitchenOrder currentSelectedItem)
        {
            if (CheckGroupMenu)
            {
                if (currentSelectedItem.IsSelectedMealMenu)
                {
                    List<KitchenEmployeeInfo> employees = GetUserByGroup(currentSelectedItem.ItemId);
                    if (employees.Any() && employees.Count > 0)
                    {
                        List<int> listSysId = UserInGroup.Select(x => x.SysId).ToList();
                        employees = employees.Where(x => !listSysId.Contains(x.SysId)).ToList();
                        employees.ForEach(x => UserInGroup.Add(x));
                        GridUserVisibility = Constant.Visible;
                        UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                    }
                }
                else
                {
                    //// Unselected item in group.
                    if (UserInGroup != null)
                    {
                        List<KitchenEmployeeInfo> currenUserInGroup = UserInGroup.Where(x => x.GroupOrderId != currentSelectedItem.ItemId || x.IsPlaceOrder).ToList();
                        UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(currenUserInGroup.Where(x => x.GroupOrderId > 0));
                        if (UserInGroup.Count == 0)
                        {
                            GridUserVisibility = Constant.Hidden;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method Press virtual key pad.
        /// </summary>
        /// <param name="value">Object parameter.</param>
        public void ExecuteNumPadPressed(object value)
        {
            var keyPadParameter = value.ToString();
            switch (keyPadParameter)
            {
                case "ESC":
                    break;

                case "RETURN":
                    break;

                case "BACK":
                    if (Result.Length > 0)
                    {
                        Result = Result.Remove(Result.Length - 1);
                    }

                    break;

                default:
                    string keypadValue = keyPadParameter.Substring(keyPadParameter.Length - 1, 1);
                    Result += keypadValue;
                    break;
            }
        }

        /// <summary>
        /// Execute Number keypad Released.
        /// </summary>
        public void ExecuteNumPadReleased()
        {
            var currentSelectedEmployee = UserInGroup.Where(x => x.SysId == CurrentSelectedEmployeeSysId).SingleOrDefault();
            CommonKitchenOrder currentSelectedMenu = null;
            EmployeeOrderDetails menuRemoved = null;
            if (!IsMenuFocused)
            {
                EmployeeInListReleased(currentSelectedEmployee);
            }
            else
            {
                if (!CommonKitchenOrder.Any(x => x.IsSelectedMealMenu && x.IsFocusedMealMenu))
                {
                    EmployeeInListReleased(currentSelectedEmployee);
                }
                else
                {
                    if (CheckMainFoodMenu)
                    {
                        if (CurrentSelectedMainMenuId > 0)
                        {
                            currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == CurrentSelectedMainMenuId).SingleOrDefault();
                            menuRemoved = currentSelectedEmployee.ListMainMenuOrderDeatails.Where(x => x.SelectedMenuItemId == CurrentSelectedMainMenuId).SingleOrDefault();
                            if (menuRemoved != null)
                            {
                                currentSelectedEmployee.ListMainMenuOrderDeatails.Remove(menuRemoved);
                                currentSelectedEmployee.IsDeleteMainFood = true;
                            }
                        }
                    }
                    else
                    {
                        if (CurrentSelectedAlternativeMenuId > 0)
                        {
                            currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == CurrentSelectedAlternativeMenuId).SingleOrDefault();
                            menuRemoved = currentSelectedEmployee.ListAlternativeMenuOrderDeatails.Where(x => x.SelectedMenuItemId == CurrentSelectedAlternativeMenuId).SingleOrDefault();
                            if (menuRemoved != null)
                            {
                                currentSelectedEmployee.ListAlternativeMenuOrderDeatails.Remove(menuRemoved);
                            }
                        }
                    }

                    currentSelectedEmployee.OrderSummary = DisplayEmployeeOrderSummary(currentSelectedEmployee);
                    UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
                    currentSelectedMenu.Quantity = 0;
                    ChangeMenuColor(currentSelectedEmployee);
                }
            }
        }

        /// <summary>
        /// Remove employee order.
        /// </summary>
        /// <param name="currentSelectedEmployee">Selected employee.</param>
        public void EmployeeInListReleased(KitchenEmployeeInfo currentSelectedEmployee)
        {
            if (CurrentSelectedEmployeeSysId > 0)
            {
                currentSelectedEmployee.IsPlaceOrder = false;
                currentSelectedEmployee.IsSelectedMemberInGroup = false;
                currentSelectedEmployee.IsDeleteMainFood = false;
                currentSelectedEmployee.ListMainMenuOrderDeatails = new List<EmployeeOrderDetails>();
                currentSelectedEmployee.ListAlternativeMenuOrderDeatails = new List<EmployeeOrderDetails>();
                currentSelectedEmployee.OrderSummary = null;
                CommonKitchenOrder = AllObservationGroup;
                ChangeButtonBorder(2);
                ChangeMemberOrderColor(currentSelectedEmployee);
            }
        }

        /// <summary>
        /// Change member color.
        /// </summary>
        /// <param name="currentSelectedEmployee">Current selected employee.</param>
        public void ChangeMemberOrderColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            if (currentSelectedEmployee.Quantity == 0)
            {
                if (currentSelectedEmployee.IsVegetarian)
                {
                    if (currentSelectedEmployee.IsPlaceOrder)
                    {
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.RegisterVegetarianMealColor;
                    }
                    else
                    {
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.UNRegisterMealColor;
                    }
                }
                else
                {
                    if (currentSelectedEmployee.IsPlaceOrder)
                    {
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.RegisterNonVegetarianMealColor;
                    }
                    else
                    {
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.UNRegisterMealColor;
                    }
                }
            }
            else
            {
                currentSelectedEmployee.EmployeeOrderMealColor = Constant.HasOrderMealBeforeColor;
            }
        }

        /// <summary>
        /// Change Menu color.
        /// </summary>
        /// <param name="currentSelectedEmployee">Menu color of Current Selected employee.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void ChangeMenuColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            bool isMainFoodMenu = CheckMainFoodMenu;
            bool isAlternativeFoodMenu = CheckAlternativeFoodMenu;
            if (!currentSelectedEmployee.ListMainMenuOrderDeatails.Any(x => x.SelectedMenuItemId == int.MaxValue))
            {
                if (!currentSelectedEmployee.IsDeleteMainFood)
                {
                    string mealName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
                    if (string.IsNullOrEmpty(mealName))
                    {
                        mealName = Constant.FixedMeal;
                    }

                    var employeeMainMeal = new EmployeeOrderDetails()
                    {
                        SelectedMenuItemId = int.MaxValue,
                        SelectedMenuItemName = mealName,
                        SelectedMenuItemQuantity = 1,
                        SelectedMenuItemPrice = 0,
                        IsMainMenu = true
                    };

                    currentSelectedEmployee.ListMainMenuOrderDeatails.Add(employeeMainMeal);
                    currentSelectedEmployee.OrderSummary = DisplayEmployeeOrderSummary(currentSelectedEmployee);
                }
            }

            if (isMainFoodMenu || isAlternativeFoodMenu)
            {
                AddFixedMenuToEmployee(currentSelectedEmployee, isMainFoodMenu);
                CommonKitchenOrder.ToList().ForEach(x => { x.IsFocusedMealMenu = false; x.IsSelectedMealMenu = false; x.Quantity = 0; });
                List<EmployeeOrderDetails> employeeOrderDetails = null;

                if (isMainFoodMenu)
                {
                    employeeOrderDetails = currentSelectedEmployee.ListMainMenuOrderDeatails;
                }
                else
                {
                    employeeOrderDetails = currentSelectedEmployee.ListAlternativeMenuOrderDeatails;
                }

                List<int> selectedMainMenu = employeeOrderDetails.Select(x => x.SelectedMenuItemId).ToList();
                List<CommonKitchenOrder> employeeMenus = CommonKitchenOrder.Where(x => selectedMainMenu.Contains(x.ItemId)).ToList();
                foreach (var menu in employeeMenus)
                {
                    foreach (var employeeDetail in employeeOrderDetails)
                    {
                        if (menu.ItemId == employeeDetail.SelectedMenuItemId)
                        {
                            menu.Quantity = employeeDetail.SelectedMenuItemQuantity;
                            menu.IsSelectedMealMenu = true;
                        }
                    }
                }

                CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
            }
        }

        /// <summary>
        /// Add current menu of employee to list.
        /// </summary>
        /// <param name="currentSelectedEmployee">Current selected employee.</param>
        /// <param name="isMainFoodMenu">Is main or alternative menu.</param>
        public void AddFixedMenuToEmployee(KitchenEmployeeInfo currentSelectedEmployee, bool isMainFoodMenu)
        {
            if (isMainFoodMenu)
            {
                string mealName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
                var fixedMeal = new CommonKitchenOrder()
                {
                    ItemId = int.MaxValue,
                    IsSelectedMealMenu = false,
                    IsMainMenu = true,
                    ItemName = mealName,
                };

                if (!CommonKitchenOrder.Any(x => x.ItemId == int.MaxValue))
                {
                    CheckExistingFixedMenu(currentSelectedEmployee, fixedMeal);
                    CommonKitchenOrder.Insert(0, fixedMeal);
                }
                else
                {
                    CommonKitchenOrder currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == int.MaxValue).SingleOrDefault();
                    CheckExistingFixedMenu(currentSelectedEmployee, currentSelectedMenu);
                }

                CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
            }
        }

        public void CheckExistingFixedMenu(KitchenEmployeeInfo currentSelectedEmployee, CommonKitchenOrder currentSelectedMenu)
        {
            if (string.IsNullOrEmpty(currentSelectedEmployee.MenuEngName) || string.IsNullOrEmpty(currentSelectedEmployee.MenuVnmName))
            {
                currentSelectedMenu.ItemName = WinFormMainResource.MainFood;
            }
            else
            {
                currentSelectedMenu.ItemName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
            }
        }

        /// <summary>
        /// Get Employee By Sys Id.
        /// </summary>
        /// <param name="sysId">Employee Sys Id.</param>
        /// <returns>KitchenEmployee Info.</returns>
        public KitchenEmployeeInfo GetEmployeeBySysId(int sysId)
        {
            var criteria = new GetEmployeeBySysIdCriteria() { SysId = sysId };
            KitchenEmployeeInfo kitchenEmployee = null;
            EmployeeInfo employee = this.KitchenService.GetEmployeeBySysId(criteria);
            if (employee != null)
            {
                kitchenEmployee = Mapper.Map<EmployeeInfo, KitchenEmployeeInfo>(employee);
            }

            return kitchenEmployee;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change button focused color.
        /// </summary>
        /// <param name="currentButton">Value of current button.</param>
        public void ChangeButtonBorder(int currentButton)
        {
            string red = Constant.Red;
            string defaultColor = Constant.DefaultButtonOrderBorderColor;
            switch (currentButton)
            {
                case 1:
                    BorderButtonPickRoom = red;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = defaultColor;
                    break;
                case 2:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = red;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = defaultColor;
                    break;
                case 3:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = red;
                    BorderButtonAlternativeFood = defaultColor;
                    break;
                case 4:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = red;
                    break;
                default:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = defaultColor;
                    break;
            }
        }

        /// <summary>
        /// Cancel Current Order.
        /// </summary>
        public void ResetCurrentModel()
        {
            string defaultColor = Constant.DefaultButtonOrderBorderColor;
            CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>();
            DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
            AllObservationFood = this.GetAllFood;
            AllObservationAlternativeFood = this.GetAllAlternativeFood;
            AllObservationGroup = this.GetAllGroup;
            AllObservationLocation = this.GetAllLocation;
            EmployeeInfo = null;
            Result = null;
            LocationName = null;
            GridUserVisibility = Constant.Hidden;
            MessageHeader = string.Empty;
            BorderButtonMainFood = defaultColor;
            BorderButtonPickGroup = defaultColor;
            BorderButtonPickRoom = defaultColor;
            BorderButtonAlternativeFood = defaultColor;
            CurrentSelectedAlternativeMenuId = 0;
            CurrentSelectedEmployeeSysId = 0;
            CurrentSelectedMainMenuId = 0;
            IsMenuFocused = false;
            OrderNo = string.Empty;
            ReferenceCode = 0;
        }

        /// <summary>
        /// Display employee order summary.
        /// </summary>
        /// <param name="currentSelectedMember">Current selected employee.</param>
        /// <returns>String order details.</returns>
        public string DisplayEmployeeOrderSummary(KitchenEmployeeInfo currentSelectedMember)
        {
            var listMainOrder = currentSelectedMember.ListMainMenuOrderDeatails;
            var listAlternativeOrder = currentSelectedMember.ListAlternativeMenuOrderDeatails;
            int sumMainOrder = listMainOrder.Sum(x => x.SelectedMenuItemQuantity);
            int sumAlternativeOrder = listAlternativeOrder.Sum(x => x.SelectedMenuItemQuantity);
            string orderInfoSummary = string.Empty;

            if (listMainOrder.Count > 0)
            {
                orderInfoSummary += CombineDataOrderSummary(listMainOrder);
            }

            orderInfoSummary += " - ";
            if (listAlternativeOrder.Count > 0)
            {
                orderInfoSummary += CombineDataOrderSummary(listAlternativeOrder);
            }

            if (orderInfoSummary.Count() > 100)
            {
                orderInfoSummary = string.Empty;
                if (listAlternativeOrder.Count > 0)
                {
                    if (listMainOrder.Count > 0)
                    {
                        orderInfoSummary += WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.CommaSeparated
                            + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
                    }
                    else
                    {
                        orderInfoSummary += WinFormMainResource.Parentheses1 + sumAlternativeOrder + WinFormMainResource.Snack + WinFormMainResource.Parentheses2;
                    }
                }
                else
                {
                    if (listMainOrder.Count > 0)
                    {
                        orderInfoSummary += WinFormMainResource.Parentheses1 + sumMainOrder + WinFormMainResource.Main + WinFormMainResource.Parentheses2;
                    }
                }
            }

            return orderInfoSummary;
        }

        /// <summary>
        /// Check string length of general order info.
        /// </summary>
        /// <param name="listMainOrder">List main order.</param>
        /// <param name="listAlternativeOrder">List alternative order.</param>
        /// <returns>Number of characters.</returns>
        public int CheckOrderInfoDataLength(List<EmployeeOrderDetails> listMainOrder, List<EmployeeOrderDetails> listAlternativeOrder)
        {
            int totalCharacter = 0;
            List<string> mainOrderConcatenate = listMainOrder.Select(x => x.SelectedMenuItemName).ToList();
            List<string> alternativeOrderConcatenate = listAlternativeOrder.Select(x => x.SelectedMenuItemName).ToList();

            if (mainOrderConcatenate.Any())
            {
                totalCharacter += mainOrderConcatenate.Aggregate((x, y) => x += y).Count();
            }

            if (alternativeOrderConcatenate.Any())
            {
                totalCharacter += alternativeOrderConcatenate.Aggregate((x, y) => x += y).Count();
            }

            return totalCharacter;
        }

        /// <summary>
        /// Display order details.
        /// </summary>
        /// <param name="listOrderDetails">List order details.</param>
        /// <returns>String order details.</returns>
        public string CombineDataOrderSummary(List<EmployeeOrderDetails> listOrderDetails)
        {
            string orderInfoSummary = string.Empty;
            var lastItem = listOrderDetails.Last();
            foreach (var item in listOrderDetails)
            {
                if (item.SelectedMenuItemQuantity > 1)
                {
                    orderInfoSummary += item.SelectedMenuItemQuantity.ToString()
                        + WinFormMainResource.QuantityPerOrder
                        + WinFormMainResource.Parentheses1
                        + item.SelectedMenuItemName
                        + WinFormMainResource.Parentheses2;
                }
                else
                {
                    orderInfoSummary += WinFormMainResource.Parentheses1
                         + item.SelectedMenuItemName
                         + WinFormMainResource.Parentheses2;
                }

                if (item != lastItem)
                {
                    orderInfoSummary += WinFormMainResource.CommaSeparated;
                }
            }

            return orderInfoSummary;
        }

        public void CurrentCulture()
        {
            if (string.IsNullOrEmpty(_defaultLanguage) || _defaultLanguage == Constant.DefaultCulture)
            {
                IsVnmCulture = true;
            }
            else
            {
                IsVnmCulture = false;
            }
        }

        /// <summary>
        /// Mapping data from entity to object in app.
        /// </summary>
        /// <param name="order">Data from DB.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void PrepareEditData(Order order)
        {
            if (order != null)
            {
                List<KitchenEmployeeInfo> data = new List<KitchenEmployeeInfo>();
                EmployeeInfo = GetEmployeeBySysId(order.SysId);
                LocationId = order.LocationId;
                if (IsVnmCulture)
                {
                    LocationName = order.VnmName;
                }
                else
                {
                    LocationName = order.EngName;
                }

                List<OrderMenu> orderMenus = order.ListMenu;
                if (orderMenus != null)
                {
                    List<EmployeeMappingData> listOrderDetails = orderMenus.GroupBy(x => x.SysId)
                    .Select(g => new EmployeeMappingData { SysId = g.Key, ListOrderDetails = g.ToList() }).ToList();

                    if (listOrderDetails.Count > 0)
                    {
                        foreach (EmployeeMappingData employeeMapping in listOrderDetails)
                        {
                            KitchenEmployeeInfo kitchenEmployeeInfo = new KitchenEmployeeInfo()
                            {
                                SysId = employeeMapping.SysId
                            };
                            foreach (OrderMenu orderDetail in employeeMapping.ListOrderDetails)
                            {
                                if ((orderDetail.IsMainMenu && !orderDetail.IsSideDish) || (!orderDetail.IsMainMenu && !orderDetail.IsSideDish))
                                {
                                    var employeeOrderDetails = new EmployeeOrderDetails()
                                    {
                                        SelectedMenuItemId = orderDetail.MenuId,
                                        SelectedMenuItemName = IsVnmCulture ? orderDetail.MenuVnmName : orderDetail.MenuEngName,
                                        SelectedMenuItemQuantity = orderDetail.QuantityOrder,
                                        SelectedMenuItemPrice = orderDetail.Price
                                    };

                                    if (orderDetail.IsMainMenu && !orderDetail.IsSideDish)
                                    {
                                        employeeOrderDetails.IsMainMenu = true;
                                        employeeOrderDetails.SelectedMenuItemId = int.MaxValue;
                                    }

                                    kitchenEmployeeInfo.ListMainMenuOrderDeatails.Add(employeeOrderDetails);
                                }

                                if (orderDetail.IsSideDish && !orderDetail.IsMainMenu)
                                {
                                    kitchenEmployeeInfo.ListAlternativeMenuOrderDeatails.Add(
                                       new EmployeeOrderDetails()
                                       {
                                           SelectedMenuItemId = orderDetail.MenuId,
                                           SelectedMenuItemName = IsVnmCulture ? orderDetail.MenuVnmName : orderDetail.MenuEngName,
                                           SelectedMenuItemQuantity = orderDetail.QuantityOrder,
                                           SelectedMenuItemPrice = orderDetail.Price
                                       });
                                }

                                kitchenEmployeeInfo.NickName = orderDetail.NickName;
                                kitchenEmployeeInfo.MenuVnmName = orderDetail.MenuVnmName;
                                kitchenEmployeeInfo.MenuEngName = orderDetail.MenuEngName;
                                kitchenEmployeeInfo.IsPlaceOrder = true;
                            }

                            data.Add(kitchenEmployeeInfo);
                            ChangeMenuColor(kitchenEmployeeInfo);
                            kitchenEmployeeInfo.OrderSummary = DisplayEmployeeOrderSummary(kitchenEmployeeInfo);
                        }
                    }

                    UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(data);
                    CommonKitchenOrder = this.GetAllGroup;
                    ExecutePickGroupCommand();
                    GridUserVisibility = Constant.Visible;
                    OrderNo = order.OrderNo.ToString().PadLeft(3, '0');
                    ReferenceCode = order.OrderId;
                }
            }
        }

        #endregion
    }
}
