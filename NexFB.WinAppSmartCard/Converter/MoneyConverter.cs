namespace NexFB.WinAppSmartCard.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using NexFB.WinAppSmartCard.Core.Constants;

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
            CultureInfo currentCulture = CultureInfo.GetCultureInfo(ConstantValue.VietNameseCultureCode);
            return value != null ? ((decimal)value).ToString("#,###", currentCulture.NumberFormat) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotImplementedException();
        }
    }
}
