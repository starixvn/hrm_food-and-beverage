// --------------------------------------------------------------------------
// <copyright file="LocaleChangedEvent.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.15">Created</change>
// </history>

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
