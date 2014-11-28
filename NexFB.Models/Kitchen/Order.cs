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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Resources;

    /// <summary>
    /// Order Meal.
    /// </summary>
    [Serializable]
    public class Order
    {
        private const string PosDateFormat = "{0:dd/MM/yyyy HH:mm}";

        public DateTime OnDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime DeliverDate { get; set; }

        public DateTime RejectedDate { get; set; }

        public int OrderId { get; set; }

        public int OrderNo { get; set; }

        public int CurrentStatusId { get; set; }

        public string CurrentStatusName { get; set; }

        public string Remark { get; set; }

        public string RemarkVnm { get; set; }

        public string Phone { get; set; }

        public int LocationId { get; set; }

        public string VnmName { get; set; }

        public string EngName { get; set; }

        public string FloorName { get; set; }

        public int SysId { get; set; }

        public int DepartmentId { get; set; }

        public string NickName { get; set; }

        public string Code { get; set; }

        public int QuantityOrder { get; set; }

        public bool IsEdited { get; set; }

        public int InvoiceNumber { get; set; }

        public bool IsSelectedMealMenu { get; set; }

        public bool IsSelectedMemberInGroup { get; set; }

        public bool TakeAway { get; set; }

        public string PhoneAvailable
        {
            get
            {
                return string.IsNullOrEmpty(Phone) ? Constants.Hidden : Constants.Visible;
            }
        }

        public string DisplayRemak
        {
            get
            {
                if (TakeAway)
                {
                    return !string.IsNullOrEmpty(RemarkVnm) ? string.Format("{0}, {1}", WinFormMainResource.TakeAway, RemarkVnm) : WinFormMainResource.TakeAway;
                }
                else
                {
                    return RemarkVnm;
                }
            }
        }

        public List<OrderMenu> ListMenu { get; set; }

        public List<OrderMenu> DisplayListMenu
        {
            get
            {
                if (ListMenu.Count > 4 || ListMenu.Count == 0)
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
                return CurrentStatusId == (int)OrderStatus.NewOrder ? WinFormMainResource.ReceiveBilingual : WinFormMainResource.DeliverBilingual;
            }
        }

        public string RegisterDateString
        {
            get
            {
                return string.Format(PosDateFormat, RegisterDate);
            }
        }

        public string ReceiveDateString
        {
            get
            {
                return string.Format(PosDateFormat, ReceiveDate);
            }
        }

        public string DeliverDateString
        {
            get
            {
                return string.Format(PosDateFormat, DeliverDate);
            }
        }

        public string RejectedDateString
        {
            get
            {
                return string.Format(PosDateFormat, RejectedDate);
            }
        }

        public string CurrentDateString
        {
            get
            {
                return string.Format(PosDateFormat, DateTime.Now);
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
                return IsEdited ? WinFormMainResource.Visible : WinFormMainResource.Hidden;
            }
        }

        public string TicketBackground
        {
            get
            {
                return InvoiceNumber == (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice ? WinFormMainResource.Black : string.Empty;
            }
        }

        public string TicketStrokeDash
        {
            get
            {
                return InvoiceNumber == (int)InvoiceNumberStatus.DeliverOrderFoodInvoice ? WinFormMainResource.StrokeDash : string.Empty;
            }
        }

        public string TicketForeground
        {
            get
            {
                return InvoiceNumber == (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice ? WinFormMainResource.White : string.Empty;
            }
        }

        public int TotalOrder
        {
            get
            {
                return ListMenu.Where(x => !x.IsDeleted).Sum(x => x.QuantityOrder);
            }
        }

        public string TotalAmount
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                decimal totalAmount = ListMenu.Sum(x => x.Price * x.QuantityOrder);

                return totalAmount > 0 ? totalAmount.ToString("#,###", culture.NumberFormat) : "0";
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
                string text = string.Empty;
                if (CurrentStatusId == (int)OrderStatus.NewOrder)
                {
                    text = WinFormMainResource.Receive.ToUpper();
                }

                if (CurrentStatusId == (int)OrderStatus.ReceivedOrder || CurrentStatusId == (int)OrderStatus.ReducedOrder)
                {
                    text = WinFormMainResource.Export.ToUpper();
                }

                return text;
            }
        }

        public bool DisabledButton
        {
            get
            {
                return CurrentStatusId != (int)OrderStatus.EditedOrder;
            }
        }

        public string ImageSource
        {
            get
            {
                string imagePath = string.Empty;
                string imageSource = string.Empty;
                string unavailableAvartarSource = "pack://application:,,,/Images/no-avatar.jpg";
                switch (DepartmentId)
                {
                    case (int)DepartmentIdentity.Nexcel:
                        imageSource = string.Format("pack://application:,,,/Images/Nexcel/{0}.jpg", Code.Trim());
                        imagePath = string.Format("images/nexcel/{0}.jpg", Code.Trim());
                        break;
                    case (int)DepartmentIdentity.Sms:
                        imageSource = string.Format("pack://application:,,,/Images/SMS/{0}.jpg", Code.Trim());
                        imagePath = string.Format("images/sms/{0}.jpg", Code.Trim());
                        break;
                    case (int)DepartmentIdentity.HongMA:
                        imageSource = string.Format("pack://application:,,,/Images/HongMa/{0}.jpg", Code.Trim());
                        imagePath = string.Format("images/hongma/{0}.jpg", Code.Trim());
                        break;
                    default:
                        imageSource = "pack://application:,,,/Images/no-avatar.jpg";
                        imagePath = "images/no-avatar.jpg";
                        break;
                }

                return GetResourceNames(imagePath) ? imageSource : unavailableAvartarSource;
            }
        }

        /// <summary>
        /// Get Resource of application.
        /// </summary>
        /// <param name="resourcePath">Find Resource path.</param>
        /// <returns>Boolean to check.</returns>
        private bool GetResourceNames(string resourcePath)
        {
            string[] resourceNames;
            string assemblyName = "NexFB.PosKitchen";
            var assembly = Assembly.Load(assemblyName);
            string resName = assemblyName + ".g.resources";
            using (var stream = assembly.GetManifestResourceStream(resName))
            {
                using (var reader = new System.Resources.ResourceReader(stream))
                {
                    resourceNames = reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key).ToArray();
                }
            }

            return resourceNames.Contains(resourcePath);
        }
    }
}
