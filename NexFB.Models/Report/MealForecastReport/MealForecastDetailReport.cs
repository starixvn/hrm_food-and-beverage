// -----------------------------------------------------------------------
// <copyright file="MealForecastDetailReport.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Meal forecast detail history report.
    /// </summary>
    [Serializable]
    public class MealForecastDetailReport
    {
        public MealForecastDetailReport()
        {
            this.MealForecastDetails = new List<MealForecastDetailModel>();
            TotalNonVegetarian = new TotalMealForecastModel();
            TotalVegetarian = new TotalMealForecastModel();
            TotalMeal = new TotalMealForecastModel();
        }

        public List<MealForecastDetailModel> MealForecastDetails { get; set; }

        public TotalMealForecastModel TotalNonVegetarian { get; set; }

        public TotalMealForecastModel TotalVegetarian { get; set; }

        public TotalMealForecastModel TotalMeal { get; set; }

        public int TotalItem { get; set; }
    }
}