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
    using System;
    using System.Globalization;
    using NexFB.Resources;

    /// <summary>
    /// Class Menu.
    /// </summary>
    [Serializable]
    public class OrderMenu
    {
        public int OrderId { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }

        public int QuantityOrder { get; set; }

        public int TotalOrder { get; set; }

        public bool IsOwner { get; set; }

        public int SysId { get; set; }

        public string NickName { get; set; }

        public string Code { get; set; }

        public bool IsDeleted { get; set; }

        public decimal Price { get; set; }

        public bool IsMainMenu { get; set; }

        public bool IsSideDish { get; set; }

        public string GeneralInfo { get; set; }

        public bool IsVegetarian { get; set; }

        public string DisplayPrice
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                if (Price > 0)
                {
                    var price = QuantityOrder * Price;
                    return price.ToString("#,###", culture.NumberFormat);
                }
                else
                {
                    return "N/A";
                }
            }
        }

        public string QuantityPerTotal
        {
            get
            {
                if (string.IsNullOrEmpty(GeneralInfo))
                {
                    return QuantityOrder.ToString();
                }
                else
                {
                    return GeneralInfo;
                }
            }
        }

        public string SetMenuAndQuantity
        {
            get
            {
                return IsDeleted ? WinFormMainResource.DisplayDeletedName : MenuVnmName;
            }
        }

        public string DisplayQuantity
        {
            get
            {
                return IsDeleted ? WinFormMainResource.DisplayDeletedName : QuantityOrder.ToString();
            }
        }

        public int DisplayRowHeight
        {
            get
            {
                //// Grater than 20 and less than 40 letter: set Height = 40, 
                //// Less than 20 letter Height = 28
                //// Grater than 40 letter Height = 52
                var textLength = SetMenuAndQuantity.Length;
                if (textLength <= 40 && textLength > 20)
                {
                    return 40;
                }
                else
                {
                    return textLength > 40 ? 52 : 28;
                }
            }
        }
    }
}
