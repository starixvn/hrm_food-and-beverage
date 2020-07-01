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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The meal forecast detail report.
    /// </summary>
    [Serializable]
    public class MealForecastDetailModel
    {
        public MealForecastDetailModel()
        {
            this.Meals = new List<MealModel>();
        }

        public int MenuId { get; set; }

        public string MenuVie { get; set; }

        public string MenuEng { get; set; }

        public List<MealModel> Meals { get; set; }

        public bool IsVegetarian { get; set; }

        public int Registration { get; set; }
        public int NonRegistration { get; set; }

        public int ActualOrder { get; set; }

        public double OrderRate
        {
            get
            {
                double orderRate = 0;
                if (ActualOrder < 0)
                {
                    orderRate = -1;
                }
                else
                {
                    if (Registration.Equals(0))
                    {
                        orderRate = -1;
                    }
                    else
                    {
                        orderRate = Math.Round((double)ActualOrder / Registration * 100);
                    }
                }

                return orderRate;
            }
        }
    }
}