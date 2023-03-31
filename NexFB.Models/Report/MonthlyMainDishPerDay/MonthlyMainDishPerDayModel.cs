// ----------------------------------------------------------------------
// <copyright file="MonthlyMainDishPerDayModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/26/2014 2:27:24 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System;
    using System.Collections.Generic;
    using Core.Enums;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;

    /// <summary>
    /// The Monthly Main Dish Per Day model.
    /// </summary>
    [Serializable]
    public class MonthlyMainDishPerDayModel
    {
        public MonthlyMainDishPerDayModel()
        {
            TypeDays = new List<MainDishPerDayModel>();
        }

        public DateTime Month { get; set; }

        public bool IsGroupByEmployee { get; set; }

        public GroupByReport GroupBy { get; set; }

        public List<MainDishPerDayModel> TypeDays { get; set; }

        public MealDayOfMonthModel Total { get; set; }

        public MealDayOfMonthModel WeekDays { get; set; }

        public int TotalItem { get; set; }

        public List<BreakRuleOrder> BreakRuleOrders { get; set; }

    }
}