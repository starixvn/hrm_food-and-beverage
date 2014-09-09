// ----------------------------------------------------------------------
// <copyright file="RawMealDateModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 3:13:22 PM">Created.</change>
// </history>

namespace NexFB.Models.MealRegistration
{
    using System;

    /// <summary>
    /// The raw meal date model contain data from database.
    /// </summary>
    public class RawMealDateModel
    {
        public DateTime WorkDate { get; set; }

        public int Week { get; set; }

        public int WeekDay { get; set; }

        public bool? Breakfast { get; set; }

        public bool? Lunch { get; set; }

        public bool? Dinner { get; set; }

        public bool? Supper { get; set; }

        public bool IsDisabled { get; set; }
    }
}
