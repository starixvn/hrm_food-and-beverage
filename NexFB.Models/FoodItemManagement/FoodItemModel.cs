// ----------------------------------------------------------------------
// <copyright file="FoodItemModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/23/2014 9:12:30 AM">Created.</change>
// </history>

namespace NexFB.Models.FoodItemManagement
{
    using System.Collections.Generic;
    using NexFB.Models.FoodMenuManagement;

    /// <summary>
    /// The food item model.
    /// </summary>
    public class FoodItemModel
    {
        public FoodItemModel()
        {
            FoodMenus = new List<FoodMenuModel>();
        }

        public int FoodId { get; set; }

        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public IList<FoodMenuModel> FoodMenus { get; set; }
    }
}
