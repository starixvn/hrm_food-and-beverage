// -----------------------------------------------------------------------
// <copyright file="MealTimeModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/3/20">Created.</change>
// </history>

namespace NexFB.Models.MealTimeManagement
{
    /// <summary>
    /// The meal time management model.
    /// </summary>
    public class MealTimeModel
    {
        public int MealId { get; set; }

        public string RegisterTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}