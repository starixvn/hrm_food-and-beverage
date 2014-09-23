// ----------------------------------------------------------------------
// <copyright file="RawFoodMenuModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:43:48 AM">Created.</change>
// </history>

namespace NexFB.Models.FoodItemManagement
{
    /// <summary>
    /// The raw food menu model.
    /// </summary>
    public class RawFoodMenuModel
    {
        public int FoodId { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }
    }
}
