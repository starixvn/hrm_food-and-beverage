// --------------------------------------------------------------------------
// <copyright file="PendingFoodViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.22">Created</change>
// </history>

using System.Collections.Generic;
using System;
namespace NexFB.WinApp.ViewModels
{
    /// <summary>
    /// Pending Food ViewModel.
    /// </summary>
    public class PendingFoodViewModel : ViewModelBase
    {
        public PendingFoodViewModel()
        {
            Persons = new List<Person>()
            {
                new Person() { Age = 10, Name = "01", OrderDate = DateTime.Now, EmployeeId = 02355, NonVegetarian= 1, Vegetarian = 10, Room="03A", OrderOwner="Rooney", RoomDescription="Phòng pantry", RoomPhone=123456},
                new Person() { Age = 10, Name = "02", OrderDate = DateTime.Now, EmployeeId = 00399, NonVegetarian= 5, Vegetarian = 6, Room="03AB", OrderOwner="Messi", RoomDescription="Sân Nou-Camp", RoomPhone=6789},
                new Person() { Age = 10, Name = "03", OrderDate = DateTime.Now, EmployeeId = 00126, NonVegetarian= 2, Vegetarian = 4, Room="03C", OrderOwner="Ronaldo", RoomDescription="Phòng tập gym", RoomPhone=354},
            };

            Test = new Person() { Age = 10, Name = "01", OrderDate = DateTime.Now, EmployeeId = 02355, NonVegetarian = 1, Vegetarian = 10, Room = "03A", OrderOwner = "Rooney", RoomDescription = "Phòng pantry", RoomPhone = 123456 };
        }

        private List<Person> _lstPerson;

        public List<Person> Persons
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

        private Person _test;

        public Person Test
        {
            get
            {
                return _test;
            }

            set
            {
                if (_test == value)
                {
                    return;
                }

                _test = value;
                OnPropertyChanged("Test");
            }
        }
    }

    public class Person
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string Room { get; set; }
        public string RoomDescription { get; set; }
        public string OrderOwner { get; set; }
        public int EmployeeId { get; set; }
        public int RoomPhone { get; set; }
        public int Vegetarian { get; set; }
        public int NonVegetarian { get; set; }
    }
}
