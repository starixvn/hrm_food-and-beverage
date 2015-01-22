// -----------------------------------------------------------------------
// <copyright file="MealForecastReportModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/7">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Meal forecast model.
    /// </summary>
    [Serializable]
    public class MealForecastReportModel
    {
        public MealForecastReportModel()
        {
            MealForecasts = new List<MealForecastItemModel>();
            TotalNonVegetarian = new TotalMealForecastModel();
            TotalVegetarian = new TotalMealForecastModel();
            TotalMeal = new TotalMealForecastModel();
        }

        public List<MealForecastItemModel> MealForecasts { get; set; }

        public TotalMealForecastModel TotalNonVegetarian { get; set; }

        public TotalMealForecastModel TotalVegetarian { get; set; }

        public TotalMealForecastModel TotalMeal { get; set; }

        public int TotalItem { get; set; }
    }
}