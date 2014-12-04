// ----------------------------------------------------------------------
// <copyright file="MealPlanningModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/2/2014 1:39:24 PM">Created.</change>
// </history>

namespace NexFB.Models.FoodMealPlanning
{
    using System;
    using NexFB.Models.FoodMenuManagement;
    using NexFB.Models.SetMenuManagement;

    /// <summary>
    /// The Meal Planning model.
    /// </summary>
    public class MealPlanningModel
    {
        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        public bool IsExist { get; set; }

        public bool IsDisable { get; set; }

        public bool IsCurrentMeal { get; set; }

        public SetMenuModel SetMenu { get; set; }

        public FoodMenuModel Menu { get; set; }
    }
}
