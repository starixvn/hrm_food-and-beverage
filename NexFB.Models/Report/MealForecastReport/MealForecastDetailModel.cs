// -----------------------------------------------------------------------
// <copyright file="MealForecastDetailModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    /// <summary>
    /// The meal forecast detail report history.
    /// </summary>
    public class MealForecastDetailModel
    {
        public string MenuVie { get; set; }

        public string MenuEng { get; set; }

        public string Meal { get; set; }

        public bool DietType { get; set; }

        public int Registration { get; set; }

        public int ActualOrder { get; set; }

        public int OrderRate { get; set; }
    }
}