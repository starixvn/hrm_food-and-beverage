// ----------------------------------------------------------------------
// <copyright file="MealModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:13:27 PM">Created.</change>
// </history>

namespace NexFB.Models.MealRegistration
{
    /// <summary>
    /// The meal model.
    /// </summary>
    public class MealModel
    {
        public int MealId { get; set; }

        public string Name { get; set; }

        public bool? IsVegetarian { get; set; }
    }
}
