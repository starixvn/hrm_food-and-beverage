// ----------------------------------------------------------------------
// <copyright file="MonthlyLocalReportModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 8:56:33 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using System.Collections.Generic;
    using Core.Enums;

    /// <summary>
    /// The monthly main dish report local model.
    /// </summary>
    [Serializable]
    public class MonthlyLocalReportModel
    {
        public MonthlyLocalReportModel()
        {
            Meals = new List<MealMonthModel>();
        }

        public DateTime Month { get; set; }

        public bool IsGroupByEmployee { get; set; }

        public GroupByReport GroupBy { get; set; }

        public List<MealMonthModel> Meals { get; set; }

        public MealDayOfMonthModel Total { get; set; }

        public MealDayOfMonthModel WeekDays { get; set; }

        public int TotalItem { get; set; }
    }
}