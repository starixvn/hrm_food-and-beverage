// --------------------------------------------------------------------------
// <copyright file="LanguageLocalization.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.28">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Language Localization.
    /// </summary>
    public enum LanguageLocalization
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// English Culture.
        /// </summary>
        [Description("EnglishCulture")]
        EnglishCulture = 1,

        /// <summary>
        /// VietNam Culture.
        /// </summary>
        [Description("VietNamCulture")]
        VietNamCulture = 2,

        /// <summary>
        /// China Culture.
        /// </summary>
        [Description("ChinaCulture")]
        ChinaCulture = 3,
    }
}
