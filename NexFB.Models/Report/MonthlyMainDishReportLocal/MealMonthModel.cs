// ----------------------------------------------------------------------
// <copyright file="MealMonthModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 8:53:48 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.MonthlyMainDishReportLocal
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.MealRegistration;

    /// <summary>
    /// The meals in month model.
    /// </summary>
    [Serializable]
    public class MealMonthModel
    {
        public MealMonthModel()
        {
            MealItems = new List<MealItemMonthModel>();
        }

        public MealModel Meal { get; set; }

        public List<int> TotalQuantityPerDays { get; set; }

        public int TotalQuantityOfMeal { get; set; }

        public decimal TotalAmountOfMeal { get; set; }

        public List<MealItemMonthModel> MealItems { get; set; }
    }
}