namespace NexFB.WinAppSmartCard.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.Kitchen;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Models.SmartCardOrder;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Enums;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using WPFMessageBoxCustomControl.Enums;

    public class MainDishesViewModel : MainDishModelProperty
    {
        private static DispatcherTimer _digitalClockDispatcher = new DispatcherTimer();
        private MainViewModel _mainViewModel;

        public MainDishesViewModel()
        {
            EditMenu = new RelayCommand((y) => ExecuteEditMenu(y));
            ConfirmCommand = new RelayCommand(async (y) => await ExecuteConfirmCommand());
            RemoveEmployeeOrder = new RelayCommand((y) => ExecuteRemoveEmployeeOrder(y));
            ShowOrderScreen();
            HideNotifyErrorOrder();
        }

        #region Properties

        public MainViewModel MainViewModel
        {
            get
            {
                return _mainViewModel;
            }

            set
            {
                _mainViewModel = value;
                this.OnPropertyChanged("MainViewModel");
            }
        }

        #endregion

        #region Public Methods

        public void AddEmployeeOrderToList(Employee employeeInfo, MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            if (OrderOwner == null)
            {
                OrderOwner = employeeInfo;
            }

            if (ScreenOrderVisible == CommonResources.Visible)
            {
                if (ObservableListEmployee != null)
                {
                    if (!ObservableListEmployee.Any(x => x.SysId == employeeInfo.SysId))
                    {
                        employeeInfo.OrderNo = ObservableListEmployee.Count + 1;
                        employeeInfo.CurrentCulture = mainViewModel.CurrentCulture;
                        HideMenuList(employeeInfo);
                        GetAlternativeFood(employeeInfo);
                        ProcessAddDataInCurrentThread(employeeInfo);
                    }
                    else
                    {
                        // this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_AlreadyThisOrder);
                    }
                }
            }
            else
            {
                DisplayEmployeeHasAlreadyOrdered(employeeInfo.IsLocal);
            }
        }

        public void CheckDataForLanguageChange(Employee employeeInfo, MainViewModel mainViewModel)
        {
            if (ObservableListEmployee.Count == 1)
            {
                ObservableListEmployee = new ObservableCollection<Employee>();
                AddEmployeeOrderToList(employeeInfo, mainViewModel);
            }
        }

        public void DisplayEmployeeHasAlreadyOrdered(bool employeeInLocal)
        {
            MainViewModel.ChangeLanguageAccordingToEmployeeType(employeeInLocal, true);
            if (ObservableListEmployee.Count == 0)
            {
                var getEmployeeOrderBySysId = new GetMyOrderBySysIdCriteria() { SysId = OrderOwner.SysId };
                List<SCEmployeeOrder> employeeOrders = this.SmartCardOrderService.GetMyOrderBySysId(getEmployeeOrderBySysId);
                if (employeeOrders != null && employeeOrders.Count > 0)
                {
                    //// Get latest order.
                    int maxValue = employeeOrders.Where(x => !x.IsSideDish).Max(x => x.OrderId);
                    SCEmployeeOrder employeeOrder = employeeOrders.FirstOrDefault(x => x.OrderId == maxValue);

                    var getOrderDetailsCriteria = new GetOrderDetailsCriteria { OrderId = employeeOrder.OrderId };
                    Order order = KitchenService.GetOrderDetails(getOrderDetailsCriteria);
                    if (order != null)
                    {
                        OrderNo = order.OrderNo;
                        List<OrderMenu> listOrder = order.ListMenu;
                        if (listOrder != null && listOrder.Count > 0)
                        {
                            foreach (var item in listOrder)
                            {
                                Employee employee = new Employee()
                                {
                                    OrderNo = ObservableListEmployee.Count + 1,
                                    MenuVnmName = item.MenuVnmName,
                                    MenuEngName = item.MenuEngName,
                                    CurrentCulture = MainViewModel.CurrentCulture,
                                    Quantity = item.QuantityOrder,
                                    NickName = item.NickName,
                                    GroupOrderNickName = employeeOrder.OrderNickName
                                };
                                employee.DisplayNotifyMenuName = string.Format("{0} x {1}", employee.DisplayMenuName, employee.Quantity);
                                ObservableListEmployee.Add(employee);
                            }

                            OrderOwner.GroupOrderNickName = order.NickName;
                            OrderOwner = this.OrderOwner;
                            CountDownTimer = ConstantValue.CountDownTimer;
                            TimerDigitalClock();
                        }
                    }
                }
            }
            else
            {
                MainViewModel.MainTabReadOnly = true;
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ReceivedOrder);
            }
        }

        public void GetAlternativeFood(Employee employeeInfo)
        {
            var getAlternativeFoodCriteria = new GetAlternativeFoodBySysIdCriteria() { SysId = employeeInfo.SysId };
            List<FoodOrder> alternativeFood = this.SmartCardOrderService.GetAlternativeFoodBySysId(getAlternativeFoodCriteria);
            if (alternativeFood != null)
            {
                List<Food> foodMapping = Mapper.Map<List<FoodOrder>, List<Food>>(alternativeFood);
                var currentMainFood = new Food
                {
                    FoodId = int.MaxValue,
                    FoodVnmName = employeeInfo.MenuVnmName,
                    FoodEngName = employeeInfo.MenuEngName,
                    FoodChineseName = employeeInfo.MenuEngName,
                    CurrentCulture = MainViewModel.CurrentCulture
                };

                SelectedFood = currentMainFood;
                foodMapping.Insert(0, currentMainFood);
                foodMapping.ForEach(x => x.CurrentCulture = MainViewModel.CurrentCulture);
                employeeInfo.ListAlternativeFood = foodMapping;
            }
        }

        public void ProcessAddDataInCurrentThread(Employee employeeInfo)
        {
            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                ObservableListEmployee.Add(employeeInfo);
                ToggleSideMenu();
                DeleteButtonVisible = AllowDeleteMemberInOrder();
                CheckAllowChangeLanguage();
            }));
        }

        public void CheckAllowChangeLanguage()
        {
            MainViewModel.IsAllowChangeLanguage = this.ObservableListEmployee.Count < 2;
        }

        public void CancelTimerDispatcher()
        {
            MainDishTimerLive = false;
            _digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        public void ShowOrderScreen()
        {
            ScreenOrderVisible = CommonResources.Visible;
            ScreenNotificationVisible = CommonResources.Hidden;
        }

        public void ShowNotificationScreen()
        {
            ScreenOrderVisible = CommonResources.Hidden;
            ScreenNotificationVisible = CommonResources.Visible;
        }

        public void ShowNotifyErrorOrder()
        {
            NotifyErrorOrder = CommonResources.Visible;
        }

        public void HideNotifyErrorOrder()
        {
            NotifyErrorOrder = CommonResources.Hidden;
        }

        public async Task ExecuteConfirmCommand()
        {
            if (ObservableListEmployee.Count == 0)
            {
                this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_ContainEmployee);
            }
            else
            {
                var orderDetails = new StringBuilder(ConstantValue.BeginRootTag);

                if (ObservableListEmployee.Any() && ObservableListEmployee.Count > 0)
                {
                    foreach (var employee in ObservableListEmployee)
                    {
                        DisplayOrderedMeal(employee);
                        if (employee.SelectedValueDropDown == 0 || employee.SelectedValueDropDown == int.MaxValue)
                        {
                            orderDetails.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, 1, 1);
                        }
                        else
                        {
                            orderDetails.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, employee.SelectedValueDropDown, 0);
                        }
                    }
                }

                string errorEmployees = CheckValidOrder(ObservableListEmployee);
                if (errorEmployees != "")
                {
                    ErrorEmployees = errorEmployees + MainScreenResources.Message_ReasonOrderFail;
                    ShowNotifyErrorOrder();
                    await Task.Delay(5000);
                    HideNotifyErrorOrder();
                }
                else
                {
                    var insertOrderCriteria = new InsertOrderMealByUserCriteria()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = OrderOwner.SysId,
                        TakeAway = false,
                        ListDetail = orderDetails.Append(ConstantValue.EndRootTag).ToString()
                    };

                    OrderResult result = this.SmartCardOrderService.InsertOrderFood(insertOrderCriteria);
                    if (result.CommandResult.Code == 0)
                    {
                        OrderSuccessAction(result);
                    }
                    else
                    {
                        this.ShowErrorMessage(MainViewModel.CurrentCulture, result.CommandResult.Message);
                    }
                }
            }
        }

        public string CheckValidOrder(ObservableCollection<Employee> observableListEmployee)
        {
            var infoMainDishOrder = GetInfoMainDishOrder(observableListEmployee);
            var errorEmployee = "";
            foreach (var item in infoMainDishOrder)
            {
                if (item.IsRegistration == null && !item.IsFreeOrder)
                {
                    errorEmployee += item.NickName + ", ";
                }
            }
            errorEmployee = errorEmployee.Trim().TrimEnd(',');
            return errorEmployee;
        }

        public List<SCMainDishEmployeeOrder> GetInfoMainDishOrder(ObservableCollection<Employee> observableListEmployee)
        {
            string userIds = "";
            foreach (var item in observableListEmployee)
            {
                userIds += item.SysId.ToString() + ",";
            }
            userIds = userIds.TrimEnd(',');

            var criteria = new GetInfoMainDishOrderCriteria()
            {
                UserIds = userIds
            };
            return this.SmartCardOrderService.GetInfoMainDishOrder(criteria);
        }

        #endregion

        #region Private Methods

        private void ExecuteRemoveEmployeeOrder(object value)
        {
            if (value != null)
            {
                if (ObservableListEmployee.Count > 1)
                {
                    int employeeSysId = (int)value;
                    Employee employeeRemoved = ObservableListEmployee.SingleOrDefault(x => x.SysId == employeeSysId);
                    ObservableListEmployee.Remove(employeeRemoved);
                    if (employeeRemoved.SysId == OrderOwner.SysId)
                    {
                        OrderOwner = ObservableListEmployee.FirstOrDefault();
                        this.MainViewModel.OrderOwner = this.OrderOwner;
                    }

                    UpdateOrderNo();
                    ToggleSideMenu();
                    DeleteButtonVisible = AllowDeleteMemberInOrder();
                    CheckAllowChangeLanguage();
                }
            }
        }

        private void UpdateOrderNo()
        {
            for (int i = 0; i < ObservableListEmployee.Count; i++)
            {
                ObservableListEmployee[i].OrderNo = i + 1;
            }
        }

        private void ExecuteEditMenu(object value)
        {
            if (value != null)
            {
                WPFMessageBoxResult editOrderConfirm = this.ShowWarningEditOrder(MainViewModel.CurrentCulture);
                if (editOrderConfirm == WPFMessageBoxResult.Yes)
                {
                    int employeeSysId = (int)value;
                    Employee selectedEmployee = ObservableListEmployee.SingleOrDefault(x => x.SysId == employeeSysId);
                    ShowMenuList(selectedEmployee);
                }
            }
        }

        private void OrderSuccessAction(OrderResult result)
        {
            OrderNo = result.OrderNo;
            MainViewModel.UpdateOrderOwnerInfoAfterOrderSuccessful();
            MainViewModel.SendMessage(result.OrderId);
            MainViewModel.MainTabReadOnly = true;
            MainViewModel.SideButtonEnable = true;
            CountDownTimer = ConstantValue.CountDownTimer;
            ShowNotificationScreen();
            TimerDigitalClock();
        }

        private void DisplayOrderedMeal(Employee employee)
        {
            if (employee.SelectedValueDropDown > 0 && employee.SelectedValueDropDown != int.MaxValue)
            {
                employee.DisplayNotifyMenuName = MainViewModel.CurrentCulture == (int)LanguageLocalization.VietNamCulture
                    ? employee.ListAlternativeFood.SingleOrDefault(x => x.FoodId == employee.SelectedValueDropDown).FoodVnmName
                    : employee.ListAlternativeFood.SingleOrDefault(x => x.FoodId == employee.SelectedValueDropDown).FoodEngName;
            }
            else
            {
                employee.DisplayNotifyMenuName = employee.DisplayMenuName;
            }
        }

        private void HideMenuList(Employee employeeInfo)
        {
            employeeInfo.FoodInfoVisibility = CommonResources.Visible;
            employeeInfo.FoodListVisibility = CommonResources.Hidden;
        }

        private void ShowMenuList(Employee employeeInfo)
        {
            employeeInfo.FoodInfoVisibility = CommonResources.Hidden;
            employeeInfo.FoodListVisibility = CommonResources.Visible;
        }

        private void TimerDigitalClock()
        {
            _digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            _digitalClockDispatcher.Tick += new EventHandler(TimerTick);
            _digitalClockDispatcher.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            CountDownTimer -= 1;
            MainDishTimerLive = true;
            if (CountDownTimer == 0)
            {
                if (MainViewModel != null)
                {
                    MainDishTimerLive = false;
                    MainViewModel.ExecuteReturnToStandByScreen();
                }

                _digitalClockDispatcher.Stop();
            }
        }

        private void ToggleSideMenu()
        {
            MainViewModel.SideButtonEnable = this.ObservableListEmployee.Count > 1 ? false : true;
        }

        private string AllowDeleteMemberInOrder()
        {
            return ObservableListEmployee.Count > 1 ? CommonResources.Visible : CommonResources.Hidden;
        }

        #endregion
    }
}