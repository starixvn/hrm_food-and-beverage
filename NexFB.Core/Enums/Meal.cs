// ----------------------------------------------------------------------
// <copyright file="Meal.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:39:56 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The meal ENUM.
    /// </summary>
    public enum Meal
    {
        /// <summary>
        /// None item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// The breakfast.
        /// </summary>
        [Description("Breakfast")]
        Breakfast = 1,

        /// <summary>
        /// The lunch.
        /// </summary>
        [Description("Lunch")]
        Lunch = 2,

        /// <summary>
        /// The dinner.
        /// </summary>
        [Description("Dinner")]
        Dinner = 3,

        /// <summary>
        /// The supper.
        /// </summary>
        [Description("Supper")]
        Supper = 4
    }
}
