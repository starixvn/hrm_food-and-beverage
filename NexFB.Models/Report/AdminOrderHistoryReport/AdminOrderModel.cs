// ----------------------------------------------------------------------
// <copyright file="AdminOrderModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 3:15:00 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.AdminOrderHistoryReport
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.Ordering;

    /// <summary>
    /// Admin order model.
    /// </summary>
    [Serializable]
    public class AdminOrderModel
    {
        public AdminOrderModel()
        {
            Menus = new List<Menu>();
        }

        public DateTime OnDate { get; set; }

        public DateTime OrderTime { get; set; }

        public string OrderFrom { get; set; }

        public MealModel Meal { get; set; }

        public MealOrderingModel Order { get; set; }

        public Status Status { get; set; }

        public Employee OrderBy { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
