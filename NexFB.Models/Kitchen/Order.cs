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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using NexFB.Core.Enums;
    using NexFB.Core.Translator;
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

        public int DepartmentId { get; set; }

        public string NickName { get; set; }

        public string Code { get; set; }

        public int QuantityOrder { get; set; }

        public bool IsEdited { get; set; }

        public int InvoiceNumber { get; set; }

        public bool IsSelectedMealMenu { get; set; }

        public bool IsSelectedMemberInGroup { get; set; }

        public bool TakeAway { get; set; }

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
                if (InvoiceNumber == (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice)
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
                if (InvoiceNumber == (int)InvoiceNumberStatus.DeliverOrderFoodInvoice)
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
                if (InvoiceNumber == (int)InvoiceNumberStatus.ReceiveOrderFoodInvoice)
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
                int totalOrder = ListMenu.Where(x => !x.IsDeleted).Sum(x => x.QuantityOrder);
                return totalOrder;
            }
        }

        public string TotalAmount
        {
            get
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("vi-VN");
                decimal totalAmount = ListMenu.Sum(x => x.Price * x.QuantityOrder);
                if (totalAmount > 0)
                {
                    return string.Format(culture, "{0:C}", totalAmount);
                }
                else
                {
                    return "0";
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

                if (CurrentStatusId == (int)OrderStatus.ReceivedOrder || CurrentStatusId == (int)OrderStatus.ReducedOrder)
                {
                    return WinFormMainResource.Export;
                }

                return string.Empty;
            }
        }

        public bool DisabledButton
        {
            get
            {
                if (CurrentStatusId == (int)OrderStatus.EditedOrder)
                {
                    return false;
                }

                return true;
            }
        }

        public string ImageSource
        {
            get
            {
                string imagePath = string.Empty;
                string imageSource = string.Empty;
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

                if (GetResourceNames(imagePath))
                {
                    return imageSource;
                }
                else
                {
                    return "pack://application:,,,/Images/no-avatar.jpg";
                }
            }
        }

        public string TranslateRemark
        {
            get
            {
                if (!string.IsNullOrEmpty(Remark))
                {
                    return FBTranslator.AutoTranslation(Remark);
                }

                return Remark;
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
