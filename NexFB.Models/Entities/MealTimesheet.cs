// ----------------------------------------------------------------------
// <copyright file="MealTimesheet.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/11/2014 10:02:36 AM">Created.</change>
// </history>

namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// The meal timesheet.
    /// </summary>
    public class MealTimesheet
    {
        public int MealId { get; set; }

        public string MealVnmName { get; set; }

        public string MealEngName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime RegisterTime { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
