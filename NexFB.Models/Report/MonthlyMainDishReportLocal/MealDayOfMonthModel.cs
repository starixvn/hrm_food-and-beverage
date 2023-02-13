// ----------------------------------------------------------------------
// <copyright file="MealDayOfMonthModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 8:47:10 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The meal days of month model.
    /// </summary>
    [Serializable]
    public class MealDayOfMonthModel
    {
        public MealDayOfMonthModel()
        {
            NumberMeals = new List<int>();
        }

        public List<int> NumberMeals { get; set; }

        public List<string> Shifts { get; set; }

        public int TotalQuantity { get; set; }

        public decimal Amount { get; set; }
    }
}
