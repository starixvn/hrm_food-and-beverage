// ----------------------------------------------------------------------
// <copyright file="MealRegisterModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/9/2014 9:47:21 AM">Created.</change>
// </history>

namespace NexFB.Models.MealRegistration
{
    using System.Collections.Generic;

    /// <summary>
    /// The meal register model.
    /// </summary>
    public class MealRegisterModel
    {
        public int Week { get; set; }

        public IList<MealDateModel> WeekMeals { get; set; }
    }
}
