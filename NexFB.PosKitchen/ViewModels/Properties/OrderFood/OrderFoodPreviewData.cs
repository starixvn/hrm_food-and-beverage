// --------------------------------------------------------------------------
// <copyright file="OrderFoodPreviewData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.10">Created</change>
// </history>

namespace NexFB.PosKitchen.ViewModels.Properties.OrderFood
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using NexFB.Pos.Core.Constants;
    using NexFB.Pos.Core.Models;

    /// <summary>
    /// Order Food Preview Data.
    /// </summary>
    public class OrderFoodPreviewData
    {
        /// <summary>
        /// Prepare data for kitchen preview.
        /// </summary>
        /// <param name="kitchenEmployeeInfo">Employee kitchen Info.</param>
        /// <returns>List of order preview.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public List<OrderPreview> PrepareDataForKitchenPreview(List<KitchenEmployeeInfo> kitchenEmployeeInfo)
        {
            kitchenEmployeeInfo = kitchenEmployeeInfo.Where(x => (x.ListMainMenuOrderDeatails.Count > 0 && x.MenuId != -1)
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
                                orderPreview.Price = orderPreview.UnitPrice.ToString("#,###", culture.NumberFormat);
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
                                orderPreview.Price = orderPreview.UnitPrice.ToString("#,###", culture.NumberFormat);
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
