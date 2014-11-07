// --------------------------------------------------------------------------
// <copyright file="StringFormatConverter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.9">Created</change>
// </history>

namespace NexFB.PosKitchen.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// String Format Converter.
    /// </summary>
    public class StringFormatConverter : IValueConverter
    {
        /// <summary>
        /// Convert text block string.
        /// </summary>
        /// <param name="value">Value convert.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">Current culture.</param>
        /// <returns>Returned object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                string orderSummary = (string)value;
                string[] orderDetails = orderSummary.Split('-');
                var span = new Span();

                if (orderDetails.Length > 1)
                {
                    string mainMenu = orderDetails[0];
                    string alternativeMenu = orderDetails[1];

                    if (string.IsNullOrEmpty(alternativeMenu.Trim()))
                    {
                        if (!string.IsNullOrEmpty(mainMenu))
                        {
                            span.Inlines.Add(new Run(mainMenu) { Foreground = Brushes.Red, FontSize = 15 });
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(mainMenu.Trim()))
                        {
                            span.Inlines.Add(new Run(alternativeMenu) { Foreground = Brushes.Black, FontSize = 15 });
                        }
                        else
                        {
                            span.Inlines.Add(new Run(mainMenu) { Foreground = Brushes.Red });
                            span.Inlines.Add(new Run(" - ") { FontWeight = FontWeights.Bold });
                            span.Inlines.Add(new Run(alternativeMenu) { Foreground = Brushes.Black });
                        }
                    }
                }
                else
                {
                    span.Inlines.Add(new Run(orderDetails[0]) { Foreground = Brushes.Red, FontSize = 17 });
                }

                return span;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
