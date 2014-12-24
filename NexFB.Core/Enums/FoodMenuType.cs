// ----------------------------------------------------------------------
// <copyright file="FoodMenuType.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/23/2014 2:14:08 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The Food Menu Type ENUM.
    /// </summary>
    public enum FoodMenuType
    {
        /// <summary>
        /// The all item.
        /// </summary>
        [Description("All")]
        All = 0,

        /// <summary>
        /// The Main Dish Only item.
        /// </summary>
        [Description("Main Dish Only")]
        MainDishOnly = 1,

        /// <summary>
        /// The Side Dish Only item.
        /// </summary>
        [Description("Side Dish Only")]
        SideDishOnly = 2,

        /// <summary>
        /// The Has Main Dish item.
        /// </summary>
        [Description("Has Main Dish")]
        HasMainDish = 3,

        /// <summary>
        /// The Has Side Dish item.
        /// </summary>
        [Description("Has Side Dish")]
        HasSideDish = 4
    }
}
