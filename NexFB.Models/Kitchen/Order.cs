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
    using System.Linq;
    using NexFB.Resources;

    /// <summary>
    /// Order Meal.
    /// </summary>
    public class Order
    {
        public DateTime OnDate { get; set; }

        public DateTime RegisterDate { get; set; }

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

        public List<OrderMenu> ListMenu { get; set; }

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
                int totalOrder = ListMenu.Where(x => x.IsDeleted == false).Count();
                return totalOrder;
            }
        }

        public string ThreeDigitOrderNo
        {
            get
            {
                return OrderNo.ToString().PadLeft(2, '0');
            }
        }
    }
}
