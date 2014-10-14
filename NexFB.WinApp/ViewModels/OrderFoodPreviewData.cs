// --------------------------------------------------------------------------
// <copyright file="OrderFoodPreviewData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.10">Created</change>
// </history>

namespace NexFB.WinApp.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using NexFB.WinApp.Core.Constants;
    using NexFB.WinApp.Core.Models;

    /// <summary>
    /// Order Food Preview Data.
    /// </summary>
    public class OrderFoodPreviewData
    {
        /// <summary>
        /// Prepare data for kitchen preview.
        /// </summary>
        /// <param name="kitchenEmployeeInfo">Employee kitchen Info.</param>
        /// <param name="isVnmCulture">Check for VN culture.</param>
        /// <returns>List of order preview.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public List<OrderPreview> PrepareDataForKitchenPreview(List<KitchenEmployeeInfo> kitchenEmployeeInfo, bool isVnmCulture)
        {
            kitchenEmployeeInfo = kitchenEmployeeInfo.Where(x => x.ListMainMenuOrderDeatails.Count > 0
                || x.ListAlternativeMenuOrderDeatails.Count > 0).ToList();
            List<OrderPreview> dataPreviews = new List<OrderPreview>();
            int orderNo = 1;
            CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN"); 
            if (kitchenEmployeeInfo != null)
            {
                foreach (var item in kitchenEmployeeInfo)
                {
                    if (item.ListMainMenuOrderDeatails.Count > 0)
                    {
                        foreach (var mainDishes in item.ListMainMenuOrderDeatails)
                        {
                            var orderPreview = new OrderPreview()
                            {
                                OrderNo = orderNo,
                                NickName = item.NickName,
                                Dishes = mainDishes.SelectedMenuItemName,
                                Quantity = mainDishes.SelectedMenuItemQuantity,
                                UnitPrice = mainDishes.SelectedMenuItemQuantity * mainDishes.SelectedMenuItemPrice,
                                Color = Constant.Red
                            };
                            if (orderPreview.UnitPrice > 0)
                            {
                                orderPreview.Price = isVnmCulture ? string.Format(culture, "{0:C}", orderPreview.UnitPrice) : string.Format("{0:C}", orderPreview.UnitPrice);
                            }
                            else
                            {
                                orderPreview.Price = Constant.NA;
                            }

                            dataPreviews.Add(orderPreview);
                            orderNo++;
                        }
                    }

                    if (item.ListAlternativeMenuOrderDeatails.Count > 0)
                    {
                        foreach (var mainDishes in item.ListAlternativeMenuOrderDeatails)
                        {
                            var orderPreview = new OrderPreview()
                            {
                                OrderNo = orderNo,
                                NickName = item.NickName,
                                Dishes = mainDishes.SelectedMenuItemName,
                                Quantity = mainDishes.SelectedMenuItemQuantity,
                                UnitPrice = mainDishes.SelectedMenuItemQuantity * mainDishes.SelectedMenuItemPrice,
                                Color = Constant.Black
                            };
                            if (orderPreview.UnitPrice > 0)
                            {
                                orderPreview.Price = isVnmCulture ? string.Format(culture, "{0:C}", orderPreview.UnitPrice) : string.Format("{0:C}", orderPreview.UnitPrice);
                            }
                            else
                            {
                                orderPreview.Price = Constant.NA;
                            }

                            dataPreviews.Add(orderPreview);
                            orderNo++;
                        }
                    }
                }
            }

            return dataPreviews;
        }
    }
}
