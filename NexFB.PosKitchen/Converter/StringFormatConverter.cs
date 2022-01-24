namespace NexFB.PosKitchen.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string orderSummary = (string)value;
            string[] orderDetails = orderSummary.Split('-');
            var span = new Span();

            if (orderDetails.Length > 1)
            {
                const byte DefaultFontSize = 15;
                string mainMenu = orderDetails[0];
                string alternativeMenu = orderDetails[1];

                if (string.IsNullOrEmpty(alternativeMenu.Trim()))
                {
                    if (!string.IsNullOrEmpty(mainMenu))
                    {
                        span.Inlines.Add(new Run(mainMenu) { Foreground = Brushes.Red, FontSize = DefaultFontSize });
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(mainMenu.Trim()))
                    {
                        span.Inlines.Add(new Run(alternativeMenu) { Foreground = Brushes.Black, FontSize = DefaultFontSize });
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
                const byte FontSize = 17;
                span.Inlines.Add(new Run(orderDetails[0]) { Foreground = Brushes.Red, FontSize = FontSize });
            }

            return span;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}