// -----------------------------------------------------------------------
// <copyright file="TotalMealForecastModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/9">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    /// <summary>
    /// The total meal forecast model.
    /// </summary>
    [Serializable]
    public class TotalMealForecastModel
    {
        public int Registration { get; set; }

        public int ActualOrder { get; set; }

        public double Efficiency
        {
            get
            {
                double efficiency = 0;
                if (ActualOrder < 0)
                {
                    efficiency = -1;
                }
                else
                {
                    if (Registration.Equals(0))
                    {
                        efficiency = -1;
                    }
                    else
                    {
                        efficiency = Math.Round((double)ActualOrder / Registration * 100);
                    }
                }

                return efficiency;
            }
        }
    }
}