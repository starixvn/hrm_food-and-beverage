// --------------------------------------------------------------------------
// <copyright file="CombinedDishesViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.12.11">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;
    using AutoMapper;
    using NexFB.Criteria.SmartCardOrder;
    using NexFB.Models.Kitchen;
    using NexFB.Pos.Core.Commands;
    using NexFB.Pos.Core.Enums;
    using NexFB.WinAppSmartCard.Core.Constants;
    using NexFB.WinAppSmartCard.Core.Models;
    using NexFB.WinAppSmartCard.Core.Resources;
    using NexFB.WinAppSmartCard.GlobalResources.Resources;
    using NexFB.WinAppSmartCard.ViewModels.Properties;
    using NexFB.WinAppSmartCard.Views;

    /// <summary>
    /// Combined Dishes ViewModel.
    /// </summary>
    public class CombinedDishesViewModel : CombinedDishesProperty
    {
        private static DispatcherTimer digitalClockDispatcher = new DispatcherTimer();

        public CombinedDishesViewModel(Employee orderOwner, ObservableCollection<Employee> listEmployee, MainViewModel mainViewModel)
        {
            ShowOrderView();
            OrderOwner = orderOwner;
            MainViewModel = mainViewModel;
            MainViewModel.SideButtonEnable = true;

            CanTakeAway = false;
            foreach (var employee in listEmployee)
            {
                DisplayOrderedMeal(employee, mainViewModel.CurrentCulture);
            }

            ObservableListEmployee = listEmployee;
            DeleteButtonVisible = ObservableListEmployee.Count > 1 ? CommonResources.Visible : CommonResources.Hidden;

            AddSideFoodPopup = new RelayCommand((x) => ExecuteAddSideFoodPopup(x));
            ConfirmSideFoodPopup = new RelayCommand((x) => ExecuteConfirmSideFoodPopup(x));
            CloseSideFoodPopup = new RelayCommand((x) => ExecuteCloseSideFoodPopup(x));
            DeletedFoodCommand = new RelayCommand((x) => ExecuteDeletedFoodCommand(x));
            DeleteEmployeeCommand = new RelayCommand((x) => ExecuteDeleteEmployeeCommand(x));

            SubQuantityCommand = new RelayCommand((x) => ExecuteSubQuantityCommand(x));
            AddQuantityCommand = new RelayCommand((x) => ExecuteAddQuantityCommand(x));

            SaveOrderFood = new RelayCommand((x) => ExecuteSaveOrderSideFood());
        }

        #region Public Methods

        public void CancelTimerDispatcher()
        {
            SideDishTimerLive = false;
            digitalClockDispatcher.Stop();
            CountDownTimer = 0;
        }

        #endregion

        #region private Methods

        /// <summary>
        /// Execute add side food popup.
        /// </summary>
        /// <param name="data">Current user id.</param>
        private void ExecuteAddSideFoodPopup(object data)
        {
            if (data != null)
            {
                int itemNo = 1;
                int sysId = (int)data;
                int currentCulture = MainViewModel.CurrentCulture;
                Employee selectedEmployee = ObservableListEmployee.Where(x => x.SysId == sysId).SingleOrDefault();
                CurrentEmployeeOrder = selectedEmployee;
                List<Food> food = GetSideFoodBySysId(sysId);

                if (selectedEmployee.ListSideFood != null && selectedEmployee.ListSideFood.Count > 0)
                {
                    List<Food> sortByQuantitySideFood = selectedEmployee.ListSideFood.OrderByDescending(x => x.Quantity).ToList();
                    CurrentListSideFood = new ObservableCollection<Food>(sortByQuantitySideFood);
                    DisplayOrderInfoOfCurrentListSideDish(sortByQuantitySideFood);
                    ShowSideFoodDialog();
                }
                else
                {
                    if (food != null && food.Count > 0)
                    {
                        food.ForEach(x => { x.CurrentCulture = currentCulture; x.CurrentItemNo = itemNo; itemNo++; x.OwnerName = selectedEmployee.NickName; });
                        CurrentListSideFood = new ObservableCollection<Food>(food);
                        selectedEmployee.ListSideFood = food;
                        DisplayOrderInfoOfCurrentListSideDish(food);
                        ShowSideFoodDialog();
                    }
                    else
                    {
                        this.ShowErrorMessage(MainViewModel.CurrentCulture, MainScreenResources.Message_UnavailableSideDish);
                    }
                }
            }
        }

        /// <summary>
        /// Confirm order side dishes.
        /// </summary>
        /// <param name="data">Popup object.</param>
        private void ExecuteConfirmSideFoodPopup(object data)
        {
            ////Update general Order information
            Employee employeeOrderInfo = ObservableListEmployee.Where(x => x.SysId == CurrentEmployeeOrder.SysId).SingleOrDefault();
            int totalSideDishOrder = employeeOrderInfo.ListSideFood.Sum(x => x.Quantity);
            if (totalSideDishOrder > 0)
            {
                CanTakeAway = true;
                string currentOrderInfo = employeeOrderInfo.DisplayNotifyMenuName;
                employeeOrderInfo.DisplayGeneralOrderInfo = string.Format(currentOrderInfo + " , " + MainScreenResources.SideDishes + " x {0}", totalSideDishOrder);
            }
            else
            {
                CanTakeAway = false;
            }

            ////Update total payment
            employeeOrderInfo.DisplayTotalPayment = DisplayTotalPayment(employeeOrderInfo.ListSideFood);

            CloseSideFoodDialog(data);
        }

        /// <summary>
        /// Close popup side dish.
        /// </summary>
        /// <param name="data">Popup object.</param>
        private void ExecuteCloseSideFoodPopup(object data)
        {
            Employee employeeOrderInfo = ObservableListEmployee.Where(x => x.SysId == CurrentEmployeeOrder.SysId).SingleOrDefault();
            employeeOrderInfo.ListSideFood.ForEach(x => x.Quantity = 0);
            employeeOrderInfo.DisplayGeneralOrderInfo = employeeOrderInfo.DisplayNotifyMenuName;
            employeeOrderInfo.DisplayTotalPayment = DisplayTotalPayment(employeeOrderInfo.ListSideFood);
            CanTakeAway = ObservableListEmployee.Any(x => x.ListSideFood != null && x.ListSideFood.Sum(y => y.Quantity) > 0);
            if (!CanTakeAway)
            {
                TakeAwayChecked = false;
            }

            CloseSideFoodDialog(data);
        }

        private void ExecuteDeletedFoodCommand(object data)
        {
            if (data != null)
            {
                var foodId = (int)data;
                Food selectedFoodItem = CurrentEmployeeOrder.ListSideFood.Where(x => x.FoodId == foodId).SingleOrDefault();
                selectedFoodItem.Quantity = 0;
                DisplayOrderInfoOfCurrentListSideDish(CurrentEmployeeOrder.ListSideFood);
            }
        }

        /// <summary>
        /// Delete employee from group.
        /// </summary>
        /// <param name="data">Employee sys id.</param>
        private void ExecuteDeleteEmployeeCommand(object data)
        {
            if (data != null)
            {
                var sysId = (int)data;
                Employee removedEmployee = ObservableListEmployee.Where(x => x.SysId == sysId).SingleOrDefault();
                ObservableListEmployee.Remove(removedEmployee);

                if (removedEmployee.SysId == OrderOwner.SysId)
                {
                    OrderOwner = ObservableListEmployee.FirstOrDefault();
                    this.MainViewModel.OrderOwner = this.OrderOwner;
                }

                DeleteButtonVisible = ObservableListEmployee.Count > 1 ? CommonResources.Visible : CommonResources.Hidden;
            }
        }

        /// <summary>
        /// Execute reduce quantity of a side food.
        /// </summary>
        /// <param name="data">Side food id.</param>
        private void ExecuteSubQuantityCommand(object data)
        {
            if (data != null)
            {
                var foodId = (int)data;
                Food selectedFoodItem = CurrentEmployeeOrder.ListSideFood.Where(x => x.FoodId == foodId).SingleOrDefault();
                if (selectedFoodItem != null)
                {
                    selectedFoodItem.Quantity = selectedFoodItem.Quantity > 0 ? selectedFoodItem.Quantity -= 1 : 0;
                    DisplayOrderInfoOfCurrentListSideDish(CurrentEmployeeOrder.ListSideFood);
                }
            }
        }

        /// <summary>
        /// Add more quantity for specific side food.
        /// </summary>
        /// <param name="data">Side food id.</param>
        private void ExecuteAddQuantityCommand(object data)
        {
            if (data != null)
            {
                var foodId = (int)data;
                Food selectedFoodItem = CurrentEmployeeOrder.ListSideFood.Where(x => x.FoodId == foodId).SingleOrDefault();
                if (selectedFoodItem != null)
                {
                    selectedFoodItem.Quantity += 1;
                    DisplayOrderInfoOfCurrentListSideDish(CurrentEmployeeOrder.ListSideFood);
                }
            }
        }

        /// <summary>
        /// Save data to database.
        /// </summary>
        private void ExecuteSaveOrderSideFood()
        {
            var mainDishes = new StringBuilder(ConstantValue.BeginRootTag);
            var sideDishes = new StringBuilder(ConstantValue.BeginRootTag);

            foreach (var employee in ObservableListEmployee)
            {
                if (employee.SelectedValueDropDown == 0 || employee.SelectedValueDropDown == int.MaxValue)
                {
                    mainDishes.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, 1, 1);
                }
                else
                {
                    mainDishes.AppendFormat(ConstantValue.MainDishOrderDetails, employee.SysId, employee.SelectedValueDropDown, 0);
                }

                List<Food> listSideFood = employee.ListSideFood;
                if (listSideFood != null)
                {
                    foreach (Food item in listSideFood)
                    {
                        if (item.Quantity > 0)
                        {
                            sideDishes.AppendFormat(ConstantValue.CombinedSideDishOrderDetails, employee.SysId, item.FoodId, item.Quantity);
                        }
                    }
                }
            }

            var insertOrderCriteria = new InsertOrderMealByUserCriteria()
            {
                CreatedDate = DateTime.Now,
                CreatedBy = OrderOwner.SysId,
                TakeAway = TakeAwayChecked,
                ListDetail = mainDishes.Append(ConstantValue.EndRootTag).ToString(),
                Foods = sideDishes.Append(ConstantValue.EndRootTag).ToString()
            };

            OrderResult result = this.SmartCardOrderService.InsertOrderFood(insertOrderCriteria);
            if (result.CommandResult.Code == 0)
            {
                PrepareDataNotificationView(result);
            }
            else
            {
                this.ShowErrorMessage(MainViewModel.CurrentCulture, result.CommandResult.Message);
            }
        }

        /// <summary>
        /// Show notification view after order successful.
        /// </summary>
        /// <param name="result">OrderResult data.</param>
        private void PrepareDataNotificationView(OrderResult result)
        {
            List<Food> selectedFood = new List<Food>();
            foreach (Employee employee in ObservableListEmployee)
            {
                employee.DisplayGeneralOrderInfo = employee.DisplayNotifyMenuName;
                if (employee.ListSideFood != null)
                {
                    selectedFood.AddRange(employee.ListSideFood.Where(x => x.Quantity > 0));
                }
            }

            SelectedSideFood = new ObservableCollection<Food>(selectedFood);
            OrderNo = result.OrderNo;
            MainViewModel.SendMessage(result.OrderId);
            CountDownTimer = ConstantValue.CountDownTimer;
            TimerDigitalClock();
            ShowNotificationView();
        }

        /// <summary>
        /// Display order info after ordered.
        /// </summary>
        /// <param name="employee">Selected Employee.</param>
        /// <param name="currentCulture">Current Culture.</param>
        private void DisplayOrderedMeal(Employee employee, int currentCulture)
        {
            if (employee.SelectedValueDropDown > 0 && employee.SelectedValueDropDown != int.MaxValue)
            {
                if (currentCulture == (int)LanguageLocalization.VietNamCulture)
                {
                    employee.DisplayNotifyMenuName = employee.ListAlternativeFood.Where(x => x.FoodId == employee.SelectedValueDropDown).SingleOrDefault().FoodVnmName;
                }
                else
                {
                    employee.DisplayNotifyMenuName = employee.ListAlternativeFood.Where(x => x.FoodId == employee.SelectedValueDropDown).SingleOrDefault().FoodEngName;
                }
            }
            else
            {
                employee.DisplayNotifyMenuName = employee.DisplayMenuName;
            }

            employee.DisplayGeneralOrderInfo = employee.DisplayNotifyMenuName;
        }

        private List<Food> GetSideFoodBySysId(int sysId)
        {
            var getSideFoodCriteria = new GetSideFoodBySysIdCriteria() { SysId = sysId };
            List<FoodOrder> sideFoods = this.SmartCardOrderService.GetSideFoodBySysId(getSideFoodCriteria);
            List<Food> foodMapping = Mapper.Map<List<FoodOrder>, List<Food>>(sideFoods);
            return foodMapping;
        }

        private void ShowSideFoodDialog()
        {
            SideFoodPopup sideFoodPopup = new SideFoodPopup();
            sideFoodPopup.DataContext = this;
            sideFoodPopup.Owner = Application.Current.MainWindow;
            sideFoodPopup.ShowDialog();
        }

        private void CloseSideFoodDialog(object data)
        {
            if (data != null)
            {
                var currentPopup = (SideFoodPopup)data;
                currentPopup.Close();
            }
        }

        /// <summary>
        /// Display total payment.
        /// </summary>
        /// <param name="foods">List side food.</param>
        /// <returns>Data to display.</returns>
        private string DisplayTotalPayment(List<Food> foods)
        {
            decimal totalPayment = 0;
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
            if (foods != null && foods.Count > 0)
            {
                List<Food> listOrdered = foods.Where(x => x.Quantity > 0).ToList();
                if (listOrdered.Count > 0)
                {
                    foreach (var order in listOrdered)
                    {
                        totalPayment += order.Quantity * order.Price;
                    }
                }
            }

            return totalPayment > 0 ? totalPayment.ToString("#,###", culture.NumberFormat) : "0";
        }

        private void DisplayOrderInfoOfCurrentListSideDish(List<Food> food)
        {
            TotalOrder = food.Sum(x => x.Quantity).ToString();
            TotalPayment = DisplayTotalPayment(food);
        }

        private void ShowOrderView()
        {
            ScreenOrderVisible = CommonResources.Visible;
            ScreenNotificationVisible = CommonResources.Hidden;
        }

        private void ShowNotificationView()
        {
            ScreenOrderVisible = CommonResources.Hidden;
            ScreenNotificationVisible = CommonResources.Visible;
        }

        private void TimerDigitalClock()
        {
            digitalClockDispatcher.Interval = new TimeSpan(0, 0, 1);
            digitalClockDispatcher.Tick += new EventHandler(TimerTick);
            digitalClockDispatcher.Start();
        }

        /// <summary>
        /// Timer click.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            CountDownTimer -= 1;
            SideDishTimerLive = true;
            if (CountDownTimer == 0)
            {
                if (MainViewModel != null)
                {
                    SideDishTimerLive = false;
                    MainViewModel.ExecuteReturnToStandByScreen();
                }

                digitalClockDispatcher.Stop();
            }
        }

        #endregion
    }
}
