namespace NexFB.PosKitchen.DependencyProperties
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// Custom TextBlock.
    /// </summary>
    public static class TextBlockInline
    {
        public static readonly DependencyProperty FormattedTextProperty =
    DependencyProperty.RegisterAttached(
        "FormattedText",
        typeof(Inline),
        typeof(TextBlockInline),
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
            if (inline == null)
            {
                textBlock.Inlines.Clear();
            }
            else
            {
                textBlock.Inlines.Add(inline);
            }
        }
    }
}
