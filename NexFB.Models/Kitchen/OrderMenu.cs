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

        public string DisplayPrice
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                if (Price > 0)
                {
                    return string.Format(culture, "{0:C}", Price);
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
                    if (QuantityOrder == TotalOrder)
                    {
                        return TotalOrder.ToString();
                    }
                    else
                    {
                        string quantityPerTotal = string.Empty;
                        quantityPerTotal = QuantityOrder.ToString() + WinFormMainResource.QuantityPerTotal + TotalOrder.ToString();
                        return quantityPerTotal;
                    }
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
                string setName = WinFormMainResource.DisplayDeletedOrder;
                if (!IsDeleted)
                {
                    setName = MenuVnmName + WinFormMainResource.QuantityPerOrder + QuantityOrder.ToString();
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
                    setName += WinFormMainResource.DisplayDeletedName;
                }

                return setName;
            }
        }
    }
}
