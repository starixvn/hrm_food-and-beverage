// ----------------------------------------------------------------------
// <copyright file="MainDishPerDayModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/26/2014 2:25:15 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishPerDay
{
    using System.Collections.Generic;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Resources;

    /// <summary>
    /// The main dish per day model.
    /// </summary>
    public class MainDishPerDayModel
    {
        public MainDishPerDayModel()
        {
            MealItems = new List<MealItemMonthModel>();
        }

        public bool IsOffDay { get; set; }

        public string TypeDay
        {
            get
            {
                return IsOffDay ? Lang.OffDay : Lang.WorkDay;
            }
        }

        public List<MealItemMonthModel> MealItems { get; set; }
    }
}
