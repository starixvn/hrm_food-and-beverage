namespace NexFB.Models.Kitchen
{
    using System;
    using System.Globalization;
    using NexFB.Resources;

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
                return Price > 0 ? (QuantityOrder * Price).ToString("#,###", culture.NumberFormat) : "N/A";
            }
        }

        public string QuantityPerTotal
        {
            get
            {
                return string.IsNullOrEmpty(GeneralInfo) ? QuantityOrder.ToString() : GeneralInfo;
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "<Pending>")]
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
