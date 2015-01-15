// -----------------------------------------------------------------------
// <copyright file="MealForecastItemModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/8">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    /// <summary>
    /// The meal forecast report history.
    /// </summary>
    [Serializable]
    public class MealForecastItemModel
    {
        public DateTime OnDate { get; set; }

        public int RegistrationNonVegetarian { get; set; }

        public int ActualOrderNonVegetarian { get; set; }

        public double EfficiencyNonVegetarian { get; set; }

        public int RegistrationVegetarian { get; set; }

        public int ActualOrderVegetarian { get; set; }

        public double EfficiencyVegetarian { get; set; }
    }
}