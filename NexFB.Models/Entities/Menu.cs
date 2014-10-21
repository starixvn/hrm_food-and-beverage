// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Menu Entity.
    /// </summary>
    public class Menu
    {
        public int MenuId { get; set; }

        public string MenuCode { get; set; }

        public string NameVN { get; set; }

        public string NameEN { get; set; }

        public bool IsMainMenu { get; set; }

        public bool IsHaveAlternativeMenu { get; set; }

        public bool IsSideDish { get; set; }

        public int QuantityOrder { get; set; }

        public double TotalAmount { get; set; }

        public string Name
        {
            get
            {
                return NameEN;
            }
        }

        public IList<Food> FoodItems { get; set; }
    }
}
