namespace NexFB.WinAppSmartCard.Converter
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// Text Block Custom.
    /// </summary>
    public static class TextBlockCustom
    {
        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached(
            "FormattedText",
            typeof(Inline),
            typeof(TextBlockCustom),
            new PropertyMetadata(null, OnFormattedTextChanged));

        public static Inline GetFormattedText(DependencyObject obj)
        {
            return (Inline)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, Inline value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        /// <summary>
        /// On Formatted Text.
        /// </summary>
        /// <param name="o">Dependency Object.</param>
        /// <param name="e">Dependency Properties changed.</param>
        private static void OnFormattedTextChanged(
            DependencyObject o,
            DependencyPropertyChangedEventArgs e)
        {
            var textBlock = o as TextBlock;
            if (textBlock == null)
            {
                return;
            }

            var inline = (Inline)e.NewValue;
            textBlock.Inlines.Clear();
            textBlock.Inlines.Add(inline);
        }
    }
}
