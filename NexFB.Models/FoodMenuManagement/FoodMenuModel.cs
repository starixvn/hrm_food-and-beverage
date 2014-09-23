// ----------------------------------------------------------------------
// <copyright file="FoodMenuModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:15:20 AM">Created.</change>
// </history>

namespace NexFB.Models.FoodMenuManagement
{
    /// <summary>
    /// The food menu model.
    /// </summary>
    public class FoodMenuModel
    {
        public int MenuId { get; set; }

        public string MenuVnmName { get; set; }

        public string MenuEngName { get; set; }

        public bool? IsVegetarian { get; set; }

        public string Description { get; set; }

        public double PricePerMeal { get; set; }

        public double PricePerDay { get; set; }

        public bool? IsActive { get; set; }
    }
}
