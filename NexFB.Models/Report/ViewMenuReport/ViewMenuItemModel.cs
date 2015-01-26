// --------------------------------------------------------------------------
// <copyright file="ViewMenuItemModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.19">Created</change>
// </history>

namespace NexFB.Models.Report.ViewMenuReport
{
    using System;

    /// <summary>
    /// ViewMenuItemModel class.
    /// </summary>
    public class ViewMenuItemModel
    {
        public bool IsVegetarian { get; set; }

        public string MealId { get; set; }

        public string MenuName { get; set; }

        public string FoodVNMName { get; set; }

        public string FoodEngName { get; set; }

        public int OrderNo { get; set; }
    }
}