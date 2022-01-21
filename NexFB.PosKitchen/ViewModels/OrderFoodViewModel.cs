namespace NexFB.PosKitchen.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Enums;
    using NexFB.Pos.Core.Models;
    using NexFB.PosKitchen.ViewModels.Properties.OrderFood;
    using NexFB.PosKitchen.Views;
    using NexFB.PosKitchen.Views.ManageOrder;
    using NexFB.Resources;
    using WPFMessageBoxCustomControl;
    using WPFMessageBoxCustomControl.Enums;

    public partial class OrderFoodViewModel : OrderFoodProperty
    {
        #region Fields

        private static readonly OrderFoodData _OrderFoodData = new OrderFoodData();
        private static readonly OrderFoodPreviewData _OrderFoodPreviewData = new OrderFoodPreviewData();
        private static readonly string _defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];

        #endregion

        #region Constructors

        public OrderFoodViewModel(MainViewModel mainViewModel, Order order = null)
        {
            InitializeData(mainViewModel, order);
            RegisterRelayCommands();
            GetTimerAddQuantiy();
        }

        #endregion

        #region Initialize Data

        public void InitializeData(MainViewModel mainViewModel, Order order)
        {
            base.InitializeData(mainViewModel.ScreenWidth, mainViewModel.ScreenHeight);
            ResetCurrentModel();
            CurrentCulture();
            ChangeButtonBorder((int)KitchenGroupButton.ReleaseAllButton);
            HideGridDisplayUser();
            MainViewModel = mainViewModel;

            if (order != null)
            {
                AllowReduceOrderToZero = order.CurrentStatusId != (int)OrderStatus.CanceledOrder;
                IsCanceledOrder = order.CurrentStatusId == (int)OrderStatus.CanceledOrder;
                SaveButtonVisible = Constant.Visible;
                IsDataEdit = true;
                PrepareEditData(order);
                AllObservationLocation = _OrderFoodData.GetAllLocation;
            }
            else
            {
                AllObservationLocation = _OrderFoodData.GetAllActiveLocation;
            }
        }

        #endregion

        #region Left Side Button Action

        ////Button Pick Room
        public bool CanExecutePickRoomCommand()
        {
            return EmployeeInfo != null;
        }

        public void ExecutePickRoomCommand()
        {
            ChangeButtonBorder((int)KitchenGroupButton.PickRoomButton);

            if (string.IsNullOrEmpty(LocationResult))
            {
                CommonKitchenOrder = AllObservationLocation;
            }
            else
            {
                ExecuteSearchLoction();
            }
        }

        public void ExecutePickGroupCommand()
        {
            ChangeButtonBorder((int)KitchenGroupButton.PickGroupButton);
            CommonKitchenOrder = AllObservationGroup;
        }

        public bool CanExecuteChooseMainFoodCommand()
        {
            return UserInGroup.Any(x => x.IsSelectedMemberInGroup) && CanOrderMainFood;
        }

        public void ExecuteChooseMainFoodCommand()
        {
            ChangeButtonBorder((int)KitchenGroupButton.ChooseMainFoodButton);
            CommonKitchenOrder = AllObservationFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.SingleOrDefault(x => x.IsSelectedMemberInGroup);
                ChangeMenuColor(currentSelectedMember);
            }
        }

        //// Button Choose Alternative Food 
        public bool CanExecuteChooseAlternativeFoodCommand()
        {
            return UserInGroup.Any(x => x.IsSelectedMemberInGroup);
        }

        public void ExecuteChooseAlternativeFoodCommand()
        {
            ChangeButtonBorder((int)KitchenGroupButton.ChooseAlternativeFoodButton);
            CommonKitchenOrder = AllObservationAlternativeFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.SingleOrDefault(x => x.IsSelectedMemberInGroup);
                ChangeMenuColor(currentSelectedMember);
            }
        }

        #endregion

        #region Middle Side Action

        public void ExecuteMealMenuSelected(object value)
        {
            if (value != null)
            {
                var itemId = (int)value;
                bool checkMainFoodMenu = CheckMainFoodMenu;
                var currentSelectedItem = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == itemId);
                if (checkMainFoodMenu || CheckAlternativeFoodMenu || CheckGroupMenu)
                {
                    if (CheckGroupMenu)
                    {
                        CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                        ReleaseDataMiddleSide(false, currentSelectedItem);
                        AddMemberInGroupToGrid(currentSelectedItem);
                        CurrentSelectedGroupId = itemId;
                    }
                    else
                    {
                        ReleaseDataMiddleSide(false, currentSelectedItem);
                        KitchenEmployeeInfo currentSelectedMember = UserInGroup.SingleOrDefault(x => x.IsSelectedMemberInGroup);
                        int additionalQuantity = 0;
                        if (checkMainFoodMenu)
                        {
                            RemoveMainDish(itemId, currentSelectedMember);
                            CurrentSelectedMainMenuId = itemId;
                            AddSelectedMenuItemToList(itemId, currentSelectedMember, (int)MenuType.MainMenu, additionalQuantity);
                        }
                        else
                        {
                            CurrentSelectedAlternativeMenuId = itemId;
                            AddSelectedMenuItemToList(itemId, currentSelectedMember, (int)MenuType.AlternativeMenu, additionalQuantity);
                        }
                    }
                }
                else
                {
                    //// Case Location Data.
                    ReleaseDataMiddleSide(true, currentSelectedItem);
                    this.LocationName = currentSelectedItem.ItemName;
                    this.LocationId = itemId;
                }

                LocationTextBlockHighLight(false);
                MainTextBlockHighLight(false);
            }
        }

        public void RemoveMainDish(int itemId, KitchenEmployeeInfo currentSelectedMember)
        {
            if (itemId == int.MaxValue)
            {
                //// Check for user had removed the main food menu.
                currentSelectedMember.IsDeleteMainFood = false;
            }
            else
            {
                if (currentSelectedMember.ListMainMenuOrderDeatails.Count == 1)
                {
                    var selectedMenuItem = currentSelectedMember.ListMainMenuOrderDeatails.SingleOrDefault();
                    if (selectedMenuItem.SelectedMenuItemId == int.MaxValue)
                    {
                        currentSelectedMember.ListMainMenuOrderDeatails.Remove(selectedMenuItem);
                        currentSelectedMember.DisabledAutoAddFood = true;

                        CommonKitchenOrder mainMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == int.MaxValue);
                        mainMenu.Quantity = 0;
                        mainMenu.IsSelectedMealMenu = false;
                        mainMenu.IsFocusedMealMenu = false;
                        currentSelectedMember.OrderSummary = _OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedMember);
                    }
                }
            }
        }

        #endregion

        #region Right Side Action

        public void ExecuteSelectAllUserInGroupCommand()
        {
            UserInGroup.ToList().ForEach(x => x.IsPlaceOrder = true);
            foreach (KitchenEmployeeInfo item in UserInGroup)
            {
                ChangeMenuColor(item);
            }

            if (EmployeeInfo == null)
            {
                KitchenEmployeeInfo firstEmployeeInGroup = UserInGroup.FirstOrDefault();
                EmployeeInfo = firstEmployeeInGroup;
                OwnerImage = firstEmployeeInGroup.ImageSource;
                LocationName = firstEmployeeInGroup.LocationName;
                LocationId = firstEmployeeInGroup.LocationId;
            }

            CommonKitchenOrder = AllObservationGroup;
            ChangeButtonBorder((int)KitchenGroupButton.PickGroupButton);
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
        }

        public bool CanExecuteEmployeeInGroupSelected()
        {
            return true;
        }

        public void ExecuteEmployeeInGroupSelected(object value)
        {
            if (value != null)
            {
                var sysId = (int)value;
                CheckCanOrderMainFood(UserInGroup.SingleOrDefault(x => x.SysId == sysId));

                KitchenEmployeeInfo employeeHasDeletedOrder = UserInGroup.SingleOrDefault(
                    x => (x.ListMainMenuOrderDeatails.Count == 0 && x.ListAlternativeMenuOrderDeatails.Count == 0)
                    && x.IsDeleteMainFood);
                if (employeeHasDeletedOrder != null)
                {
                    if (employeeHasDeletedOrder.SysId != sysId)
                    {
                        this.ShowErrorMessage(CurrentCulture(), WinFormMainResource.Message_Atleast);
                    }
                }
                else
                {
                    IsMenuFocused = false;
                    var currentSelectedEmployee = UserInGroup.SingleOrDefault(x => x.SysId == sysId);
                    UserInGroup.ToList().ForEach(y => y.IsSelectedMemberInGroup = false);
                    currentSelectedEmployee.IsSelectedMemberInGroup = true;
                    currentSelectedEmployee.IsPlaceOrder = true;

                    CurrentSelectedEmployeeSysId = sysId;
                    ChangeMemberOrderColor(currentSelectedEmployee);

                    if (EmployeeInfo == null)
                    {
                        EmployeeInfo = currentSelectedEmployee;
                        LocationName = currentSelectedEmployee.LocationName;
                        OwnerImage = currentSelectedEmployee.ImageSource;
                        LocationId = currentSelectedEmployee.LocationId;
                    }

                    DisabledMainFoodMenu();
                }

                LocationTextBlockHighLight(false);
                MainTextBlockHighLight(false);
            }
        }

        #endregion

        #region KeyPad Action

        public bool CanExecuteReceiveCommand()
        {
            bool hasOrderFood = UserInGroup.Any(x => x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0);
            return AllowReduceOrderToZero ? EmployeeInfo != null : EmployeeInfo != null && hasOrderFood && !IsCanceledOrder;
        }

        public void ExecuteReceiveCommand()
        {
            bool dataValid = CheckDataValid();
            if (dataValid)
            {
                ShowOverlay();
                OrderConfirmationWindow confirmationWindow = new OrderConfirmationWindow();
                List<OrderPreview> orderPreview = _OrderFoodPreviewData.PrepareDataForKitchenPreview(UserInGroup.ToList());
                OrderPreviewData = new ObservableCollection<OrderPreview>(orderPreview);
                TotalOrder = orderPreview.Sum(x => x.Quantity);
                decimal totalPrice = orderPreview.Sum(x => x.UnitPrice);
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                TotalPrice = totalPrice.ToString("#,###", culture.NumberFormat);

                confirmationWindow.DataContext = this;
                confirmationWindow.Owner = Application.Current.MainWindow;
                confirmationWindow.Show();
            }
        }

        public bool CanExecuteAddCommand()
        {
            bool isInChooseFood = this.CheckMainFoodMenu || this.CheckAlternativeFoodMenu;
            bool isMenuFocused = CommonKitchenOrder.Any(x => x.IsFocusedMealMenu);
            return !(isInChooseFood && isMenuFocused);
        }

        public void ExecuteAddCommand()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                var getEmployeeByCodeCriteria = new GetEmployeeByCodeCriteria { Code = Result };
                List<EmployeeInfo> employees = this.KitchenService.GetEmployeeOrderInfo(getEmployeeByCodeCriteria);
                List<KitchenEmployeeInfo> listKitchenEmployeeInfo = Mapper.Map<List<EmployeeInfo>, List<KitchenEmployeeInfo>>(employees);
                if (listKitchenEmployeeInfo.Any())
                {
                    if (listKitchenEmployeeInfo.Count == 1)
                    {
                        KitchenEmployeeInfo selectedEmployeeInfo = listKitchenEmployeeInfo.SingleOrDefault();
                        AddMemberToOrder(selectedEmployeeInfo);
                    }
                    else
                    {
                        this.ShowOverlay();
                        DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>(listKitchenEmployeeInfo);
                        MessageHeader = string.Format(WinFormMainResource.DuplicateEmployeeMessage, DuplicateUser.Count);
                        EmployeeSelectionWindow employeeSelectionWindow = new EmployeeSelectionWindow
                        {
                            DataContext = this,
                            Owner = Application.Current.MainWindow
                        };
                        employeeSelectionWindow.ShowDialog();
                    }
                }
                else
                {
                    this.ShowErrorMessage(CurrentCulture(), string.Format(WinFormMainResource.CannotFindEmployeeCode, Result));
                }

                Result = null;

                if (CommonKitchenOrder.Any(x => x.IsFocusedMealMenu) || UserInGroup.Any(x => x.IsSelectedMemberInGroup))
                {
                    MainTextBlockHighLight(false);
                }
            }
        }

        public bool CanExecuteCancelCommand()
        {
            return EmployeeInfo != null || CommonKitchenOrder.Count > 0 || (Result != null && Result.Any());
        }

        public void ExecuteCancelCommand(object value)
        {
            if (value != null)
            {
                if (EmployeeInfo != null || (UserInGroup.Any() && UserInGroup.Count > 0) || CommonKitchenOrder.Count > 0)
                {
                    ShowOverlay();
                    string confirmMessage = ReferenceCode == 0 ? WinFormMainResource.Message_CancelOrder : WinFormMainResource.Message_CancelOrderEdit;
                    WPFMessageBoxResult confirmCancel = WPFMessageBox.Show(
                        CurrentCulture(),
                        WinFormMainResource.Confirmation,
                        confirmMessage,
                        WPFMessageBoxButtons.YesNo,
                        WPFMessageBoxImage.Alert);
                    if (confirmCancel == WPFMessageBoxResult.Yes)
                    {
                        ResetCurrentModel(value);
                    }

                    HideOverlay();
                }
                else
                {
                    Result = string.Empty;
                }
            }
        }

        public bool CanExecuteNumPadPressed()
        {
            return !(Result != null && Result.Length == 6);
        }

        public void ExecuteNumPadPressed(object value)
        {
            var keyPadParameter = value.ToString();
            switch (keyPadParameter)
            {
                case "BACK":
                    Result = Result.Length > 0 ? Result.Remove(Result.Length - 1) : string.Empty;
                    LocationResult = LocationResult.Length > 0 ? LocationResult.Remove(LocationResult.Length - 1) : string.Empty;
                    break;

                default:
                    if (LocationTextboxResultFocused)
                    {
                        LocationResult += keyPadParameter.Substring(keyPadParameter.Length - 1, 1);
                        ExecuteSearchLoction();
                    }
                    else
                    {
                        Result += keyPadParameter.Substring(keyPadParameter.Length - 1, 1);
                        if ((CheckMainFoodMenu || CheckAlternativeFoodMenu) && CommonKitchenOrder.Any(x => x.IsFocusedMealMenu))
                        {
                            StartTimerAddQuantity();
                        }
                    }

                    break;
            }
        }

        public void ExecuteSearchLoction()
        {
            var filterList = AllObservationLocation.Where(x => x.ItemName.ToUpperInvariant().Contains(LocationResult));
            CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(filterList);
        }

        public bool CanExecuteNumPadReleased()
        {
            bool hasSelectedMemberInGroup = UserInGroup.Any(x => x.IsSelectedMemberInGroup);
            bool hasMiddleSideDataFocused = CommonKitchenOrder.Any(x => x.IsFocusedMealMenu);
            bool hasTextBlockFocused = this.MaindTextboxResultFocused && !string.IsNullOrEmpty(Result);
            bool hasSearchLocationFocused = this.LocationTextboxResultFocused && !string.IsNullOrEmpty(LocationResult);

            return hasMiddleSideDataFocused || hasSelectedMemberInGroup || hasTextBlockFocused || hasSearchLocationFocused;
        }

        public void ExecuteNumPadReleased()
        {
            var currentSelectedEmployee = UserInGroup.SingleOrDefault(x => x.SysId == CurrentSelectedEmployeeSysId);
            CommonKitchenOrder currentSelectedMenu = null;

            if (this.LocationTextboxResultFocused)
            {
                if (!string.IsNullOrEmpty(LocationResult))
                {
                    LocationResult = LocationResult.Remove(LocationResult.Length - 1);
                    ExecuteSearchLoction();
                }
                else
                {
                    CommonKitchenOrder = this.AllObservationLocation;
                }
            }
            else
            {
                if (this.MaindTextboxResultFocused)
                {
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = Result.Remove(Result.Length - 1);
                    }
                }
                else
                {
                    if (!IsMenuFocused)
                    {
                        EmployeeInListReleased(currentSelectedEmployee);
                    }
                    else
                    {
                        MiddSideDataReleased(currentSelectedMenu, currentSelectedEmployee);
                        IsMenuFocused = false;
                    }
                }
            }
        }

        #endregion

        #region Modal Confirm Action

        public void ExecuteSaveOrderCommand(object value)
        {
            var mainMenuData = new StringBuilder(Constant.BeginRootTag);
            var alternativeMenuData = new StringBuilder(Constant.BeginRootTag);
            var additionalMenuData = new StringBuilder(Constant.BeginRootTag);
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
                            mainMenuData.AppendFormat(Constant.MainMenuTemplate, user.SysId, Constant.DefaultMainFoodValue, mainOrder.SelectedMenuItemQuantity);
                        }
                        else
                        {
                            alternativeMenuData.AppendFormat(Constant.AlternativeMenuTemplate, user.SysId, mainOrder.SelectedMenuItemId, mainOrder.SelectedMenuItemQuantity);
                        }
                    }
                }

                if (additionalOrders != null && additionalOrders.Count > 0)
                {
                    additionalOrders.ForEach(x => additionalMenuData.AppendFormat(Constant.AdditionalMenuTemplate, user.SysId, x.SelectedMenuItemId, x.SelectedMenuItemQuantity));
                }
            }

            AddOrEditOrderData(value, mainMenuData, alternativeMenuData, additionalMenuData);
        }

        public void ExecuteSaveDialogCommand(object value)
        {
            if (DuplicateUser.Any(x => x.IsChecked))
            {
                KitchenEmployeeInfo selectedEmployee = DuplicateUser.FirstOrDefault(x => x.IsChecked);
                if (EmployeeInfo == null)
                {
                    EmployeeInfo = selectedEmployee;
                    OwnerImage = EmployeeInfo.ImageSource;
                    LocationName = EmployeeInfo.LocationName;
                    LocationId = EmployeeInfo.LocationId;
                    GetEmployeeInGroup(EmployeeInfo.SysId, value);
                }
                else
                {
                    GetEmployeeInGroup(DuplicateUser.FirstOrDefault(x => x.IsChecked).SysId, value);
                }
            }
        }

        public void ExecuteCancelDialogCommand(object value)
        {
            CloseDialog(value);
        }

        public void ExecuteCancelDialogOrderConfirmation(object value)
        {
            if (value != null)
            {
                var currentWindow = (OrderConfirmationWindow)value;
                currentWindow.Close();
                HideOverlay();
            }
        }

        public void ExecuteCloseVirtualKeyboard(object value)
        {
            if (value != null)
            {
                var currentWindow = (VirtualKeyboard)value;
                currentWindow.Close();
                FullKeyPadVisible = false;
                SearchLocationKeyboard = Constant.Hidden;
                LocationTextBlockHighLight(false);
            }
        }

        #endregion

        #region Public Methods

        public void AddMemberToOrder(KitchenEmployeeInfo selectedEmployeeInfo)
        {
            if (EmployeeInfo == null)
            {
                EmployeeInfo = selectedEmployeeInfo;
                OwnerImage = selectedEmployeeInfo.ImageSource;
                LocationName = selectedEmployeeInfo.LocationName;
                LocationId = selectedEmployeeInfo.LocationId;
            }

            bool result = GetEmployeeInGroup(selectedEmployeeInfo.SysId);
            if (result)
            {
                if (CanOrderMainFood)
                {
                    ChangeButtonBorder((int)KitchenGroupButton.ChooseMainFoodButton);
                    CommonKitchenOrder = AllObservationFood;
                }
                else
                {
                    ChangeButtonBorder((int)KitchenGroupButton.ChooseAlternativeFoodButton);
                    CommonKitchenOrder = AllObservationAlternativeFood;
                }
            }
        }

        public void ChangeMenuColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            CheckCanOrderMainFood(currentSelectedEmployee);
            bool checkMainFoodMenu = CheckMainFoodMenu;
            bool hasNotOrderMainFood = !currentSelectedEmployee.ListMainMenuOrderDeatails.Any(x => x.SelectedMenuItemId == int.MaxValue);
            bool isAutoAddMainFood = hasNotOrderMainFood && !IsDataEdit && !currentSelectedEmployee.DisabledAutoAddFood && CanOrderMainFood;
            if (isAutoAddMainFood)
            {
                currentSelectedEmployee = AutoAddMainFoodWhenFocused(currentSelectedEmployee);
            }

            if (CheckGroupMenu)
            {
                CommonKitchenOrder.ToList().ForEach(x => { x.IsFocusedMealMenu = false; });
            }

            if (checkMainFoodMenu || CheckAlternativeFoodMenu)
            {
                AddFixedMenuToEmployee(currentSelectedEmployee, checkMainFoodMenu);
                CommonKitchenOrder.ToList().ForEach(x =>
                    {
                        x.IsFocusedMealMenu = false;
                        x.IsSelectedMealMenu = false;
                        x.Quantity = 0;
                    });
                List<EmployeeOrderDetails> employeeOrderDetails = null;
                employeeOrderDetails = checkMainFoodMenu ? currentSelectedEmployee.ListMainMenuOrderDeatails : currentSelectedEmployee.ListAlternativeMenuOrderDeatails;

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
            }
            else
            {
                DisabledMainFoodMenu();
            }

            CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
        }

        public void DisabledMainFoodMenu()
        {
            if (CanOrderMainFood)
            {
                ExecuteChooseMainFoodCommand();
            }
            else
            {
                ExecuteChooseAlternativeFoodCommand();
            }
        }

        public void AddSelectedMenuItemToList(int itemId, KitchenEmployeeInfo currentSelectedMember, int menu, int increaseValue)
        {
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                EmployeeOrderDetails employeeOrderDetails = null;
                List<EmployeeOrderDetails> currentMenuList = null;
                var orderDetails = new EmployeeOrderDetails
                {
                    SelectedMenuItemId = itemId,
                    SelectedMenuItemQuantity = 1,
                };

                switch (menu)
                {
                    case (int)MenuType.MainMenu:
                        currentMenuList = currentSelectedMember.ListMainMenuOrderDeatails;
                        employeeOrderDetails = currentMenuList.SingleOrDefault(x => x.SelectedMenuItemId == itemId);
                        break;
                    case (int)MenuType.AlternativeMenu:
                        currentMenuList = currentSelectedMember.ListAlternativeMenuOrderDeatails;
                        employeeOrderDetails = currentMenuList.SingleOrDefault(x => x.SelectedMenuItemId == itemId);
                        break;
                    default:
                        // do nothing
                        break;
                }

                CommonKitchenOrder selectedMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == itemId);
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
                            employeeOrderDetails.SelectedMenuItemQuantity = increaseValue;
                            selectedMenu.Quantity = employeeOrderDetails.SelectedMenuItemQuantity;
                        }
                    }
                }

                currentSelectedMember.OrderSummary = _OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedMember);
                UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
            }
        }

        public List<KitchenEmployeeInfo> GetUserByGroup(int groupId)
        {
            var getUserByGroupIdCriteria = new GetUserByGroupIdCriteria { GroupOrderId = groupId };
            List<EmployeeInfo> users = this.KitchenService.GetUserByGroupId(getUserByGroupIdCriteria);
            List<KitchenEmployeeInfo> data = Mapper.Map<List<EmployeeInfo>, List<KitchenEmployeeInfo>>(users);
            return data;
        }

        public void CloseDialog(object value)
        {
            if (value != null)
            {
                var currentWindow = (EmployeeSelectionWindow)value;
                currentWindow.Close();
                HideOverlay();
            }
        }

        public bool GetEmployeeInGroup(int sysId, object value = null)
        {
            if (!UserInGroup.Any(x => x.SysId == sysId))
            {
                KitchenEmployeeInfo employee = this.GetEmployeeBySysId(sysId);
                var getEmployeeOrderStatusCriteria = new GetEmployeeOrderStatusCriteria { SysId = employee.SysId };
                EmployeeOrderStatus employeeStatus = this.KitchenService.EmployeeOrderStatus(getEmployeeOrderStatusCriteria);
                employee.MenuId = employeeStatus.MenuId;
                employee.MenuVnmName = employeeStatus.MenuVnmName;
                employee.MenuEngName = employeeStatus.MenuEngName;

                CheckCanOrderMainFood(employee);
                if (employee != null)
                {
                    UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                    employee.IsSelectedMemberInGroup = true;
                    employee.IsPlaceOrder = true;
                    UserInGroup.Add(employee);
                    ShowGridDisplayUser();
                    CloseEmployeeSelectionDialog(value);
                }

                CurrentSelectedEmployeeSysId = sysId;
                DisabledMainFoodMenu();
                ////ChangeMenuColor(employee);
            }
            else
            {
                this.ShowErrorMessage(CurrentCulture(), WinFormMainResource.Message_EmployeeExist);
                return false;
            }

            return true;
        }

        public void CloseEmployeeSelectionDialog(object value)
        {
            if (value != null)
            {
                CloseDialog(value);
            }
        }

        public void AddMemberInGroupToGrid(CommonKitchenOrder currentSelectedItem)
        {
            if (CheckGroupMenu)
            {
                if (currentSelectedItem.IsSelectedMealMenu)
                {
                    PrepareDataEmployeeInGroup(currentSelectedItem.ItemId);
                }
                else
                {
                    //// Unselected item in group.
                    if (UserInGroup != null)
                    {
                        UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup.Where(x => x.IsPlaceOrder));
                        List<CommonKitchenOrder> selectedGroups = CommonKitchenOrder.Where(x => x.IsSelectedMealMenu).ToList();
                        foreach (var group in selectedGroups)
                        {
                            PrepareDataEmployeeInGroup(group.ItemId);
                        }

                        UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
                    }
                }

                if (UserInGroup != null)
                {
                    GridUserVisibility = UserInGroup.Count > 0 ? Constant.Visible : Constant.Hidden;
                }
            }
        }

        public bool CheckValidEmployeeData(int sysId, object value = null)
        {
            KitchenEmployeeInfo currentSelectedEmployee = UserInGroup.SingleOrDefault(x => x.SysId == sysId);
            CheckCanOrderMainFood(currentSelectedEmployee);
            if (CanOrderMainFood)
            {
                if (currentSelectedEmployee != null)
                {
                    currentSelectedEmployee.IsPlaceOrder = true;
                    currentSelectedEmployee.IsSelectedMemberInGroup = true;
                    CloseEmployeeSelectionDialog(value);
                    ChangeMenuColor(currentSelectedEmployee);
                    return true;
                }
                else
                {
                    ResetCurrentModel();
                    this.ShowErrorMessage(CurrentCulture(), WinFormMainResource.Message_Unhandled);
                    return false;
                }
            }
            else
            {
                currentSelectedEmployee.IsPlaceOrder = true;
                currentSelectedEmployee.IsSelectedMemberInGroup = true;
                ChangeMenuColor(currentSelectedEmployee);
                return false;
            }
        }

        public void ExecuteChangeKeyPad()
        {
            if (!FullKeyPadVisible)
            {
                var virtalKeyboard = new VirtualKeyboard
                {
                    DataContext = this,
                    Owner = Application.Current.MainWindow,
                    Top = VirtualKeyboardAnimation
                };
                virtalKeyboard.Show();
                FullKeyPadVisible = true;
            }
        }

        public void ShowSearchLocationKeyBoard()
        {
            SearchLocationKeyboard = Constant.Visible;
            ExecuteChangeKeyPad();
        }

        public void MiddSideDataReleased(CommonKitchenOrder currentSelectedMenu, KitchenEmployeeInfo currentSelectedEmployee)
        {
            EmployeeOrderDetails menuRemoved = null;
            if (CheckMainFoodMenu || CheckAlternativeFoodMenu)
            {
                if (CheckMainFoodMenu)
                {
                    if (CurrentSelectedMainMenuId > 0)
                    {
                        currentSelectedMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == CurrentSelectedMainMenuId);
                        menuRemoved = currentSelectedEmployee.ListMainMenuOrderDeatails.SingleOrDefault(x => x.SelectedMenuItemId == CurrentSelectedMainMenuId);
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
                        currentSelectedMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == CurrentSelectedAlternativeMenuId);
                        menuRemoved = currentSelectedEmployee.ListAlternativeMenuOrderDeatails.SingleOrDefault(x => x.SelectedMenuItemId == CurrentSelectedAlternativeMenuId);
                        if (menuRemoved != null)
                        {
                            currentSelectedEmployee.ListAlternativeMenuOrderDeatails.Remove(menuRemoved);
                        }
                    }
                }

                currentSelectedEmployee.OrderSummary = _OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedEmployee);
                UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
                currentSelectedMenu.Quantity = 0;
                ChangeMenuColor(currentSelectedEmployee);
                if (currentSelectedEmployee.ListMainMenuOrderDeatails.Count == 0 && currentSelectedEmployee.ListAlternativeMenuOrderDeatails.Count == 0)
                {
                    EmployeeInListReleased(currentSelectedEmployee);
                }
            }
            else
            {
                if (CurrentSelectedGroupId > 0)
                {
                    currentSelectedMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == CurrentSelectedGroupId);
                    currentSelectedMenu.IsFocusedMealMenu = false;
                    currentSelectedMenu.IsSelectedMealMenu = false;
                    AddMemberInGroupToGrid(currentSelectedMenu);
                }
            }
        }

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
                currentSelectedEmployee.DisabledAutoAddFood = false;
                ChangeButtonBorder((int)KitchenGroupButton.PickGroupButton);
                ChangeMemberOrderColor(currentSelectedEmployee);
                CommonKitchenOrder = AllObservationGroup;
            }
        }

        public void ChangeMemberOrderColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            if (currentSelectedEmployee.IsPlaceOrder)
            {
                currentSelectedEmployee.EmployeeOrderMealColor = currentSelectedEmployee.IsVegetarian ?
                    Constant.RegisterVegetarianMealColor : Constant.RegisterNonVegetarianMealColor;
            }
            else
            {
                currentSelectedEmployee.EmployeeOrderMealColor = currentSelectedEmployee.Quantity > 0 ?
                    Constant.HasOrderMealBeforeColor : Constant.UNRegisterMealColor;
            }
        }

        public void AddFixedMenuToEmployee(KitchenEmployeeInfo currentSelectedEmployee, bool isMainFoodMenu)
        {
            if (isMainFoodMenu)
            {
                string mealName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
                CommonKitchenOrder dynamicMainFoodMenu = null;

                if (!CommonKitchenOrder.Any(x => x.ItemId == int.MaxValue))
                {
                    dynamicMainFoodMenu = new CommonKitchenOrder
                    {
                        ItemId = int.MaxValue,
                        IsSelectedMealMenu = false,
                        IsMainMenu = true,
                        ItemName = mealName,
                    };
                    CommonKitchenOrder.Insert(0, dynamicMainFoodMenu);
                }
                else
                {
                    dynamicMainFoodMenu = CommonKitchenOrder.SingleOrDefault(x => x.ItemId == int.MaxValue);
                }

                CheckExistingFixedMenu(currentSelectedEmployee, dynamicMainFoodMenu);
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

        public KitchenEmployeeInfo GetEmployeeBySysId(int sysId)
        {
            var criteria = new GetEmployeeBySysIdCriteria { SysId = sysId };
            KitchenEmployeeInfo kitchenEmployee = null;
            EmployeeInfo employee = this.KitchenService.GetEmployeeBySysId(criteria);
            if (employee != null)
            {
                kitchenEmployee = MappingEmployeeData(employee);
            }

            return kitchenEmployee;
        }

        public void AddOrEditOrderData(object value, StringBuilder mainMenuData, StringBuilder alternativeMenuData, StringBuilder additionalMenuData)
        {
            if (mainMenuData.Length == 6)
            {
                mainMenuData.Length = 0;
            }

            if (alternativeMenuData.Length == 6)
            {
                alternativeMenuData.Length = 0;
            }

            if (additionalMenuData.Length == 6)
            {
                additionalMenuData.Length = 0;
            }

            if (ReferenceCode == 0)
            {
                InsertNewOrder(LocationId, mainMenuData, alternativeMenuData, additionalMenuData, value);
            }
            else
            {
                EditOrder(LocationId, mainMenuData, alternativeMenuData, additionalMenuData, value);
            }
        }

        public void EditOrder(int locationId, StringBuilder mainMenuData, StringBuilder alternativeMenuData, StringBuilder additionalMenuData, object value)
        {
            var editOrderMealCriteria = new EditOrderMealCriteria
            {
                OwnerId = EmployeeInfo.SysId,
                ModifiedDate = DateTime.Now,
                LocationId = locationId,
                Menus = mainMenuData.Length > 0 ? mainMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                Alternative = alternativeMenuData.Length > 0 ? alternativeMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                Foods = additionalMenuData.Length > 0 ? additionalMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                OrderId = ReferenceCode,
                TakeAway = false
            };
            CommandResult result = KitchenService.EditOrderFood(editOrderMealCriteria);
            if (result.Code != 0)
            {
                WPFMessageBox.Show(CurrentCulture(), result.Code == 106 ? WinFormMainResource.Message_DeliveryArea : result.Message);
            }
            else
            {
                ExecuteCancelDialogOrderConfirmation(value);
                ExecutePrintInvoice(ReferenceCode, (int)InvoiceNumberStatus.DeliverOrderFoodInvoice, false);
                this.MainViewModel.SendEditOrder(ReferenceCode);
                ResetCurrentModel();
            }
        }

        public void InsertNewOrder(int locationId, StringBuilder mainMenuData, StringBuilder alternativeMenuData, StringBuilder additionalMenuData, object value)
        {
            var insertOrderMealCriteria = new InsertOrderMealCriteria
            {
                OwnerId = EmployeeInfo.SysId,
                CreatedDate = DateTime.Now,
                LocationId = locationId,
                Menus = mainMenuData.Length > 0 ? mainMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                Alternative = alternativeMenuData.Length > 0 ? alternativeMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                Foods = additionalMenuData.Length > 0 ? additionalMenuData.Append(Constant.EndRootTag).ToString() : string.Empty,
                TakeAway = false
            };

            OrderResult result = KitchenService.InsertOrderFood(insertOrderMealCriteria);
            if (result.CommandResult.Code != 0)
            {
                WPFMessageBox.Show(CurrentCulture(), result.CommandResult.Code == 106 ? WinFormMainResource.Message_DeliveryArea : result.CommandResult.Message);
            }
            else
            {
                ExecuteCancelDialogOrderConfirmation(value);
                ExecutePrintInvoice(result.OrderId, (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice, true);
                this.MainViewModel.SendNewOrder(result.OrderId);
                ResetCurrentModel();
            }
        }

        public bool CheckDataValid()
        {
            if (EmployeeInfo != null && LocationId < 1)
            {
                this.ShowErrorMessage(CurrentCulture(), WinFormMainResource.Message_PickRoom);
                return false;
            }

            if (!AllowReduceOrderToZero && UserInGroup != null && UserInGroup.Count > 0
                && !UserInGroup.Any(x => x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0))
            {
                this.ShowErrorMessage(CurrentCulture(), WinFormMainResource.Message_Atleast1);
                return false;
            }

            return true;
        }

        public void CheckCanOrderMainFood(KitchenEmployeeInfo selectedEmploye)
        {
            CanOrderMainFood = selectedEmploye.MenuId != Constant.DisabledEmployeeOrder;
        }

        public void ResetCurrentModel(object value = null)
        {
            CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>();
            DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
            AllObservationFood = _OrderFoodData.GetAllFood;
            AllObservationAlternativeFood = _OrderFoodData.GetAllAlternativeFood;
            AllObservationGroup = _OrderFoodData.GetAllGroup;
            AllObservationLocation = _OrderFoodData.GetAllLocation;
            EmployeeInfo = null;
            Result = null;
            LocationName = null;
            OwnerImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constant.DefaultAvatar);
            LocationId = 0;
            MessageHeader = string.Empty;
            BorderButtonMainFood = Constant.DefaultMenuColor;
            BorderButtonPickGroup = Constant.DefaultMenuColor;
            BorderButtonPickRoom = Constant.DefaultMenuColor;
            BorderButtonAlternativeFood = Constant.DefaultMenuColor;
            CurrentSelectedAlternativeMenuId = 0;
            CurrentSelectedEmployeeSysId = 0;
            CurrentSelectedMainMenuId = 0;
            CurrentSelectedGroupId = 0;
            SearchLocationVisible = Constant.Hidden;
            LocationContentMargin = Constant.OtherContentMargin;
            IsMenuFocused = false;
            MaindTextboxResultFocused = false;
            LocationTextboxResultFocused = false;
            IsCanceledOrder = false;
            OrderNo = string.Empty;
            ReferenceCode = 0;
            //// Change Text From Nhận to Lưu
            SaveButtonVisible = Constant.Hidden;
            OverlayVisible = Constant.Hidden;
            SearchLocationKeyboard = Constant.Hidden;
            IsDataEdit = false;
            AllowReduceOrderToZero = false;
            HideGridDisplayUser();
            if (value != null)
            {
                Border textBlockBorder = (Border)value;
                textBlockBorder.BorderBrush = Brushes.Silver;
            }
        }

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

        public void PrepareEditData(Order order)
        {
            if (order != null)
            {
                var data = new List<KitchenEmployeeInfo>();
                EmployeeInfo = GetEmployeeBySysId(order.SysId);
                OwnerImage = EmployeeInfo.ImageSource;
                LocationId = order.LocationId;
                LocationName = IsVnmCulture ? order.VnmName : order.EngName;
                List<OrderMenu> orderMenus = order.ListMenu.Where(x => !x.IsDeleted).ToList();

                if (orderMenus != null)
                {
                    List<EmployeeMappingData> listOrderDetails = orderMenus.GroupBy(x => x.SysId)
                    .Select(g => new EmployeeMappingData { SysId = g.Key, ListOrderDetails = g.ToList() }).ToList();

                    if (listOrderDetails.Count > 0)
                    {
                        foreach (EmployeeMappingData employeeMapping in listOrderDetails)
                        {
                            List<OrderMenu> orderData = employeeMapping.ListOrderDetails.Where(x => !x.IsDeleted).ToList();
                            KitchenEmployeeInfo kitchenEmployeeInfo = MappingDataForEditAction(orderData, employeeMapping.SysId);

                            data.Add(kitchenEmployeeInfo);
                            ChangeMenuColor(kitchenEmployeeInfo);
                            kitchenEmployeeInfo.OrderSummary = _OrderFoodData.DisplayEmployeeOrderSummary(kitchenEmployeeInfo);
                        }
                    }

                    IsDataEdit = false;
                    UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(data);
                    CommonKitchenOrder = _OrderFoodData.GetAllGroup;
                    LocationId = order.LocationId;
                    ExecutePickGroupCommand();
                    ShowGridDisplayUser();
                    OrderNo = order.OrderNo.ToString().PadLeft(3, '0');
                    ReferenceCode = order.OrderId;
                }
            }
        }

        public void MainTextBlockHighLight(bool isFocused)
        {
            if (MainTexboxResultBorder != null)
            {
                if (isFocused)
                {
                    MainTexboxResultBorder.BorderBrush = Brushes.Red;
                    MaindTextboxResultFocused = true;
                }
                else
                {
                    MainTexboxResultBorder.BorderBrush = Brushes.Silver;
                    MaindTextboxResultFocused = false;
                }
            }
        }

        public void LocationTextBlockHighLight(bool isFocused)
        {
            if (LocationTexboxResultBorder != null)
            {
                if (isFocused)
                {
                    LocationTexboxResultBorder.BorderBrush = Brushes.Red;
                    LocationTextboxResultFocused = true;
                }
                else
                {
                    LocationTexboxResultBorder.BorderBrush = Brushes.Silver;
                    LocationTextboxResultFocused = false;
                    CloseFullKeyPadWhenVisible();
                }
            }
        }

        private void CloseFullKeyPadWhenVisible()
        {
            if (Application.Current.Windows.Count > 1)
            {
                WindowCollection windows = Application.Current.Windows;
                foreach (Window item in windows)
                {
                    if (item.GetType().Name == "VirtualKeyboard")
                    {
                        item.Close();
                        FullKeyPadVisible = false;
                        SearchLocationKeyboard = Constant.Hidden;
                    }
                }
            }
        }

        private KitchenEmployeeInfo MappingDataForEditAction(List<OrderMenu> orderData, int sysId)
        {
            KitchenEmployeeInfo kitchenEmployeeInfo = GetEmployeeBySysId(sysId);
            foreach (OrderMenu orderDetail in orderData)
            {
                if ((orderDetail.IsMainMenu && !orderDetail.IsSideDish) || (!orderDetail.IsMainMenu && !orderDetail.IsSideDish))
                {
                    var employeeOrderDetails = new EmployeeOrderDetails
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
                        kitchenEmployeeInfo.MenuEngName = orderDetail.MenuEngName;
                        kitchenEmployeeInfo.MenuVnmName = orderDetail.MenuVnmName;
                    }

                    kitchenEmployeeInfo.ListMainMenuOrderDeatails.Add(employeeOrderDetails);
                }

                if (orderDetail.IsSideDish && !orderDetail.IsMainMenu)
                {
                    kitchenEmployeeInfo.ListAlternativeMenuOrderDeatails.Add(
                       new EmployeeOrderDetails
                       {
                           SelectedMenuItemId = orderDetail.MenuId,
                           SelectedMenuItemName = IsVnmCulture ? orderDetail.MenuVnmName : orderDetail.MenuEngName,
                           SelectedMenuItemQuantity = orderDetail.QuantityOrder,
                           SelectedMenuItemPrice = orderDetail.Price
                       });
                }

                kitchenEmployeeInfo.NickName = orderDetail.NickName;
                kitchenEmployeeInfo.IsPlaceOrder = true;
                kitchenEmployeeInfo.DisabledAutoAddFood = true;
                kitchenEmployeeInfo.IsVegetarian = orderDetail.IsVegetarian;
            }

            return kitchenEmployeeInfo;
        }

        private void TimerTickToAddQuantity(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            var currentSelectedItem = CommonKitchenOrder.SingleOrDefault(x => x.IsFocusedMealMenu && x.IsSelectedMealMenu);
            if (currentSelectedItem != null)
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.SingleOrDefault(x => x.IsSelectedMemberInGroup);
                bool isNumeric = int.TryParse(Result, out int moreQuantity);
                if (isNumeric)
                {
                    AddSelectedMenuItemToList(
                        currentSelectedItem.ItemId,
                        currentSelectedMember,
                        CheckMainFoodMenu ? (int)MenuType.MainMenu : (int)MenuType.AlternativeMenu,
                        moreQuantity);
                }
            }

            Result = string.Empty;
            timer.Stop();
        }

        private void PrepareDataEmployeeInGroup(int groupId)
        {
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup.Where(x => x.IsPlaceOrder));
            List<KitchenEmployeeInfo> employees = GetUserByGroup(groupId);
            if (employees.Any() && employees.Count > 0)
            {
                List<int> listSysId = UserInGroup.Select(x => x.SysId).ToList();
                employees = employees.Where(x => !listSysId.Contains(x.SysId)).ToList();
                employees.ForEach(x => UserInGroup.Add(x));
                ShowGridDisplayUser();
                UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
            }
        }

        private void StartTimerAddQuantity()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(TimerAddQuantity)
            };
            timer.Tick += TimerTickToAddQuantity;
            timer.Start();
        }

        #endregion

        #region Private Methods

        private static KitchenEmployeeInfo MappingEmployeeData(EmployeeInfo employeeInfo)
        {
            return employeeInfo != null ? Mapper.Map<EmployeeInfo, KitchenEmployeeInfo>(employeeInfo) : null;
        }

        private int CurrentCulture()
        {
            if (string.IsNullOrEmpty(_defaultLanguage) || _defaultLanguage == Constant.DefaultCulture)
            {
                IsVnmCulture = true;
                return (int)LanguageLocalization.VietNamCulture;
            }
            else
            {
                IsVnmCulture = false;
                return (int)LanguageLocalization.EnglishCulture;
            }
        }

        private KitchenEmployeeInfo AutoAddMainFoodWhenFocused(KitchenEmployeeInfo currentSelectedEmployee)
        {
            if (!currentSelectedEmployee.IsDeleteMainFood)
            {
                string mealName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
                if (string.IsNullOrEmpty(mealName))
                {
                    mealName = Constant.FixedMeal;
                }

                var employeeMainMeal = new EmployeeOrderDetails
                {
                    SelectedMenuItemId = int.MaxValue,
                    SelectedMenuItemName = mealName,
                    SelectedMenuItemQuantity = 1,
                    SelectedMenuItemPrice = 0,
                    IsMainMenu = true
                };

                currentSelectedEmployee.ListMainMenuOrderDeatails.Add(employeeMainMeal);
                currentSelectedEmployee.OrderSummary = _OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedEmployee);
            }

            return currentSelectedEmployee;
        }

        private void RegisterRelayCommands()
        {
            RegisterNumPadRelayCommand();

            DefaulMenuSelected = new RelayCommand((y) => ExecuteMealMenuSelected(y));
            DefaultEmployeeSelected = new RelayCommand((x) => CanExecuteEmployeeInGroupSelected(), (y) => ExecuteEmployeeInGroupSelected(y));

            SaveDialogCommand = new RelayCommand((y) => ExecuteSaveDialogCommand(y));
            CancelDialogCommand = new RelayCommand((y) => ExecuteCancelDialogCommand(y));
            SelectAllUserInGroup = new RelayCommand((y) => ExecuteSelectAllUserInGroupCommand());
            CancelDialogOrderConfirmation = new RelayCommand((y) => ExecuteCancelDialogOrderConfirmation(y));
            SaveOrderCommand = new RelayCommand((y) => ExecuteSaveOrderCommand(y));
            ChangeKeyPad = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteChangeKeyPad());
            CloseVirtualKeyboard = new RelayCommand((x) => ExecuteCloseVirtualKeyboard(x));
        }

        private void RegisterNumPadRelayCommand()
        {
            AddCommand = new RelayCommand((x) => CanExecuteAddCommand(), (y) => ExecuteAddCommand());
            CancelCommand = new RelayCommand((x) => CanExecuteCancelCommand(), (y) => ExecuteCancelCommand(y));
            ReceiveCommand = new RelayCommand((x) => CanExecuteReceiveCommand(), (y) => ExecuteReceiveCommand());
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadReleased());

            PickRoomCommand = new RelayCommand((x) => CanExecutePickRoomCommand(), (y) => ExecutePickRoomCommand());
            PickGroupCommand = new RelayCommand((y) => ExecutePickGroupCommand());
            ChooseMainFoodCommand = new RelayCommand((x) => CanExecuteChooseMainFoodCommand(), (y) => ExecuteChooseMainFoodCommand());
            ChooseAlternativeFoodCommand = new RelayCommand((x) => CanExecuteChooseAlternativeFoodCommand(), (y) => ExecuteChooseAlternativeFoodCommand());
        }

        private void GetTimerAddQuantiy()
        {
            string timerAddQuantity = ConfigurationManager.AppSettings[Constant.TimerAddQuantity];
            if (!string.IsNullOrEmpty(timerAddQuantity))
            {
                try
                {
                    TimerAddQuantity = double.Parse(timerAddQuantity);
                }
                catch (Exception)
                {
                    TimerAddQuantity = 1;
                }
            }
        }

        #endregion
    }
}