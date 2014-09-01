// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Class Menu.
    /// </summary>
    public class Menu
    {
        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public string MenuENGName { get; set; }

        public string MenuVNMName { get; set; }

        public int QtyOrd { get; set; }

        public int TotalOrder { get; set; }

        public string QuantityPerTotal
        {
            get
            {
                if (QtyOrd == TotalOrder)
                {
                    return TotalOrder.ToString();
                }
                else
                {
                    string quantityPerTotal = string.Empty;
                    quantityPerTotal = QtyOrd.ToString() + "/" + TotalOrder.ToString();
                    return quantityPerTotal;
                }
            }
        }
    }
}
