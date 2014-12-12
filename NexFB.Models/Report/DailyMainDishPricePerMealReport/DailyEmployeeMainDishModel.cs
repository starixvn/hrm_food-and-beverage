// ----------------------------------------------------------------------
// <copyright file="DailyEmployeeMainDishModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/12/2014 9:37:49 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using NexFB.Models.Entities;
    using NexFB.Models.ShiftMealMapping;

    /// <summary>
    /// The daily main dish of employee model.
    /// </summary>
    public class DailyEmployeeMainDishModel
    {
        public DateTime WorkDate { get; set; }

        public ShiftMealMappingModel Shift { get; set; }

        public Employee Employee { get; set; }

        public int Breakfast { get; set; }

        public int Lunch { get; set; }

        public int Dinner { get; set; }

        public int Supper { get; set; }
    }
}
