namespace NexFB.Pos.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Invoice Type.
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// The none item.
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Main Menu.
        /// </summary>
        [Description("MainMenu")]
        MainMenu = 1,

        /// <summary>
        /// Alternative Menu.
        /// </summary>
        [Description("AlternativeMenu")]
        AlternativeMenu = 2,
    }
}
