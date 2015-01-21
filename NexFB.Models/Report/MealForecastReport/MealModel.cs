// -----------------------------------------------------------------------
// <copyright file="MealModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Models.Report.MealForecastReport
{
    using System;

    /// <summary>
    /// The meal model.
    /// </summary>
    [Serializable]
    public class MealModel
    {
        public int MenuId { get; set; }

        public int MealId { get; set; }

        public string MealName { get; set; }
    }
}