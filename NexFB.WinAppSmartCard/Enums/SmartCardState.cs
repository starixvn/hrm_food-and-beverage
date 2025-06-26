namespace NexFB.WinAppSmartCard.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Smart card State.
    /// </summary>
    public enum SmartcardState
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// The Inserted item.
        /// </summary>
        [Description("Inserted")]
        Inserted = 1,

        /// <summary>
        /// The Ejected item.
        /// </summary>
        [Description("Ejected")]
        Ejected = 2
    }
}
