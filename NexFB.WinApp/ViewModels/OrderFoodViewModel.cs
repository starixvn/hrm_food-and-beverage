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
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;
    using AutoMapper;
    using BlogsPrajeesh.BlogSpot.WPFControls;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.Employee;
    using NexFB.Criteria.Kitchen;
    using NexFB.Models.Kitchen;
    using NexFB.Resources;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Models;
    using NexFB.WinApp.ViewModels.Partials.OrderFood;
    using NexFB.WinApp.Views;
    using NexFB.WinApp.Views.ManageOrder;

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public partial class OrderFoodViewModel : OrderFoodProperty
    {
        #region Fields

        private const string _AdditionalMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";
        private const string _MainMenuTemplate = "<User SysId='{0}' MenuId='{1}' Qty='{2}'></User>";
        private const string _AlternativeMenuTemplate = "<User SysId='{0}' FoodId='{1}' Qty='{2}'></User>";
        private const string _BeginRootTag = "<ROOT>";
        private const string _EndRootTag = "</ROOT>";
        private static readonly OrderFoodData OrderFoodData = new OrderFoodData();
        private static readonly OrderFoodPreviewData OrderFoodPreviewData = new OrderFoodPreviewData();
        private static string _defaultLanguage = ConfigurationManager.AppSettings[Constant.DefaultLanguage];

        #endregion

        #region Constructors
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed. Suppression is OK here.")]
        public OrderFoodViewModel(MainViewModel mainViewModel, Order order = null)
        {
            InitializeData(mainViewModel, order);
            RegisterRelayCommands();
        }

        #endregion

        #region Initialize Data

        /// <summary>
        /// Initialize Data.
        /// </summary>
        /// <param name="mainViewModel">Main View Model.</param>
        /// <param name="order">Order from edit screen.</param>
        public void InitializeData(MainViewModel mainViewModel, Order order)
        {
            base.InitializeData();
            ResetCurrentModel();
            CurrentCulture();
            ChangeButtonBorder(Constant.ReleaseAllButton);
            HideGridDisplayUser();
            AllObservationFood = OrderFoodData.GetAllFood;
            AllObservationAlternativeFood = OrderFoodData.GetAllAlternativeFood;
            AllObservationLocation = OrderFoodData.GetAllLocation;
            AllObservationGroup = OrderFoodData.GetAllGroup;
            MainViewModel = mainViewModel;

            if (order != null)
            {
                SaveButtonVisible = Constant.Visible;
                IsDataEdit = true;
                PrepareEditData(order);
            }
        }

        #endregion

        #region Left Side Button Action

        ////Button Pick Room
        public bool CanExecutePickRoomCommand()
        {
            if (EmployeeInfo != null)
            {
                return true;
            }

            return false;
        }

        public void ExecutePickRoomCommand()
        {
            ChangeButtonBorder(Constant.PickRoomButton);
            CommonKitchenOrder = AllObservationLocation;
        }

        ////Button Pick Group
        public bool CanExecutePickGroupCommand()
        {
            return true;
        }

        public void ExecutePickGroupCommand()
        {
            ChangeButtonBorder(Constant.PickGroupButton);
            CommonKitchenOrder = AllObservationGroup;
        }

        /// <summary>
        /// Choose Main Food Command.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CanExecuteChooseMainFoodCommand()
        {
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Choose Main Food command.
        /// </summary>
        public void ExecuteChooseMainFoodCommand()
        {
            ChangeButtonBorder(Constant.ChooseMainFoodButton);
            CommonKitchenOrder = AllObservationFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                ChangeMenuColor(currentSelectedMember);
            }
        }

        //// Button Choose Alternative Food 
        public bool CanExecuteChooseAlternativeFoodCommand()
        {
            return CanExecuteChooseMainFoodCommand();
        }

        /// <summary>
        /// Choose Alternative Food command.
        /// </summary>
        public void ExecuteChooseAlternativeFoodCommand()
        {
            ChangeButtonBorder(Constant.ChooseAlternativeFoodButton);
            CommonKitchenOrder = AllObservationAlternativeFood;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                ChangeMenuColor(currentSelectedMember);
            }
        }

        #endregion

        #region Middle Side Action

        public bool CanExecuteMealMenuSelected()
        {
            return true;
        }

        /// <summary>
        /// Execute Meal Menu Selected.
        /// </summary>
        /// <param name="value">Value pass from user interface.</param>
        public void ExecuteMealMenuSelected(object value)
        {
            if (value != null)
            {
                var itemId = (int)value;
                bool checkMainFoodMenu = CheckMainFoodMenu;
                var currentSelectedItem = CommonKitchenOrder.Where(x => x.ItemId == itemId).SingleOrDefault();

                if (checkMainFoodMenu || CheckAlternativeFoodMenu || CheckGroupMenu)
                {
                    ReleaseDataMiddleSide(false, currentSelectedItem);
                    if (CheckGroupMenu)
                    {
                        AddMemberInGroupToGrid(currentSelectedItem);
                        CurrentSelectedGroupId = itemId;
                    }
                    else
                    {
                        KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                        int additionalQuantity = 0;
                        if (checkMainFoodMenu)
                        {
                            if (itemId == int.MaxValue)
                            {
                                //// Check for user used to deleted main food menu.
                                currentSelectedMember.IsDeleteMainFood = false;
                            }

                            CurrentSelectedMainMenuId = itemId;
                            AddSelectedMenuItemToList(itemId, currentSelectedMember, Constant.MainMenuId, additionalQuantity);
                        }
                        else
                        {
                            CurrentSelectedAlternativeMenuId = itemId;
                            AddSelectedMenuItemToList(itemId, currentSelectedMember, Constant.AlternativeMenuId, additionalQuantity);
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
            }
        }

        #endregion

        #region Right Side Action

        public bool CanExecuteSelectAllUserInGroupCommand()
        {
            return true;
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

        public bool CanExecuteEmployeeInGroupSelected()
        {
            return true;
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
                KitchenEmployeeInfo previousEmployee = UserInGroup.Where(x => (x.ListMainMenuOrderDeatails.Count == 0 && x.ListAlternativeMenuOrderDeatails.Count == 0)
                    && x.IsDeleteMainFood).SingleOrDefault();
                if (previousEmployee != null)
                {
                    if (previousEmployee.SysId != sysId)
                    {
                        this.ShowErrorMessage(WinFormMainResource.Message_Atleast);
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
                        LocationName = EmployeeInfo.LocationName;
                    }

                    ChangeMenuColor(currentSelectedEmployee);
                }
            }
        }

        #endregion

        #region KeyPad Action

        public bool CanExecuteReceiveCommand()
        {
            bool hasOrderFood = UserInGroup.Any(x => x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0);
            if (EmployeeInfo != null && hasOrderFood)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Receive order.
        /// </summary>
        public void ExecuteReceiveCommand()
        {
            bool dataValid = CheckDataValid();
            if (dataValid)
            {
                ShowOverlay();
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

        public bool CanExecuteAddCommand()
        {
            if (CommonKitchenOrder != null && (this.CheckMainFoodMenu || this.CheckAlternativeFoodMenu)
                && CommonKitchenOrder.Any(x => x.IsFocusedMealMenu))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Add member to order.
        /// </summary>
        public void ExecuteAddCommand()
        {
            if (!string.IsNullOrEmpty(Result))
            {
                var getEmployeeByCodeCriteria = new GetEmployeeByCodeCriteria() { Code = Result };
                List<EmployeeInfo> employees = this.KitchenService.GetEmployeeOrderInfo(getEmployeeByCodeCriteria);
                List<KitchenEmployeeInfo> listKitchenEmployeeInfo = Mapper.Map<List<EmployeeInfo>, List<KitchenEmployeeInfo>>(employees);
                if (listKitchenEmployeeInfo.Any())
                {
                    if (listKitchenEmployeeInfo.Count == 1)
                    {
                        KitchenEmployeeInfo selectedEmployeeInfo = listKitchenEmployeeInfo.SingleOrDefault();
                        if (EmployeeInfo == null)
                        {
                            EmployeeInfo = selectedEmployeeInfo;
                            LocationName = EmployeeInfo.LocationName;
                        }

                        bool result = GetEmployeeInGroup(selectedEmployeeInfo.SysId);
                        if (result)
                        {
                            ChangeButtonBorder(Constant.ChooseMainFoodButton);
                            CommonKitchenOrder = AllObservationFood;
                        }
                    }
                    else
                    {
                        this.ShowOverlay();
                        DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>(listKitchenEmployeeInfo);
                        MessageHeader = string.Format(WinFormMainResource.DuplicateEmployeeMessage, DuplicateUser.Count);
                        EmployeeSelectionWindow employeeSelectionWindow = new EmployeeSelectionWindow()
                        {
                            DataContext = this,
                            Owner = Application.Current.MainWindow
                        };
                        employeeSelectionWindow.ShowDialog();
                    }
                }
                else
                {
                    this.ShowErrorMessage(string.Format(WinFormMainResource.CannotFindEmployeeCode, Result));
                }

                Result = null;
            }
        }

        public bool CanExecuteCancelCommand()
        {
            if (EmployeeInfo != null || CommonKitchenOrder.Count > 0 || (Result != null && Result.Count() > 0))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Cancel command.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public void ExecuteCancelCommand(object value)
        {
            if (value != null)
            {
                if (EmployeeInfo != null || (UserInGroup.Any() && UserInGroup.Count > 0) || CommonKitchenOrder.Count > 0)
                {
                    WPFMessageBoxResult confirmCancel = WPFMessageBox.Show(
                        WinFormMainResource.Confirmation,
                        WinFormMainResource.Message_CancelOrder,
                        WPFMessageBoxButtons.YesNo,
                        WPFMessageBoxImage.Alert);
                    if (confirmCancel == WPFMessageBoxResult.Yes)
                    {
                        ResetCurrentModel(value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Result))
                    {
                        Result = string.Empty;
                    }
                }
            }
        }

        public bool CanExecuteNumPadPressed()
        {
            if (Result != null && Result.Length == 6)
            {
                return false;
            }

            return true;
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
                    if ((CheckMainFoodMenu || CheckAlternativeFoodMenu) && CommonKitchenOrder.Any(x => x.IsFocusedMealMenu))
                    {
                        StartTimerAddQuantity();
                    }

                    break;
            }
        }

        /// <summary>
        /// Action for Keypad Released.
        /// </summary>
        /// <returns>Boolean to check.</returns>
        public bool CanExecuteNumPadReleased()
        {
            bool hasSelectedMemberInGroup = UserInGroup.Any(x => x.IsSelectedMemberInGroup);
            bool hasMiddleSideDataFocused = CommonKitchenOrder.Any(x => x.IsFocusedMealMenu);
            bool hasTextBlockFocused = this.TextBlockFocused && !string.IsNullOrEmpty(Result);
            if (hasMiddleSideDataFocused || hasSelectedMemberInGroup || hasTextBlockFocused)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Execute Number keypad Released.
        /// </summary>
        public void ExecuteNumPadReleased()
        {
            var currentSelectedEmployee = UserInGroup.Where(x => x.SysId == CurrentSelectedEmployeeSysId).SingleOrDefault();
            CommonKitchenOrder currentSelectedMenu = null;

            if (this.TextBlockFocused)
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

        #endregion

        #region Modal Confirm Action

        public bool CanSaveOrderCommand()
        {
            return true;
        }

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

        public bool CanExecuteSaveDialogCommand()
        {
            return true;
        }

        /// <summary>
        /// Add employee in dialog after user select an Employee.
        /// </summary>
        /// <param name="value">Dialog object.</param>
        public void ExecuteSaveDialogCommand(object value)
        {
            if (DuplicateUser.Any(x => x.IsChecked))
            {
                if (EmployeeInfo == null)
                {
                    EmployeeInfo = DuplicateUser.Where(x => x.IsChecked).FirstOrDefault();
                    GetEmployeeInGroup(EmployeeInfo.SysId, value);
                }
                else
                {
                    GetEmployeeInGroup(DuplicateUser.Where(x => x.IsChecked).FirstOrDefault().SysId, value);
                }
            }
        }

        public bool CanExecuteCancelDialogCommand()
        {
            return true;
        }

        public void ExecuteCancelDialogCommand(object value)
        {
            CloseDialog(value);
        }

        public bool CanExecuteCancelDialogOrderConfirmation()
        {
            return true;
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Change Menu color.
        /// </summary>
        /// <param name="currentSelectedEmployee">Menu color of Current Selected employee.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void ChangeMenuColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            bool checkMainFoodMenu = CheckMainFoodMenu;
            bool hasNotOrderMainFood = !currentSelectedEmployee.ListMainMenuOrderDeatails.Any(x => x.SelectedMenuItemId == int.MaxValue);
            bool isAutoAddMainFood = hasNotOrderMainFood && !IsDataEdit && !currentSelectedEmployee.DisabledAutoAddFood;
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
                CommonKitchenOrder.ToList().ForEach(x => { x.IsFocusedMealMenu = false; x.IsSelectedMealMenu = false; x.Quantity = 0; });
                List<EmployeeOrderDetails> employeeOrderDetails = null;
                if (checkMainFoodMenu)
                {
                    employeeOrderDetails = currentSelectedEmployee.ListMainMenuOrderDeatails;
                }
                else
                {
                    employeeOrderDetails = currentSelectedEmployee.ListAlternativeMenuOrderDeatails;
                }

                //// Get list id of foods that user has ordered.
                List<int> selectedMainMenu = employeeOrderDetails.Select(x => x.SelectedMenuItemId).ToList();
                //// Fill data order of curren user to view.
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
            else
            {
                ExecuteChooseMainFoodCommand();
            }

            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
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
                    case Constant.MainMenuId:
                        currentMenuList = currentSelectedMember.ListMainMenuOrderDeatails;
                        employeeOrderDetails = currentMenuList.Where(x => x.SelectedMenuItemId == itemId).SingleOrDefault();

                        break;
                    case Constant.AlternativeMenuId:
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
                            employeeOrderDetails.SelectedMenuItemQuantity = increaseValue;
                            selectedMenu.Quantity = employeeOrderDetails.SelectedMenuItemQuantity;
                        }
                    }
                }

                currentSelectedMember.OrderSummary = OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedMember);
                UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
            }
        }

        public List<KitchenEmployeeInfo> GetUserByGroup(int groupId)
        {
            var getUserByGroupIdCriteria = new GetUserByGroupIdCriteria() { GroupOrderId = groupId };
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

        /// <summary>
        /// Get Employee Belong to a Group.
        /// </summary>
        /// <param name="sysId">Employee Sys Id.</param>
        /// <param name="value">Instance of EmployeeSelectionWindow.</param>
        /// <returns>Boolean to check.</returns>
        public bool GetEmployeeInGroup(int sysId, object value = null)
        {
            var getEmployeeInGroupBySysIdCriteria = new GetEmployeeInGroupBySysIdCriteria() { SysId = sysId };
            EmployeeInGroup employeeInGroup = this.KitchenService.GetEmployeeInGroup(getEmployeeInGroupBySysIdCriteria);
            bool result = false;
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

                    result = CheckValidEmployeeData(sysId, value);
                }
                else
                {
                    if (!UserInGroup.Any(x => x.SysId == sysId))
                    {
                        KitchenEmployeeInfo employee = MappingEmployeeData(employeeInGroup.Employees.FirstOrDefault());
                        if (employee != null)
                        {
                            UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                            employee.IsSelectedMemberInGroup = true;
                            employee.IsPlaceOrder = true;
                            UserInGroup.Add(employee);

                            ShowGridDisplayUser();
                            CloseEmployeeSelectionDialog(value);
                        }

                        CommonKitchenOrder = AllObservationFood;
                        ChangeMenuColor(employee);
                    }
                    else
                    {
                        this.ShowErrorMessage(WinFormMainResource.Message_EmployeeExist);
                    }

                    result = true;
                }

                CurrentSelectedEmployeeSysId = sysId;
            }

            return result;
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
                        ShowGridDisplayUser();
                        UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                    }
                }
                else
                {
                    //// Unselected item in group.
                    if (UserInGroup != null)
                    {
                        List<KitchenEmployeeInfo> currenUserInGroup = UserInGroup.Where(x => x.GroupOrderId != currentSelectedItem.ItemId || x.IsPlaceOrder).ToList();
                        UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(currenUserInGroup.Where(x => x.GroupOrderId > 0 || x.IsPlaceOrder));
                        if (UserInGroup.Count == 0)
                        {
                            HideGridDisplayUser();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check valid employee data in grid.
        /// </summary>
        /// <param name="sysId">Employee sys id.</param>
        /// <param name="value">Employee selection Dialog value.</param>
        /// <returns>Boolean to check.</returns>
        public bool CheckValidEmployeeData(int sysId, object value = null)
        {
            KitchenEmployeeInfo currentSelectedEmployee = UserInGroup.Where(x => x.SysId == sysId).SingleOrDefault();
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
                this.ShowErrorMessage(WinFormMainResource.Message_Unhandled);
                return false;
            }
        }

        /// <summary>
        /// Keypad change to Number keypad.
        /// </summary>
        public void ExecuteChangeKeyPad()
        {
            if (NumPadVisible == Constant.Visible)
            {
                NumPadVisible = Constant.Hidden;
                KeyPadVisible = Constant.Visible;
            }
            else
            {
                NumPadVisible = Constant.Visible;
                KeyPadVisible = Constant.Hidden;
            }
        }

        /// <summary>
        /// Middle side data released.
        /// </summary>
        /// <param name="currentSelectedMenu">Current selected Menu.</param>
        /// <param name="currentSelectedEmployee">Current Selected Employee.</param>
        public void MiddSideDataReleased(CommonKitchenOrder currentSelectedMenu, KitchenEmployeeInfo currentSelectedEmployee)
        {
            EmployeeOrderDetails menuRemoved = null;
            if (CheckMainFoodMenu || CheckAlternativeFoodMenu)
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

                currentSelectedEmployee.OrderSummary = OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedEmployee);
                UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(this.UserInGroup);
                currentSelectedMenu.Quantity = 0;
                ChangeMenuColor(currentSelectedEmployee);
            }
            else
            {
                if (CurrentSelectedGroupId > 0)
                {
                    currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == CurrentSelectedGroupId).SingleOrDefault();
                    currentSelectedMenu.IsFocusedMealMenu = false;
                    currentSelectedMenu.IsSelectedMealMenu = false;
                    AddMemberInGroupToGrid(currentSelectedMenu);
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
                ChangeButtonBorder(Constant.PickGroupButton);
                ChangeMemberOrderColor(currentSelectedEmployee);
                CommonKitchenOrder = AllObservationGroup;
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
        /// Add current menu of employee to list.
        /// </summary>
        /// <param name="currentSelectedEmployee">Current selected employee.</param>
        /// <param name="isMainFoodMenu">Is main or alternative menu.</param>
        public void AddFixedMenuToEmployee(KitchenEmployeeInfo currentSelectedEmployee, bool isMainFoodMenu)
        {
            if (isMainFoodMenu)
            {
                string mealName = IsVnmCulture ? currentSelectedEmployee.MenuVnmName : currentSelectedEmployee.MenuEngName;
                CommonKitchenOrder dynamicMainFoodMenu = null;

                if (!CommonKitchenOrder.Any(x => x.ItemId == int.MaxValue))
                {
                    dynamicMainFoodMenu = new CommonKitchenOrder()
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
                    dynamicMainFoodMenu = CommonKitchenOrder.Where(x => x.ItemId == int.MaxValue).SingleOrDefault();
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
                kitchenEmployee = MappingEmployeeData(employee);
            }

            return kitchenEmployee;
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
                this.ShowErrorMessage(result.Message);
            }
            else
            {
                if (WPFMessageBox.Show(string.Empty, WinFormMainResource.Message_OrderSuccessful, WPFMessageBoxButtons.OK) == WPFMessageBoxResult.Ok)
                {
                    ExecuteCancelDialogOrderConfirmation(value);
                    ExecutePrintInvoice(ReferenceCode, (int)InvoiceNumberStatus.DeliverOrderFoodInvoice, false);
                    this.MainViewModel.SendEditOrder(ReferenceCode);
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
                this.ShowErrorMessage(result.CommandResult.Message);
            }
            else
            {
                if (WPFMessageBox.Show(string.Empty, WinFormMainResource.Message_OrderSuccessful, WPFMessageBoxButtons.OK) == WPFMessageBoxResult.Ok)
                {
                    ExecuteCancelDialogOrderConfirmation(value);
                    ExecutePrintInvoice(result.OrderId, (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice, true);
                    this.MainViewModel.SendNewOrder(result.OrderId);
                    ResetCurrentModel();
                }
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
                    this.ShowErrorMessage(WinFormMainResource.Message_PickRoom);
                    return false;
                }
            }

            if (UserInGroup != null && UserInGroup.Count > 0)
            {
                if (!UserInGroup.Any(x => x.ListMainMenuOrderDeatails.Count > 0 || x.ListAlternativeMenuOrderDeatails.Count > 0))
                {
                    this.ShowErrorMessage(WinFormMainResource.Message_Atleast1);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Cancel Current order.
        /// </summary>
        /// <param name="value">Parameter value.</param>
        public void ResetCurrentModel(object value = null)
        {
            string defaultColor = Constant.DefaultButtonOrderBorderColor;
            CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
            UserInGroup = new ObservableCollection<KitchenEmployeeInfo>();
            DuplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
            AllObservationFood = OrderFoodData.GetAllFood;
            AllObservationAlternativeFood = OrderFoodData.GetAllAlternativeFood;
            AllObservationGroup = OrderFoodData.GetAllGroup;
            AllObservationLocation = OrderFoodData.GetAllLocation;
            EmployeeInfo = null;
            Result = null;
            LocationName = null;
            LocationId = 0;
            GridUserVisibility = Constant.Hidden;
            MessageHeader = string.Empty;
            BorderButtonMainFood = defaultColor;
            BorderButtonPickGroup = defaultColor;
            BorderButtonPickRoom = defaultColor;
            BorderButtonAlternativeFood = defaultColor;
            CurrentSelectedAlternativeMenuId = 0;
            CurrentSelectedEmployeeSysId = 0;
            CurrentSelectedMainMenuId = 0;
            CurrentSelectedGroupId = 0;
            IsMenuFocused = false;
            TextBlockFocused = false;
            OrderNo = string.Empty;
            ReferenceCode = 0;
            NumPadVisible = Constant.Visible;
            KeyPadVisible = Constant.Hidden;
            //// Change Text From Nhận to Lưu
            SaveButtonVisible = Constant.Hidden;
            OverlayVisible = Constant.Hidden;
            IsDataEdit = false;
            if (value != null)
            {
                Border textBlockBorder = (Border)value;
                textBlockBorder.BorderBrush = Brushes.White;
            }
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
                LocationName = IsVnmCulture ? order.VnmName : order.EngName;
                List<OrderMenu> orderMenus = order.ListMenu;

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
                            kitchenEmployeeInfo.OrderSummary = OrderFoodData.DisplayEmployeeOrderSummary(kitchenEmployeeInfo);
                        }
                    }

                    IsDataEdit = false;
                    UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(data);
                    CommonKitchenOrder = OrderFoodData.GetAllGroup;
                    ExecutePickGroupCommand();
                    ShowGridDisplayUser();
                    OrderNo = order.OrderNo.ToString().PadLeft(3, '0');
                    ReferenceCode = order.OrderId;
                }
            }
        }

        /// <summary>
        /// Mapping Data for edit action.
        /// </summary>
        /// <param name="orderData">List order menu.</param>
        /// <param name="sysId">Current Sys id.</param>
        /// <returns>Kitchen Employee Info entity.</returns>
        private KitchenEmployeeInfo MappingDataForEditAction(List<OrderMenu> orderData, int sysId)
        {
            KitchenEmployeeInfo kitchenEmployeeInfo = new KitchenEmployeeInfo() { SysId = sysId };
            foreach (OrderMenu orderDetail in orderData)
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
                        kitchenEmployeeInfo.MenuEngName = orderDetail.MenuEngName;
                        kitchenEmployeeInfo.MenuVnmName = orderDetail.MenuVnmName;
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
                kitchenEmployeeInfo.IsPlaceOrder = true;
                kitchenEmployeeInfo.DisabledAutoAddFood = true;
            }

            return kitchenEmployeeInfo;
        }

        /// <summary>
        /// Timer tick to get data.
        /// </summary>
        /// <param name="sender">Object Sender.</param>
        /// <param name="e">Event Argument..</param>
        private void TimerTickToAddQuantity(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            var currentSelectedItem = CommonKitchenOrder.Where(x => x.IsFocusedMealMenu && x.IsSelectedMealMenu).SingleOrDefault();
            if (currentSelectedItem != null)
            {
                KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                int moreQuantity;
                bool isNumeric = int.TryParse(Result, out moreQuantity);
                if (isNumeric)
                {
                    if (CheckMainFoodMenu)
                    {
                        AddSelectedMenuItemToList(currentSelectedItem.ItemId, currentSelectedMember, Constant.MainMenuId, moreQuantity);
                    }
                    else
                    {
                        AddSelectedMenuItemToList(currentSelectedItem.ItemId, currentSelectedMember, Constant.AlternativeMenuId, moreQuantity);
                    }
                }
            }

            Result = string.Empty;
            timer.Stop();
        }

        /// <summary>
        /// Timer get data.
        /// </summary>
        private void StartTimerAddQuantity()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTickToAddQuantity;
            timer.Start();
        }

        #endregion

        #region Private Methods

        private KitchenEmployeeInfo MappingEmployeeData(EmployeeInfo employeeInfo)
        {
            KitchenEmployeeInfo employee = null;
            if (employeeInfo != null)
            {
                employee = Mapper.Map<EmployeeInfo, KitchenEmployeeInfo>(employeeInfo);
            }

            return employee;
        }

        /// <summary>
        /// Default add Main food to employee.
        /// </summary>
        /// <param name="currentSelectedEmployee">Selected employee.</param>
        /// <returns>Current employee data.</returns>
        private KitchenEmployeeInfo AutoAddMainFoodWhenFocused(KitchenEmployeeInfo currentSelectedEmployee)
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
                currentSelectedEmployee.OrderSummary = OrderFoodData.DisplayEmployeeOrderSummary(currentSelectedEmployee);
            }

            return currentSelectedEmployee;
        }

        /// <summary>
        /// Register Relay Command.
        /// </summary>
        private void RegisterRelayCommands()
        {
            RegisterNumPadRelayCommand();
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
            ChangeKeyPad = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteChangeKeyPad());
        }

        private void RegisterNumPadRelayCommand()
        {
            AddCommand = new RelayCommand((x) => CanExecuteAddCommand(), (y) => ExecuteAddCommand());
            CancelCommand = new RelayCommand((x) => CanExecuteCancelCommand(), (y) => ExecuteCancelCommand(y));
            ReceiveCommand = new RelayCommand((x) => CanExecuteReceiveCommand(), (y) => ExecuteReceiveCommand());
            NumPadPressed = new RelayCommand((x) => CanExecuteNumPadPressed(), (y) => ExecuteNumPadPressed(y));
            NumPadReleased = new RelayCommand((x) => CanExecuteNumPadReleased(), (y) => ExecuteNumPadReleased());
        }

        #endregion
    }
}
