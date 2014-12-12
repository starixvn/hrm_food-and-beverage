// ----------------------------------------------------------------------
// <copyright file="TotalPriceModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/12/2014 9:34:36 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    /// <summary>
    /// The total price model.
    /// </summary>
    public class TotalPriceModel
    {
        public int Total { get; set; }

        public int Breakfast { get; set; }

        public int Lunch { get; set; }

        public int Dinner { get; set; }

        public int Supper { get; set; }
    }
}
