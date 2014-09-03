// --------------------------------------------------------------------------
// <copyright file="OrderMenu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.3">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Class Menu.
    /// </summary>
    public class OrderMenu
    {
        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }

        public int QtyOrd { get; set; }

        public int TotalOrder { get; set; }

        public bool IsOwner { get; set; }

        public int SysId { get; set; }

        public string NickName { get; set; }

        public string Code { get; set; }

        public bool IsDeleted { get; set; }

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

        public string SetMenuAndQuantity
        {
            get
            {
                string setName = "---";
                if (!IsDeleted)
                {
                    setName = MenuVnmName + " x " + QtyOrd.ToString();
                }
                return setName;
            }
        }

        public string DisplayName
        {
            get
            {
                string setName = NickName;
                if (IsDeleted)
                {
                    setName += " (X)";
                }
                return setName;
            }
        }
    }
}
