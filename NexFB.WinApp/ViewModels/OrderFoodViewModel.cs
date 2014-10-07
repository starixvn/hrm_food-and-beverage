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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using AutoMapper;
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

    /// <summary>
    /// Order Food ViewModel.
    /// </summary>
    public class OrderFoodViewModel : ViewModelBase
    {
        #region Fields

        private static ObservableCollection<CommonKitchenOrder> _commonKitchenOrder = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableLocation = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableAlternativeFood = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<CommonKitchenOrder> _allObservableGroup = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<KitchenEmployeeInfo> _userInGroup = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<KitchenEmployeeInfo> _duplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
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
        private string _menuFoodVisible;

        #endregion

        #region Constructors

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Reviewed. Suppression is OK here.")]
        public OrderFoodViewModel()
        {
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

            AllObservationFood = this.GetAllFood;
            AllObservationAlternativeFood = this.GetAllAlternativeFood;
            AllObservationLocation = this.GetAllLocation;
            AllObservationGroup = this.GetAllGroup;
        }

        #endregion

        #region Properties

        public ObservableCollection<Order> GetWaitingMealOrder
        {
            get
            {
                var criteria = new GetWaitingMealOrderCriteria();
                List<Order> waitingMeals = KitchenService.GetWaitngMealOrder(criteria);

                ObservableCollection<Order> observableWaitingMeals = new ObservableCollection<Order>(waitingMeals);
                return observableWaitingMeals;
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllLocation
        {
            get
            {
                var getAllLocation = new GetAllLocationCriteria();
                List<Location> locations = this.CommonService.GetAllLocation(getAllLocation).ToList();

                List<CommonKitchenOrder> data = Mapper.Map<List<Location>, List<CommonKitchenOrder>>(locations);
                return new ObservableCollection<CommonKitchenOrder>(data);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllFood
        {
            get
            {
                var getAllFood = new GetFoodNameCriteria();
                List<FoodOrder> foods = this.KitchenService.GetAllFoodOrder(getAllFood);
                if (foods != null)
                {
                    var fixedMeal = new CommonKitchenOrder()
                    {
                        ItemId = int.MaxValue,
                        ItemName = Constant.FixedMeal,
                        IsSelectedMealMenu = false,
                        IsMainMenu = true
                    };

                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data.Insert(0, fixedMeal);
                    return new ObservableCollection<CommonKitchenOrder>(data);
                }

                return new ObservableCollection<CommonKitchenOrder>();
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllAlternativeFood
        {
            get
            {
                var getAlternativeFood = new GetAlternativeFoodCriteria();
                List<FoodOrder> alternativeFoods = this.KitchenService.GetAllAlternativeFoodOrder(getAlternativeFood).ToList();

                List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(alternativeFoods);
                return new ObservableCollection<CommonKitchenOrder>(data);
            }
        }

        public ObservableCollection<CommonKitchenOrder> GetAllGroup
        {
            get
            {
                var getAllGroup = new GetGroupNameCriteria();
                List<GroupOrder> groups = this.KitchenService.GetAllGroupOrder(getAllGroup);

                List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                return new ObservableCollection<CommonKitchenOrder>(data);
            }
        }

        public string MenuFoodVisible
        {
            get
            {
                return _menuFoodVisible;
            }

            set
            {
                _menuFoodVisible = value;
                OnPropertyChanged("MenuFoodVisible");
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

        #endregion

        #region RelayCommands

        // Can Execute Command.
        public bool CanExecuteSelectAllUserInGroupCommand()
        {
            if (UserInGroup.Count > 0)
            {
                return true;
            }
            
            return false;
        }

        public bool CanExecuteSaveDialogCommand()
        {
            return true;
        }

        public bool CanExecuteCancelDialogCommand()
        {
            return true;
        }

        public bool CanExecuteReceiveCommand()
        {
            if (EmployeeInfo != null && UserInGroup.Any())
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
            return true;
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
            bool isCurrentMenu = CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0;
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup) || (CommonKitchenOrder.Any(x => x.IsSelectedMealMenu) && isCurrentMenu))
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
        public void ExecuteReceiveCommand()
        {
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

                            Result = null;
                        }
                        else
                        {
                            MessageBox.Show(string.Format(WinFormMainResource.CannotFindEmployeeCode, Result));
                            Result = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show(WinFormMainResource.IllegalEmployeeCode);
                        Result = null;
                    }
                }
                else
                {
                    bool isMainMenu = CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0;
                    bool isAlternativeMenu = CommonKitchenOrder.Except(AllObservationAlternativeFood).ToList().Count == 0;
                    if (isMainMenu || isAlternativeMenu)
                    {
                        var currentSelectedItem = CommonKitchenOrder.Where(x => x.IsFocusedMealMenu && x.IsSelectedMealMenu).SingleOrDefault();
                        currentSelectedItem.Quantity += int.Parse(Result);
                        Result = null;
                    }
                }
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
                CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                CommonKitchenOrder.Where(x => currentSelectedMember.ListSelectedMainMenuItem.Contains(x.ItemId)).ToList().ForEach(y => y.IsSelectedMealMenu = true);
                ////CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
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
                CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                CommonKitchenOrder.Where(x => currentSelectedMember.ListSelectedAlternativeMenuItem.Contains(x.ItemId)).ToList().ForEach(y => y.IsSelectedMealMenu = true);
                ////CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
            }
        }

        /// <summary>
        /// Execute Meal Menu Selected.
        /// </summary>
        /// <param name="value">Value pass from user interface.</param>
        public void ExecuteMealMenuSelected(object value)
        {
            bool isMainMenu = CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0;
            bool isAlternativeMenu = CommonKitchenOrder.Except(AllObservationAlternativeFood).ToList().Count == 0;
            bool isGroupMenu = CommonKitchenOrder.Except(AllObservationGroup).ToList().Count == 0;
            bool isLocationMenu = CommonKitchenOrder.Except(AllObservationLocation).ToList().Count == 0;
            if (value != null)
            {
                var itemId = (int)value;
                var currentSelectedItem = CommonKitchenOrder.Where(x => x.ItemId == itemId).SingleOrDefault();
                if (isMainMenu || isAlternativeMenu)
                {
                    CommonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                    currentSelectedItem.IsFocusedMealMenu = true;
                    currentSelectedItem.IsSelectedMealMenu = true;
                    IsMenuFocused = true;
                    if (currentSelectedItem.Quantity == 0)
                    {
                        currentSelectedItem.Quantity = 1;
                    }

                    KitchenEmployeeInfo currentSelectedMember = UserInGroup.Where(x => x.IsSelectedMemberInGroup).SingleOrDefault();
                    if (isMainMenu)
                    {
                        CurrentSelectedMainMenuId = itemId;
                        AddSelectedMenuItemToList(itemId, currentSelectedMember.ListSelectedMainMenuItem);
                    }
                    else
                    {
                        CurrentSelectedAlternativeMenuId = itemId;
                        AddSelectedMenuItemToList(itemId, currentSelectedMember.ListSelectedAlternativeMenuItem);
                    }
                }

                if (isGroupMenu)
                {
                    currentSelectedItem.IsSelectedMealMenu = !currentSelectedItem.IsSelectedMealMenu;
                    AddMemberInGroupToGrid(currentSelectedItem);
                }

                if (isLocationMenu)
                {
                    CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                    currentSelectedItem.IsSelectedMealMenu = true;
                    if (string.IsNullOrEmpty(EmployeeInfo.LocationName))
                    {
                        this.LocationName = currentSelectedItem.ItemName;
                    }
                }
            }
        }

        public void AddSelectedMenuItemToList(int itemId, List<int> currentSelectedMeal)
        {
            if (UserInGroup.Any(x => x.IsSelectedMemberInGroup))
            {
                if (!currentSelectedMeal.Any(x => x == itemId))
                {
                    currentSelectedMeal.Add(itemId);
                }
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
                IsMenuFocused = false;
                var sysId = (int)value;
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
                    GetEmployeeInGroup(EmployeeInfo.SysId);
                }
                else
                {
                    GetEmployeeInGroup(DuplicateUser.Where(x => x.IsChecked == true).FirstOrDefault().SysId);
                }

                CloseDialog(value);
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

        /// <summary>
        /// Get Employee Belong to a Group.
        /// </summary>
        /// <param name="sysId">Employee Sys Id.</param>
        public void GetEmployeeInGroup(int sysId)
        {
            var getEmployeeInGroupBySysIdCriteria = new GetEmployeeInGroupBySysIdCriteria() { SysId = sysId };
            EmployeeInGroup employeeInGroup = this.KitchenService.GetEmployeeInGroup(getEmployeeInGroupBySysIdCriteria);
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
                    var currentSelectedEmployee = UserInGroup.Where(x => x.SysId == sysId).SingleOrDefault();
                    currentSelectedEmployee.IsPlaceOrder = true;
                    currentSelectedEmployee.IsSelectedMemberInGroup = true;
                    CurrentSelectedEmployeeSysId = sysId;
                }
                else
                {
                    KitchenEmployeeInfo employee = this.GetEmployeeBySysId(sysId);
                    if (employee != null)
                    {
                        UserInGroup.ToList().ForEach(x => x.IsSelectedMemberInGroup = false);
                        employee.IsSelectedMemberInGroup = true;
                        employee.IsPlaceOrder = true;
                        UserInGroup.Add(employee);
                        GridUserVisibility = Constant.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// Add Member In Group to Grid.
        /// </summary>
        /// <param name="currentSelectedItem">Current Selected member.</param>
        public void AddMemberInGroupToGrid(CommonKitchenOrder currentSelectedItem)
        {
            if (CommonKitchenOrder.Except(AllObservationGroup).ToList().Count == 0)
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
                        var currenUserInGroup = UserInGroup.Where(x => x.GroupOrderId != currentSelectedItem.ItemId || x.IsPlaceOrder).ToList();
                        UserInGroup = new ObservableCollection<KitchenEmployeeInfo>(currenUserInGroup);
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
            if (!IsMenuFocused)
            {
                if (CurrentSelectedEmployeeSysId > 0)
                {
                    currentSelectedEmployee.IsPlaceOrder = false;
                    currentSelectedEmployee.IsSelectedMemberInGroup = false;
                    ChangeMemberOrderColor(currentSelectedEmployee);
                }
            }
            else
            {
                bool isMainFoodMenu = CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0;
                if (isMainFoodMenu)
                {
                    if (CurrentSelectedMainMenuId > 0)
                    {
                        var currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == CurrentSelectedMainMenuId).SingleOrDefault();
                        currentSelectedEmployee.ListSelectedMainMenuItem.Remove(CurrentSelectedMainMenuId);
                        currentSelectedMenu.Quantity = 0;
                    }
                }
                else
                {
                    if (CurrentSelectedAlternativeMenuId > 0)
                    {
                        var currentSelectedMenu = CommonKitchenOrder.Where(x => x.ItemId == CurrentSelectedAlternativeMenuId).SingleOrDefault();
                        currentSelectedEmployee.ListSelectedAlternativeMenuItem.Remove(CurrentSelectedAlternativeMenuId);
                        currentSelectedMenu.Quantity = 0;
                    }
                }

                ChangeMenuColor(currentSelectedEmployee);
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
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.UnRegisterMealColor;
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
                        currentSelectedEmployee.EmployeeOrderMealColor = Constant.UnRegisterMealColor;
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
        public void ChangeMenuColor(KitchenEmployeeInfo currentSelectedEmployee)
        {
            bool isMainFoodMenu = CommonKitchenOrder.Except(AllObservationFood).ToList().Count == 0;
            bool isAlternativeFoodMenu = CommonKitchenOrder.Except(AllObservationAlternativeFood).ToList().Count == 0;
            if (isMainFoodMenu || isAlternativeFoodMenu)
            {
                CommonKitchenOrder.ToList().ForEach(x => x.IsSelectedMealMenu = false);
                CommonKitchenOrder.ToList().ForEach(x => x.IsFocusedMealMenu = false);
                if (isMainFoodMenu)
                {
                    CommonKitchenOrder.Where(x => currentSelectedEmployee.ListSelectedMainMenuItem.Contains(x.ItemId)).ToList().ForEach(y => y.IsSelectedMealMenu = true);
                }
                else
                {
                    CommonKitchenOrder.Where(x => currentSelectedEmployee.ListSelectedAlternativeMenuItem.Contains(x.ItemId)).ToList().ForEach(y => y.IsSelectedMealMenu = true);
                }

                CommonKitchenOrder = new ObservableCollection<CommonKitchenOrder>(this.CommonKitchenOrder);
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
                    MenuFoodVisible = Constant.Hidden;
                    break;
                case 2:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = red;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = defaultColor;
                    MenuFoodVisible = Constant.Hidden;
                    break;
                case 3:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = red;
                    BorderButtonAlternativeFood = defaultColor;
                    MenuFoodVisible = Constant.Visible;
                    break;
                case 4:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = red;
                    MenuFoodVisible = Constant.Visible;
                    break;
                default:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonMainFood = defaultColor;
                    BorderButtonAlternativeFood = defaultColor;
                    MenuFoodVisible = Constant.Hidden;
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
            Result = string.Empty;
            GridUserVisibility = Constant.Hidden;
            MessageHeader = string.Empty;
            BorderButtonMainFood = defaultColor;
            BorderButtonPickGroup = defaultColor;
            BorderButtonPickRoom = defaultColor;
            BorderButtonAlternativeFood = defaultColor;
        }

        #endregion
    }
}
