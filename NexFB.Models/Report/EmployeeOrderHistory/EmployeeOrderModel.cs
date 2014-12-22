// ----------------------------------------------------------------------
// <copyright file="EmployeeOrderModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 1:10:29 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.EmployeeOrderHistory
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.Ordering;

    /// <summary>
    /// Employee order model.
    /// </summary>
    public class EmployeeOrderModel
    {
        public DateTime OnDate { get; set; }

        public string OrderFrom { get; set; }

        public MealModel Meal { get; set; }

        public MealOrderingModel Order { get; set; }

        public Status Status { get; set; }

        public Employee OrderBy { get; set; }

        public List<Menu> Menus { get; set; }
    }
}
