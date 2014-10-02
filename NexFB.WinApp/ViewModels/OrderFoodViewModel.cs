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
        private static ObservableCollection<CommonKitchenOrder> _allObservableGroup = new ObservableCollection<CommonKitchenOrder>();
        private static ObservableCollection<KitchenEmployeeInfo> _userInGroup = new ObservableCollection<KitchenEmployeeInfo>();
        private static ObservableCollection<KitchenEmployeeInfo> _duplicateUser = new ObservableCollection<KitchenEmployeeInfo>();
        private static KitchenEmployeeInfo _employeeInfo;

        private string _result;
        private string _borderButtonPickRoom;
        private string _borderButtonChooseDishes;
        private string _borderButtonPickGroup;
        private string _imageCancel;
        private string _gridUserVisibility;
        private string _messageHeader;

        private int _currentSelectedEmployeeSysId;

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
            ChooseDishesCommand = new RelayCommand((x) => CanExecuteChooseDishesCommand(), (y) => ExecuteChooseDishesCommand());
            DefaultEmployeeSelected = new RelayCommand((x) => CanExecuteEmployeeInGroupSelected(), (y) => ExecuteEmployeeInGroupSelected(y));

            SaveDialogCommand = new RelayCommand((x) => CanExecuteSaveDialogCommand(), (y) => ExecuteSaveDialogCommand(y));
            CancelDialogCommand = new RelayCommand((x) => CanExecuteCancelDialogCommand(), (y) => ExecuteCancelDialogCommand(y));

            AllObservationFood = this.GetAllFood;
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
                        IsSelectedMealMenu = false
                    };

                    List<CommonKitchenOrder> data = Mapper.Map<List<FoodOrder>, List<CommonKitchenOrder>>(foods);
                    data.Insert(0, fixedMeal);
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

                List<CommonKitchenOrder> data = Mapper.Map<List<GroupOrder>, List<CommonKitchenOrder>>(groups);
                return new ObservableCollection<CommonKitchenOrder>(data);
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
                if (_messageHeader == value)
                {
                    return;
                }

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
                if (_currentSelectedEmployeeSysId == value)
                {
                    return;
                }

                _currentSelectedEmployeeSysId = value;
                OnPropertyChanged("CurrentSelectedEmployeeSysId");
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
                if (_employeeInfo == value)
                {
                    return;
                }

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
                if (_userInGroup == value)
                {
                    return;
                }

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
                if (_duplicateUser == value)
                {
                    return;
                }

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
                if (_commonKitchenOrder == value)
                {
                    return;
                }

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
                if (_allObservableFood == value)
                {
                    return;
                }

                _allObservableFood = value;
                OnPropertyChanged("AllObservationFood");
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
                if (_allObservableGroup == value)
                {
                    return;
                }

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
                if (_allObservableLocation == value)
                {
                    return;
                }

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

        public string BorderButtonChooseDishes
        {
            get
            {
                return _borderButtonChooseDishes;
            }

            set
            {
                _borderButtonChooseDishes = value;
                this.OnPropertyChanged("BorderButtonChooseDishes");
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

        public ICommand ChooseDishesCommand { get; set; }

        public ICommand DefaulMenuSelected { get; set; }

        public ICommand DefaultEmployeeSelected { get; set; }

        public ICommand ReceiveCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand CancelDialogCommand { get; set; }

        public ICommand SaveDialogCommand { get; set; }

        #endregion

        #region RelayCommands

        // Can Execute Command.
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
            return true;
        }

        public bool CanExecuteAddCommand()
        {
            return true;
        }

        public bool CanExecuteCancelCommand()
        {
            return true;
        }

        public bool CanExecutePickRoomCommand()
        {
            return true;
        }

        public bool CanExecutePickGroupCommand()
        {
            return true;
        }

        public bool CanExecuteChooseDishesCommand()
        {
            return true;
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
            return true;
        }

        // Execute Command
        public void ExecuteReceiveCommand()
        {
        }

        /// <summary>
        /// Add member to order.
        /// </summary>
        public void ExecuteAddCommand()
        {
            if (Result != null && Result.Length == 5)
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

        public void ExecuteCancelCommand()
        {
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

        public void ExecuteChooseDishesCommand()
        {
            ChangeButtonBorder(3);
            CommonKitchenOrder = AllObservationFood;
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
                var currentSelectedItem = CommonKitchenOrder.Where(x => x.ItemId == itemId).SingleOrDefault();
                currentSelectedItem.IsSelectedMealMenu = !currentSelectedItem.IsSelectedMealMenu;
                AddMemberInGroupToGrid(currentSelectedItem);
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
            }
        }

        /// <summary>
        /// Add Member In Group to Grid.
        /// </summary>
        /// <param name="currentSelectedItem">Current Selected member.</param>
        public void AddMemberInGroupToGrid(CommonKitchenOrder currentSelectedItem)
        {
            if (CommonKitchenOrder == AllObservationGroup)
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
                    }
                }
                else
                {
                    if (UserInGroup != null)
                    {
                        var currenUserInGroup = UserInGroup.Where(x => x.GroupOrderId != currentSelectedItem.ItemId || x.IsSelectedMemberInGroup).ToList();
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

        public void ExecuteNumPadReleased()
        {
            if (CurrentSelectedEmployeeSysId > 0)
            {
                var currentSelectedEmployee = UserInGroup.Where(x => x.SysId == CurrentSelectedEmployeeSysId).SingleOrDefault();
                currentSelectedEmployee.IsPlaceOrder = false;
                currentSelectedEmployee.IsSelectedMemberInGroup = false;
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
                    BorderButtonChooseDishes = defaultColor;
                    break;
                case 2:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = red;
                    BorderButtonChooseDishes = defaultColor;
                    break;
                case 3:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonChooseDishes = red;
                    break;
                default:
                    BorderButtonPickRoom = defaultColor;
                    BorderButtonPickGroup = defaultColor;
                    BorderButtonChooseDishes = defaultColor;
                    break;
            }
        }

        #endregion
    }
}
