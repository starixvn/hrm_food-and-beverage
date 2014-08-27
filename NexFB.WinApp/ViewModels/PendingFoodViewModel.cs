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
            ReceiveOrderFoodCommand = new RelayCommand((x) => CanExecuteReceiveOrderFoodCommand(), (x) => ShowMessage(x));
            ExportOrderFoodCommand = new RelayCommand((x) => CanExecuteExportOrderFoodCommand(), (x) => ShowMessage(x));
            Persons = new List<Order>()
            {
                new Order()
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
                    MealType = new List<MealType>()
                    {
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 1" },
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 2" },
                        new MealType() { Quantity = 1, Total = 5, Type = "Chay" },
                    }
                },
                new Order()
                {
                    Age = 10,
                    Name = "02",
                    OrderDate = DateTime.Now,
                    EmployeeCode = "00399",
                    Room = "03B",
                    OrderOwner = "Ronaldo",
                    RoomDescription = "Santiago Bernabeu", 
                    RoomPhone = 123456,
                    FoodOrderStatus = 2,
                    MealType = new List<MealType>()
                    {
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 1" }, 
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 2" },
                        new MealType() { Quantity = 1, Total = 5, Type = "Chay" },
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 1" }, 
                        new MealType() { Quantity = 2, Total = 5, Type = "Trưa 2" },
                        new MealType() { Quantity = 1, Total = 5, Type = "Chay" },
                    }
                }
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

        public void ShowMessage(object obj)
        {
            if (obj == null)
            {
                MessageBox.Show("null");
            }
            else
            {
                MessageBox.Show(obj.ToString());
            }
        }
    }
}
