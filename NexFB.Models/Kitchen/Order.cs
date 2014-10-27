// --------------------------------------------------------------------------
// <copyright file="Order.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using NexFB.Core.Enums;
    using NexFB.Resources;

    /// <summary>
    /// Order Meal.
    /// </summary>
    [Serializable]
    public class Order
    {
        public DateTime OnDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime DeliverDate { get; set; }

        public int OrderId { get; set; }

        public int OrderNo { get; set; }

        public int CurrentStatusId { get; set; }

        public string CurrentStatusName { get; set; }

        public string Remark { get; set; }

        public string Phone { get; set; }

        public int LocationId { get; set; }

        public string VnmName { get; set; }

        public string EngName { get; set; }

        public string FloorName { get; set; }

        public int SysId { get; set; }

        public string NickName { get; set; }

        public string Code { get; set; }

        public int QuantityOrder { get; set; }

        public bool IsEdited { get; set; }

        public int InvoiceNumber { get; set; }

        public bool IsSelectedMealMenu { get; set; }

        public bool IsSelectedMemberInGroup { get; set; }

        public List<OrderMenu> ListMenu { get; set; }

        public List<OrderMenu> DisplayListMenu
        {
            get
            {
                if (ListMenu.Count > 4)
                {
                    List<OrderMenu> generalOrderList = new List<OrderMenu>();
                    List<OrderMenu> sideDish = ListMenu.Where(x => x.IsSideDish && !x.IsDeleted).ToList();
                    List<OrderMenu> mainMenu = ListMenu.Where(x => !x.IsSideDish && !x.IsDeleted).ToList();

                    string display = string.Empty;
                    if (mainMenu.Count > 0)
                    {
                        display += mainMenu.Sum(x => x.QuantityOrder).ToString() + WinFormMainResource.Main + WinFormMainResource.Space;
                    }

                    if (sideDish.Count > 0)
                    {
                        if (mainMenu.Count > 0)
                        {
                            display += WinFormMainResource.Dash;
                        }

                        display += sideDish.Sum(x => x.QuantityOrder).ToString() + WinFormMainResource.Snack;
                    }

                    OrderMenu generalInfo = new OrderMenu()
                    {
                        MenuVnmName = WinFormMainResource.Total,
                        GeneralInfo = display
                    };
                    generalOrderList.Add(generalInfo);

                    return generalOrderList;
                }
                else
                {
                    return ListMenu;
                }
            }
        }

        public string PrintLabel
        {
            get
            {
                if (CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    return WinFormMainResource.ReceiveBilingual;
                }
                else
                {
                    return WinFormMainResource.DeliverBilingual;
                }
            }
        }

        public string RegisterDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm}", RegisterDate);
            }
        }

        public string ReceiveDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm}", ReceiveDate);
            }
        }

        public string DeliverDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm}", DeliverDate);
            }
        }

        public string CurrentDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);
            }
        }

        public bool HasNote
        {
            get
            {
                return !string.IsNullOrEmpty(Remark);
            }
        }

        public string HasEdited
        {
            get
            {
                if (IsEdited)
                {
                    return WinFormMainResource.Visible;
                }
                else
                {
                    return WinFormMainResource.Hidden;
                }
            }
        }

        public string TicketBackground
        {
            get
            {
                if (InvoiceNumber == 0 || InvoiceNumber == 1)
                {
                    return WinFormMainResource.Black;
                }

                return string.Empty;
            }
        }

        public string TicketStrokeDash
        {
            get
            {
                if (InvoiceNumber == 2)
                {
                    return WinFormMainResource.StrokeDash;
                }

                return string.Empty;
            }
        }

        public string TicketForeground
        {
            get
            {
                if (InvoiceNumber == 0 || InvoiceNumber == 1)
                {
                    return WinFormMainResource.White;
                }

                return string.Empty;
            }
        }

        public int TotalOrder
        {
            get
            {
                int totalOrder = ListMenu.Where(x => !x.IsDeleted).Count();
                return totalOrder;
            }
        }

        public string TotalAmount
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                decimal totalAmount = ListMenu.Sum(x => x.Price);
                if (totalAmount > 0)
                {
                    return string.Format(culture, "{0:C}", totalAmount);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TwoDigitOrderNo
        {
            get
            {
                return OrderNo.ToString().PadLeft(3, '0');
            }
        }

        public string ButtonLabel
        {
            get
            {
                if (CurrentStatusId == 1)
                {
                    return WinFormMainResource.Receive;
                }

                if (CurrentStatusId == 2 || CurrentStatusId == 3)
                {
                    return WinFormMainResource.Export;
                }

                return string.Empty;
            }
        }
    }
}
