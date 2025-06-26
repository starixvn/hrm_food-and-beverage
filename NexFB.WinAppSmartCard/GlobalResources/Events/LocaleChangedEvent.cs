namespace NexFB.WinAppSmartCard.GlobalResources.Events
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Justification = "Reviewed. Suppression is OK here.")]
    public delegate void LocaleChangedEventHander(object sender, LocaleChangedEvent e);

    /// <summary>
    /// Locale Change Event.
    /// </summary>
    public class LocaleChangedEvent : EventArgs
    {
        public LocaleChangedEvent(Locale newLocale)
        {
            NewLocale = newLocale;
        }

        public Locale NewLocale { get; set; }
    }
}
