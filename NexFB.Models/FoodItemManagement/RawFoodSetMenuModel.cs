// ----------------------------------------------------------------------
// <copyright file="RawFoodSetMenuModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:43:48 AM">Created.</change>
//   <change who="Quinn.Vo" date="10/23/2014 4:58:00 PM">Change Menu to Set Menu.</change>
// </history>

namespace NexFB.Models.FoodItemManagement
{
    /// <summary>
    /// The raw food menu model.
    /// </summary>
    public class RawFoodSetMenuModel
    {
        public int FoodId { get; set; }

        public int SetMenuId { get; set; }

        public string SetMenuVnmName { get; set; }

        public string SetMenuEngName { get; set; }
    }
}
