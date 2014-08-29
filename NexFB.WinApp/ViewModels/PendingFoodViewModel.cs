// --------------------------------------------------------------------------
// <copyright file="PendingFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.27">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using NexFB.Criteria.Employee;
    using NexFB.Models.Authority;
    using NexFB.Services.Implement;
    using NexFB.WinApp.Core.Commands;
    using NexFB.WinApp.Models.ManageOrder;

    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class PendingFoodViewModel : ViewModelBase
    {
        private List<Order> _lstPerson;

        public PendingFoodViewModel()
        {
            Random rnd = new Random();
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ShowMessage(x));
            ExportOrderFoodCommand = new RelayCommand((x) => CanExecuteExportOrderFoodCommand(), (x) => ShowMessage(x));
            var mealType1 = new MealType()
            {
                Quantity = 2,
                Total = 5,
                Type = "Trưa 1"
            };

            var listMealType = new List<MealType>()
            {
                mealType1, mealType1
            };
            var order1 = new Order()
            {
                Age = 10,
                Name = "01",
                OrderDate = DateTime.Now,
                EmployeeCode = "02355",
                Room = "03A",
                OrderOwner = "Rooney",
                RoomDescription = "Phòng pantry",
                RoomPhone = 123456,
                GeneralNote = "Một phần không giá, thêm canh",
                FoodOrderStatus = 1,
                MealType = listMealType
            };
            var order2 = new Order()
            {
                Age = 10,
                Name = "01",
                OrderDate = DateTime.Now,
                EmployeeCode = "02355",
                Room = "03A",
                OrderOwner = "Rooney",
                RoomDescription = "Phòng pantry",
                RoomPhone = 123456,
                GeneralNote = "Một phần không giá, thêm canh",
                FoodOrderStatus = 2,
                MealType = listMealType
            };

            Persons = new List<Order>()
            {
                order1, order1, order2, order1, order2, order2, order1, order1, order2, order1, order1,
            };
        }

        public ICommand ExportOrderFoodCommand { get; set; }

        public ICommand ReceiveOrderFoodCommand { get; set; }

        public List<Order> Persons
        {
            get
            {
                return _lstPerson;
            }

            set
            {
                if (_lstPerson == value)
                {
                    return;
                }

                _lstPerson = value;
                OnPropertyChanged("Persons");
            }
        }

        public bool CanExecuteReceiveOrderFoodCommand()
        {
            return true;
        }

        public bool CanExecuteExportOrderFoodCommand()
        {
            return true;
        }

        /// <summary>
        /// Get Data from DB.
        /// </summary>
        /// <param name="obj">Parameter value.</param>
        public void ShowMessage(object obj)
        {
            var criteria = new GetEmployeeByCodeCriteria()
            {
                Code = "02355",
                DepartmentId = "1"
            };
            var _employeeService = new EmployeeService();

            UserSession user = _employeeService.GetUserByCode(criteria);
            MessageBox.Show(user.ToString());
        }
    }
}
