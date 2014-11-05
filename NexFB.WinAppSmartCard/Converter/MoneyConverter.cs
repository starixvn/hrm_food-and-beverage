// --------------------------------------------------------------------------
// <copyright file="MoneyConverter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.23">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Converter
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Data;

    /// <summary>
    /// Money Converter.
    /// </summary>
    public class MoneyConverter : IValueConverter
    {
        /// <summary>
        /// Decimal to money format.
        /// </summary>
        /// <param name="value">Value formatted.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Object parameter.</param>
        /// <param name="culture">Current culture info.</param>
        /// <returns>Value after formatting.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                decimal price = (decimal)value;
                CultureInfo currentCulture = CultureInfo.GetCultureInfo("vi-VN"); 
                if (price > 0)
                {
                    return string.Format(currentCulture, "{0:C}", price);
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }
    }
}
