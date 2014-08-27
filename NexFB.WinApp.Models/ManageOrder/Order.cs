// --------------------------------------------------------------------------
// <copyright file="Order.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.27">Created</change>
// </history>

namespace NexFB.WinApp.Models.ManageOrder
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Order entity.
    /// </summary>
    public class Order
    {
        public int No { get; set; }

        public int Age { get; set; }

        public string Name { get; set; }

        public DateTime OrderDate { get; set; }

        public string Room { get; set; }

        public string RoomDescription { get; set; }

        public string OrderOwner { get; set; }

        public string EmployeeCode { get; set; }

        public int RoomPhone { get; set; }

        public int Vegetarian { get; set; }

        public int NonVegetarian { get; set; }

        public string GeneralNote { get; set; }

        public List<MealType> MealType { get; set; }

        public int FoodOrderStatus { get; set; }

        public bool HasNote
        {
            get
            {
                return !string.IsNullOrEmpty(GeneralNote);
            }
        }
    }
}
