// --------------------------------------------------------------------------
// <copyright file="MealType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.8.27">Created</change>
// </history>

namespace NexFB.WinApp.Models.ManageOrder
{
    /// <summary>
    /// Meal Types.
    /// </summary>
    public class MealType
    {
        public string Type { get; set; }

        public int Quantity { get; set; }

        public int Total { get; set; }

        public string QuantityPerTotal
        {
            get
            {
                if (Quantity == Total)
                {
                    return Total.ToString();
                }
                else
                {
                    string quantityPerTotal = string.Empty;
                    quantityPerTotal = Quantity.ToString() + "/" + Total.ToString();
                    return quantityPerTotal;
                }
            }
        }
    }
}
