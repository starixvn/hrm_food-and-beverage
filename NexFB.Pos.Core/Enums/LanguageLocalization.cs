namespace NexFB.Pos.Core.Enums
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
