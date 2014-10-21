// --------------------------------------------------------------------------
// <copyright file="Food.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.9">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    /// <summary>
    /// Food Item.
    /// </summary>
    public class Food
    {
        public int FoodId { get; set; }

        public string Name 
        {
            get
            {
                return NameVN;
            }
        }

        public string NameVN { get; set; }

        public string NameEN { get; set; }
    }
}
